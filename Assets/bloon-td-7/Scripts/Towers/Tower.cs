using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] protected float cooldown = 1f;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float range = 7.5f;
    [SerializeField] protected int cost = 100;
    protected List<GameObject> enemiesInRange = new List<GameObject>();
    protected float remainingCooldown;
    [SerializeField] protected int[] UpgradeCosts = { 100,200,300,400 };
    protected int level = 0;
    protected bool selectedToSacrifice = false;
    [SerializeField] private GameObject sacrificeIcon;


    private AudioSource audioSource;
    private AudioClip laserShootSound;
    /// <summary>
    /// range collision detection
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        GameObject hit = collision.gameObject;
        if (hit.tag == "BTD7Enemy")
        {
            enemiesInRange.Add(hit);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        enemiesInRange.Remove(collision.gameObject);
    }

    /// <summary>
    /// Gets the target from enemies in a towers range.
    /// </summary>
    protected GameObject GetTarget()
    {
        GameObject enemyMax = null;
        for (int i = 0; i < enemiesInRange.Count; i++)
        {
            if (enemiesInRange[i] == null)
            {
                enemiesInRange.RemoveAt(i);
                i--;
            }
            else
            {
                if (enemyMax == null)
                    enemyMax = enemiesInRange[i];
                else if (enemyMax.GetComponent<Enemy>().DistanceTravelled < enemiesInRange[i].GetComponent<Enemy>().DistanceTravelled)
                {
                    enemyMax = enemiesInRange[i];
                }
            }
        }
        return enemyMax;
    }


    // Start is called before the first frame update
    protected virtual void Start()
    {
        remainingCooldown = cooldown;
        audioSource = BTD7.GameManager.instance.waveManager.GetComponent<AudioSource>();
        laserShootSound = BTD7.GameManager.instance.laserShootSound;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (remainingCooldown < cooldown)
        {
            remainingCooldown += Time.deltaTime;
            return;
        }

        // get the target enemy
        GameObject SelectedTarget = GetTarget();
        if (SelectedTarget == null) { return; }

        // reset cooldown
        remainingCooldown = 0;

        // attack
        Attack(SelectedTarget);
        MakeSound();
    }

    protected virtual void MakeSound()
    {
        audioSource.clip = laserShootSound;
        audioSource?.Play();
    }

    protected virtual void Attack(GameObject target) {

    }

    /// <summary>
    /// Upgrades the tower level when given a money amount spent. Upgrade funcitonality to be implemented in children classes.
    /// </summary>
    /// <param name="cost">Money sacrificed</param>
    public void CalcLevel (int cost)
    {
        int level = GetLevel(cost);
        for (int i = 0; i <= level; i++)
        {
            Upgrade(level);
        }
    }

    public int GetLevel(int cost)
    {
        int i = 0;
        while (i < 4 && cost >= UpgradeCosts[i])
        {
            i++;
        }
        return i;
    }

    protected virtual void Upgrade(int level)
    {

    }

    /// <summary>
    /// Returns the total cost of the tower based off of its level
    /// </summary>
    /// <returns></returns>
    public int GetCost()
    {
        int sum = cost;
        for (int i = 0; i < level; i++)
            sum += UpgradeCosts[i];
        return sum;
    }

    /// <summary>
    /// Toggles between selecting this tower to be sacrificed or not
    /// </summary>
    /// <returns>True if tower selected to sacrifice, false if tower deselected</returns>
    public bool ToggleSacrifice()
    {
        selectedToSacrifice = !selectedToSacrifice;
        sacrificeIcon.SetActive(selectedToSacrifice);
        return selectedToSacrifice;
    }

    /// <summary>
    /// Deletes this tower
    /// </summary>
    public void Die()
    {
        Destroy(gameObject);
    }
}
