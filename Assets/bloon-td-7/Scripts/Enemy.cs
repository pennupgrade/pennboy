using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    public int waypointIndex = 0;
    protected List<Vector3> waypoints;
    protected Rigidbody rb;
    public int dmg;
    public int health;
    public int id;
    public int moneyWorth;
    public string modifiers;
    private const float WAYPOINT_CHANGE_DISTANCE = 0.01f;
    public bool isCamo { get; protected set; }
    public float size;
    private float renderSizeY;
    private bool canStart = false;
    private Vector3 targetPos;

    public float DistanceTravelled { get; protected set; }

    private void Start()
    {
        //Initialize(15f, 1, 3, 0, 1, false, 5);
        Initialize(moveSpeed, dmg, health, id, moneyWorth, isCamo, size);
    }

    /// <summary>
    /// Initializes a new enemy with the given parameters. To be called when instatiating new enemies.
    /// </summary>
    public void Initialize(float moveSpeed, int dmg, int health, int id, int moneyWorth, bool isCamo, float size)
    {
        this.health = health;
        this.id = id;
        this.dmg = dmg;
        this.moveSpeed = moveSpeed;
        this.isCamo = isCamo;
        this.moneyWorth = moneyWorth;
        this.size = size + Random.Range(-0.5f,0.5f);
        //height given random deviations to prevent ui glitching
        transform.localScale = new Vector3(size, size + Random.Range(-1f,4f), size);
        renderSizeY = size + Random.Range(-1f, 4f);
        waypoints = new List<Vector3>();
        this.gameObject.layer = 2;

        Transform waypointListTransform = GameObject.Find("EnemyWaypoints").transform;

        // Temp script to get all waypoints until game manager is implemented
        foreach (Transform child in waypointListTransform)
        {
            waypoints.Add(child.position);
        }

        transform.position = waypoints[0];

        rb = GetComponent<Rigidbody>();

        DistanceTravelled = Mathf.PI * (Random.Range(1, 100) / 100f);
        targetPos = transform.position;
        canStart = true;
    }

    protected void Update()
    {
        if (!canStart) return;
        DistanceTravelled += Time.deltaTime * Mathf.Sqrt(moveSpeed) * 1.5f;
        if (CalcDistance(targetPos, waypoints[waypointIndex+1]) <= WAYPOINT_CHANGE_DISTANCE)
        {
            targetPos = waypoints[waypointIndex + 1];
            waypointIndex++;
            if (waypointIndex >= waypoints.Count-1)
            {
                // Deal dmg damage to player health
                GameManager.instance.healthManager.TakeDamage(dmg);
                Die();
                GameManager.instance.moneyManager.SpendMoney(moneyWorth);
            }
        }
    }

    private void FixedUpdate()
    {
        targetPos = Vector3.MoveTowards(targetPos, waypoints[waypointIndex + 1], moveSpeed * Time.deltaTime);
        float sinpos = Mathf.Abs(Mathf.Sin(DistanceTravelled));
        float sinsize = Mathf.Abs(Mathf.Sin(DistanceTravelled - (Mathf.PI / 5)));
        transform.localScale = new Vector3(size, (renderSizeY * .8f) + (sinsize * renderSizeY * .2f), size);
        transform.position = targetPos + new Vector3(0, (sinpos * 8f) + (renderSizeY/4), 0);
    }

    private float CalcDistance(Vector3 pos1, Vector3 pos2)
    {
        float dx = pos2.x - pos1.x;
        float dy = pos2.y - pos1.y;
        float dz = pos2.z - pos1.z;

        return Mathf.Sqrt((dx*dx)+(dy*dy)+(dz*dz));
    }

    /// <summary>
    /// Destroys this enemy's game object
    /// </summary>
    public void Die()
    {
        GameManager.instance.moneyManager.EarnMoney(moneyWorth);
        Destroy(gameObject);
    }

    public void Damage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            // Add money count to game manager
            Die();
        }
    }

}
