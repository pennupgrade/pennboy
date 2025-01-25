using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ElectricityStorage : MonoBehaviour
{
    public static ElectricityStorage Instance { get; private set; }

    public GameObject electricity;
    public int n_electricity = 20;
    public float splitLikelihood = 0.6f;
    public int n_split_ = 2;
    public int n_trav_ = 6;
    public int n_consider = 2;
    public float distanceLimit_ = 3;
    public float duration = 2;

    [SerializeField]
    List<GameObject> electricitys = new List<GameObject>();
    HashSet<int> activeElectricityID = new HashSet<int>();


    void Awake()
    {
        // Check if there is already an instance of this class
        if (Instance == null)
        {
            Instance = this;   // Set this instance as the singleton instance
            DontDestroyOnLoad(gameObject); // Optional: keep it between scenes

            // prepare electricity
            ReadyElectricity();
        }
        else
        {
            Destroy(gameObject); // Destroy this object if an instance already exists
        }

    }


    //Start is called before the first frame update
    public void ReadyElectricity()
    {
        for (int i = 0; i < n_electricity; i++)
        {
            GameObject l = Instantiate(electricity);
            l.transform.SetParent(transform);
            l.SetActive(false);
            electricitys.Add(l);
        }
    }

    public void EffectElectricity(GameObject enemy)
    {
        //Debug.Log("Thunder");
        // get tuple of affected enemies
        List<(GameObject, GameObject)> pairOfEnemies = ChooseEnemies(enemy);

        //reserve electricity id
        List<int> eID = reserve_eID(pairOfEnemies.Count);
        List<(Transform, Transform, Transform, Transform, Transform)> info4returningElectricity = new List<(Transform, Transform, Transform, Transform, Transform)>();

        // create elctric arc between them
        //Debug.Log("pair " + pairOfEnemies.Count + " eid " + eID.Count);
        for (int i = 0; i < eID.Count; i++)
        {
            (GameObject first, GameObject second) = pairOfEnemies[i];
            CreateElectricity(eID[i], first, second, true, info4returningElectricity);
        }

        // damage on Enemies & turn off electricity
        StartCoroutine(TurnOffElectricity(eID, pairOfEnemies, info4returningElectricity));
        
    }

    List<int> reserve_eID(int n)
    {
        List<int> eID = new List<int>();

        for (int i = 0; i < n_electricity; i++)
        {
            if (activeElectricityID.Contains(i)) continue;
            activeElectricityID.Add(i);
            eID.Add(i);
            n--;
            if (n == 0) break;
        }
        return eID;
    }

    IEnumerator TurnOffElectricity(List<int> eID,
        List<(GameObject, GameObject)> pairOfEnemies,
        List<(Transform, Transform, Transform, Transform, Transform)> info4returningElectricity)
    {
        yield return new WaitForSeconds(duration);

        /* Damage the enemy at the end */
        DamageEnemies(pairOfEnemies);

        foreach (int i in eID)
        {
            Destroy(electricitys[i]);
            electricitys[i] = Instantiate(electricity);
            electricitys[i].SetActive(false);
            electricitys[i].transform.SetParent(transform);
            //electricitys[i].SetActive(false);
        }
        foreach (int i in eID)
        {
            activeElectricityID.Remove(i);
        }
    }

    public List<(GameObject, GameObject)> ChooseEnemies(GameObject enemy)
    {
        /* 
         * get closest enemies around me
         * choose one depending on 
         */

        //return:
        List<(GameObject, GameObject)> gameObjectPairs = new List<(GameObject, GameObject)>();

        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        HashSet<GameObject> setEnemies = new HashSet<GameObject>();
        foreach (GameObject other in allEnemies) setEnemies.Add(other);

        int n_split = n_split_;
        int n_trav = n_trav_;
        float distanceLimit = distanceLimit_;

        HashSet<GameObject> origins = new HashSet<GameObject>();
        origins.Add(enemy);
        setEnemies.Remove(enemy);

        while (n_trav > 0 && origins.Count > 0)
        {
            //Debug.Log("n_trav=" + n_trav + " origins=" + origins.Count);

            HashSet<GameObject> newOrigins = new HashSet<GameObject>();
            foreach (GameObject origin in origins)
            {
                Vector3 originPos = origin.transform.position;
                List<GameObject> filtered = setEnemies
                    .Where(obj => (obj.transform.position.y > originPos.y) && (Vector3.Distance(originPos, obj.transform.position) < distanceLimit) )
                    .OrderBy(obj => Vector3.Distance(originPos, obj.transform.position))
                    .Take(n_consider)
                    .ToList();
                if (filtered.Count == 0)
                {
                    filtered = setEnemies
                    .Where(obj => Vector3.Distance(originPos, obj.transform.position) < distanceLimit)
                    .Take(n_consider)
                    .ToList();
                }
                if (filtered.Count == 0) {
                    //Debug.Log("breaking because zero filtered");
                    continue;
                }

                GameObject chosen = null;
                GameObject newOrigin = null;
                if (n_split > 0 && filtered.Count >= 2 && Random.Range(0f, 1f) > splitLikelihood)
                {
                    // split
                    //Debug.Log("split");
                    chosen = filtered[0];
                    newOrigin = filtered[1];
                    //Debug.Log("chose: " + chosen.name);
                    //Debug.Log("newOrigin: " + newOrigin.name);
                }
                else
                {
                    // random choice
                    chosen = filtered[Random.Range(0, Mathf.Min(n_consider, filtered.Count))];
                    //Debug.Log("chose: " + chosen.name);
                }

                gameObjectPairs.Add((origin, chosen));
                setEnemies.Remove(chosen);
                newOrigins.Add(chosen);
                n_trav -= 1;

                if (newOrigin != null)
                {
                    gameObjectPairs.Add((origin, newOrigin));
                    setEnemies.Remove(newOrigin);
                    newOrigins.Add(newOrigin);
                    n_split -= 1;
                    n_trav -= 1;
                }
            }
            origins = newOrigins;

        }

        return gameObjectPairs;
    }

    public void CreateElectricity(int index, GameObject first, GameObject second, bool straight, List<(Transform, Transform, Transform, Transform, Transform)> info4returningElectricity)
    /* return info4returningElectricity, which contains information of pos used for each electricity bezier, 
     * which must be returned back to the relevant parent once we are done using it */
    {
        //Debug.Log("index " + index);
        electricitys[index].SetActive(true);

        Transform electricity = electricitys[index].transform;

        Transform Pos1 = electricity.Find("Pos1");
        if (Pos1 == null) Debug.Log("pos1, ", Pos1);
        if (first == null) Debug.Log("first, ", first);
        Pos1.SetParent(first.transform);
        Pos1.localPosition = Vector3.zero;

        Transform Pos4 = electricity.Find("Pos4");
        Pos4.SetParent(second.transform);
        Pos4.localPosition = Vector3.zero;

        Transform Pos2 = electricity.Find("Pos2");
        Transform Pos3 = electricity.Find("Pos3");

        if (straight)
        {
            Pos2.SetParent(first.transform);
            Pos2.localPosition = Vector3.zero;

            Pos3.SetParent(second.transform);
            Pos3.localPosition = Vector3.zero;
        }
        else
        {
            Pos2.SetParent(first.transform);
            Pos2.localPosition = Vector3.zero;

            Pos3.SetParent(second.transform);
            Pos3.localPosition = Vector3.zero;
        }

        info4returningElectricity.Add((electricity, Pos1, Pos2, Pos3, Pos4));
    }

    void DamageEnemies(List<(GameObject, GameObject)> pairOfEnemies)
    {
        HashSet<GameObject> enemies = new HashSet<GameObject>();
        foreach ((GameObject first, GameObject second) in pairOfEnemies)
        {
            enemies.Add(first);
            enemies.Add(second);
        }

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null)
            {
                Debug.Log("Accesing dead enemy");
            }
            else
            {
                enemy.GetComponent<BombGame.Enemy>().TakeDamage(3);
            }
        }

    }
}
