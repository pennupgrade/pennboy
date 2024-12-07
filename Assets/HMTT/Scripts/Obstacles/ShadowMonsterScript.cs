using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMonsterScript : MonoBehaviour
{
    //shadow monster follows a temple-run monster type behavior
    //three states: off-screen, side-screen, on player
    //advances state when player is slowed down by obstacle OR player velocity falls below threshold for certain amount of time
    //lowers state when player dodges obstacles for certain amount of time AND player velocity exceeds threshold for certain amount of time
    //off-screen, no behavior
    //side-screen, launches foxfires
    //on-player, kills player

    [SerializeField] float velMin;
    [SerializeField] float stateDownVel;
    [SerializeField] float stateUpVel;
    [SerializeField] float stateUpTime;
    private float currUpTime;
    [SerializeField] float stateDownTime;
    private float currDownTime;

    [SerializeField] Transform firePivot;
    [SerializeField] GameObject foxfire;
    [SerializeField] float foxfireCD;
    [SerializeField] int foxfireCDVariance;
    [SerializeField] int minFoxfire;
    [SerializeField] int maxFoxfire;
    private float currCD;

    [SerializeField]
    private float moveSpeed;
    private int state; //0 = offscreen, 1 = onscreen, 2 = on-player
    [SerializeField] float onScreenPos;

    private MovementScript player;
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        player = FindObjectOfType<MovementScript>();
        state = 1;
        currDownTime = 0;
        currUpTime = 0;
        currCD = foxfireCD + Random.Range(-foxfireCDVariance, foxfireCDVariance);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.getVelocity().x < stateDownVel)
        {
            currDownTime += Time.deltaTime;
        } else
        {
            currDownTime = 0;
        }
        if (player.getVelocity().x > stateUpVel)
        {
            currUpTime += Time.deltaTime;
        } else
        {
            currUpTime = 0;
        }
        switchState();
        Vector3 target;
        switch(state)
        {
            case 0:
                target = new Vector3(player.transform.position.x - onScreenPos * 2, this.transform.position.y, this.transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                break;
            case 1:
                target = new Vector3(player.transform.position.x - onScreenPos, this.transform.position.y, this.transform.position.z);
                transform.position = target;
                currCD -= Time.deltaTime;
                fire();
                break;
            case 2:
                target = new Vector3(player.transform.position.x, this.transform.position.y, this.transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                break;
        }
    }

    private void fire()
    {
        if (currCD <= 0 && state == 1)
        {
            int randInt = Random.Range(minFoxfire, maxFoxfire);
            for (int i = 0; i < randInt; i++)
            {
                GameObject foxfireInstance = Instantiate(foxfire, firePivot.transform);
                foxfireInstance.transform.position = firePivot.transform.position;
            }
            currCD = foxfireCD + Random.Range(-foxfireCDVariance, foxfireCDVariance);
        }
    }

    private void switchState()
    {
        if (currDownTime > stateDownTime || player.getVelocity().x < velMin)
        {
            state++;
        }
        if (currUpTime > stateUpTime)
        {
            state--;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

        }
    }
}
