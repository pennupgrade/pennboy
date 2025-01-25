using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private float waveCooldown;
    [SerializeField] private float globalTimer;
    private bool waveOccurring = false;
    [SerializeField] private int waveIndex;
    private List<Spawner> spawners;
    private int spawnersCreated;
    [SerializeField] private TextMeshProUGUI roundText;

    // Start is called before the first frame update
    private void Start()
    {
        waveCooldown = 1f;
        globalTimer = 0;
        waveOccurring = false;
        waveIndex = 0;
        spawnersCreated = 0;
        spawners = new List<Spawner>();
    }
    // Update is called once per frame
    /// <summary>
    /// EnemyInfo(float moveSpeed, int dmg, int health, int moneyWorth, Color color, float size = 5, bool isCamo = false, bool canTeleport)
    /// </summary>
    public EnemyInfo[] enemyList =
    {
        new EnemyInfo(10f,1,1,5, new Color32(0,255,0,255)),               //0 - slime
        new EnemyInfo(25f,1,1,10, new Color32(125,209,123, 255)),        //1 - goblin
        new EnemyInfo(8f,4,5,20, new Color32(21, 92, 20, 255),8),         //2 - orcs
        new EnemyInfo(6f,10,15,30,new Color32(70, 89, 70, 255),12),        //3 - ogres
        new EnemyInfo(20f,1,1,5,new Color32(255,255,255,255)),       //4 skeleton
        new EnemyInfo(10f,10,5,5, new Color32(64, 255, 150,255)),   //5 elf
        new EnemyInfo(40f,2,1,5, new Color32(222, 182, 250,255)),  //6 fairy
        new EnemyInfo(50f,15,3,15, new Color32(117, 12, 5,255)),    //7 demon
        new EnemyInfo(10f,3,4,5, new Color32(100,100,100,255)),    //8 dwarf
        new EnemyInfo(20f,20,5,10, new Color32(40, 96, 250,255)),   //9 wizard
        new EnemyInfo(35f,40,12,25, new Color32(139, 155, 199,255)), //10 light wizard
        new EnemyInfo(35f,40,12,25, new Color32(0, 0, 46,255)), //11 dark wizard
        new EnemyInfo(50f,100,25,50, new Color32(114, 0, 252,255),8, false, true), //12 master wizard
        new EnemyInfo(100f,1000,100,100,new Color32(255,0,0,255),20), //13 dragon
        new EnemyInfo(40f,10,1,20, new Color32(0,0,0,255),4, false, true), //14 the flash
        new EnemyInfo(5f, 1000, 5000, 1000000, new Color32(255,255,255,255), 30) //15 god
        //new EnemyInfo(30f,10,10,100,Color.cyan),        // fast assassain enemy
        //new EnemyInfo(100f,0,10000,0,Color.black),       //4 - distraction enemy
        //new EnemyInfo(3,100,1000,1000,new Color(61, 110, 173),30), //5 - boss enemy
    };
    /// <summary>
    /// WaveInfo(       all are in one string
    /// enemyIds,
    /// enemyCount,
    /// spacing,        (time in seconds between enemy spawn)
    /// time               (when cluster spawns)
    /// </summary>
    public WaveInfo[] waves =
    {
        //tutorial wave, starts counting at 0, enter wave index for wave you want

        //wave 0 - slimes
        new WaveInfo(
                "0",
                "20",
                "0.6",
                "0"
            ),
        //wave 1 - slime + skeletons
        new WaveInfo(
                "0,4",
                "10,6",
                "0.5,0.4",
                "0,3"
            ),
        //wave 2 - many goblins
        new WaveInfo(
                "1",
                "50",
                "0.3",
                "0"
            ),
        //wave 3 - slime + goblins
        new WaveInfo(
                "0,1,0,1",
                "20,10,15,8",
                "0.5,0.2,0.5,0.2",
                "0,1,2.5,4"
            ),
        //wave 4 - orcs
        new WaveInfo(
            "1,2",
            "20,3",
            "0.3,2",
            "2,0"
            ),
        //wave 5 - elf + goblin
        new WaveInfo(
            "1,5",
            "30,10",
            "0.4,2",
            "0,1"
            ),
        //wave 6 - goblin + orc + ogre
        new WaveInfo(
            "1,2,3",
            "40,20,5",
            "0.4,1,3",
            "0,2,4"
            ),
        //wave 7 - fairy attack
        new WaveInfo(
            "6",
            "20",
            "0.5",
            "0"
            ),
        //wave 8 - fairy + elf + goblin
        new WaveInfo(
            "1,5,6",
            "100,20,20",
            "0.3,0.5,0.5",
            "0,1,1"
            ),
        //wave 9 - all
        new WaveInfo(
            "0,1,2,3,4,5,6",
            "100,100,10,10,200,30,30",
            "0.5,0.5,2,2,0.3,1,1",
            "0,0,1,1,0,0.5,0.5"
            ),
        //wave 10 - demons
        new WaveInfo(
            "7",
            "10",
            "2",
            "0"),
        //wave 11 - dwarfs
        new WaveInfo(
            "1,4,8",
            "100,40,20",
            "0.4,0.5,0.5",
            "0,0,1"),
        //wave 12 - wizard
        new WaveInfo(
            "5,6,9",
            "100,100,10",
            "0.5,0.5,2",
            "0,0,0.25"
            ),
        //wave 13 - wizard invasion
        new WaveInfo(
            "9",
            "50",
            "1",
            "0"
            ),
        //wave 14 - light and dark wizards
        new WaveInfo(
            "10,11",
            "25,27",
            "1,1",
            "0,0.5"
            ),
        //wave 15 - master wizard
        new WaveInfo(
            "12",
            "1",
            "0",
            "0"
            ),
        //wave 16 - EVERYTHING
        new WaveInfo(
            "0,1,2,3,4,5,6,7,8,9,10,11,12,13,14",
            "100,100,10,10,100,50,25,25,100,50,25,25,10,5,1",
            "0.3,0.4,2,2,0.6,1,2,2,0.8,0.6,2,2,4,15,1",
            "0,0,2,2,0,2,2,4,0,3,6,6,7,15,30"
            ),
        //wave 17 god
        new WaveInfo(
            "15",
            "1",
            "1",
            "0"
            ),
        //wave 18
        new WaveInfo(
            "14",
            "10",
            "2",
            "0"
            )
    };
     
    void Update()
    {
        //stop waves after final wave
        if(waveIndex >= waves.Count()) { return; }
        roundText.text = "Round:\n" + (waveIndex+1).ToString();
        //inbetween waves, wait until timer reached
        if (globalTimer < waveCooldown && !waveOccurring)
        {
            globalTimer += Time.deltaTime;
        }
        else
        {
            //spawn wave
            if (!waveOccurring)
            {   
                //Debug.Log(waveIndex);
                WaveInfo currWave = waves[waveIndex];
                for (int i = 0; i < waves[waveIndex].enemyIdList.Count(); i++)
                {
                    new Spawner(currWave.enemyIdList[i], currWave.enemyCount[i], currWave.spacing[i], currWave.time[i]);
                }
                //Instantiate(waveList.transform.GetChild(waveIndex).gameObject, transform.position, transform.rotation);
                waveOccurring = true;
                globalTimer = 0;
            } else
            { //update wave while wave is occurring
                for (int i = 0; i < spawners.Count; i++)
                {
                    spawners[i].Update();
                }
                if (spawners.Count == 0 && spawnersCreated == waves[waveIndex].enemyIdList.Count())
                {
                    waveOccurring = false;
                    waveIndex++;
                    spawnersCreated = 0;
                }
            }
        }

    }

    public class EnemyInfo
    {
        public float moveSpeed;
        public int dmg;
        public int health;
        public int moneyWorth;
        public bool isCamo;
        public float size;
        public bool canTeleport;
        public Color32 color;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moveSpeed">How fast enemies move</param>
        /// <param name="dmg">The damage enemies deal after passing the map</param>
        /// <param name="health">The health of the enemy</param>
        /// <param name="moneyWorth">The money the enemy drops after dying</param>
        /// <param name="color">The color of the enemy</param>
        /// <param name="size">The size of the enemy</param>
        /// <param name="isCamo">Whether enemies are camoflauged or not</param>
        public EnemyInfo(float moveSpeed, int dmg, int health, int moneyWorth, Color32 color, float size = 5, bool isCamo = false, bool canTeleport = false)
        {
            this.moveSpeed = moveSpeed;
            this.dmg = dmg;
            this.health = health;
            this.moneyWorth = moneyWorth;
            this.isCamo = isCamo;
            this.size = size;
            this.color = color;
            this.canTeleport = canTeleport;
        }
    }

    public class WaveInfo
    {
        public int[] enemyIdList;
        public int[] enemyCount;
        public float[] spacing;
        public float[] time;


        /// <summary>
        /// All arrays are connected such that the index of each array represents a cluster of enemies.
        /// </summary>
        /// <param name="enemyIdList">
        /// List of enemies that will appear in order. Values correspond to index in the EnemyList array
        /// </param>
        /// <param name="enemyCount">
        /// List of how many of each enemy will spawn corresponding to the index
        /// </param>
        /// <param name="spacing">
        /// The space between each enemies in the cluster.
        /// </param>
        /// <param name="time">
        /// The time at which the cluster spawns.
        /// </param>
        public WaveInfo(string enemyIdList, string enemyCount, string spacing, string time)
        {
            this.enemyIdList = enemyIdList?.Split(',')?.Select(int.Parse)?.ToArray();
            this.enemyCount = enemyCount?.Split(',')?.Select(int.Parse)?.ToArray();
            this.spacing = spacing?.Split(',')?.Select(float.Parse)?.ToArray();
            this.time = time?.Split(',')?.Select(float.Parse)?.ToArray();
        }

    }
    public class Spawner
    {
        public List<GameObject> enemies = new List<GameObject>();
        public float timer;
        int enemyId;
        int enemySpawned = 0;
        int enemyCount;
        float spacing;
        float start_time;
       
        //keeps track of individual time for each spawner
        public void Update()
        {
            if (timer >= start_time + spacing || enemySpawned == 0 && timer == start_time)
            {
                EnemyInfo enemyInfo = BTD7.GameManager.instance.waveManager.enemyList[enemyId];
                GameObject newEnemy = Instantiate(BTD7.GameManager.instance.enemy,
                    BTD7.GameManager.instance.waveManager.gameObject.transform.position,
                    BTD7.GameManager.instance.waveManager.gameObject.transform.rotation);
                newEnemy.GetComponent<Enemy>().Initialize(
                    enemyInfo.moveSpeed,
                    enemyInfo.dmg,
                    enemyInfo.health,
                    enemyId,
                    enemyInfo.moneyWorth,
                    enemyInfo.color,
                    enemyInfo.isCamo,
                    enemyInfo.size + Random.Range(-0.3f, 0.3f),
                    enemyInfo.canTeleport
                    );
                
                //newEnemy.GetComponent<Renderer>().material.color = enemyInfo.color;
                enemies.Add(newEnemy);
                enemySpawned++;
                timer = start_time;
            } else if (timer < start_time + spacing && enemySpawned < enemyCount)
            {
                timer += Time.deltaTime;
            } 
            //Debug.Log(checkAllNull(enemies));
            if (checkAllNull(enemies) && enemies.Count >= enemyCount)
            {
                Debug.Log("killed cluster");
                BTD7.GameManager.instance.waveManager.spawners.Remove(this);
            }

        }
        public Spawner (int enemyId, int enemyCount, float spacing, float time) 
        {
            // 0 <= enemyId <= enemyList.Count()
            this.enemyId = System.Math.Min(BTD7.GameManager.instance.waveManager.enemyList.Count() - 1, System.Math.Max(0, enemyId));
            this.enemyCount = System.Math.Max(0, enemyCount);
            this.spacing = System.Math.Max(0.1f, spacing);
            this.start_time = System.Math.Max(0, time);
            BTD7.GameManager.instance.waveManager.spawners.Add(this);
            BTD7.GameManager.instance.waveManager.spawnersCreated++;
        }



        private bool checkAllNull(List<GameObject> l)
        {
            
            foreach (GameObject obj in l)
            {
                if (obj != null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}


