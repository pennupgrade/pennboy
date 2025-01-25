using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PewTower : Tower
{
    [SerializeField] protected float projectileSpeed = 240f;
    protected float timeBetweenShots = 0.3f;
    protected float attackCooldownTimer = 0;
    protected int numExtraShotsInBurst = 2;
    protected int numShots = 2;
    protected int projectilePierce = 1;
    protected GameObject savedTarget = null;
    protected override void Start()
    {
        base.Start();
        UpgradeCosts = new int[] { 150, 350, 550, 800 };
        cost = 75;
    }
    protected override void Attack(GameObject target)
    {
        if (numShots == numExtraShotsInBurst)
        {
            numShots = 0;
            savedTarget = target;
        }
        if (target == null) return;
        Projectile projectile = Instantiate(BTD7.GameManager.instance.projectile, transform.position, transform.rotation).GetComponent<Projectile>();
        projectile.Initialize(damage, projectileSpeed, projectilePierce, target.transform.position);
    }

    protected override void Update()
    {
        if (numShots < numExtraShotsInBurst)
        {
            attackCooldownTimer += Time.deltaTime;
            if (attackCooldownTimer >= timeBetweenShots)
            {
                attackCooldownTimer = 0;
                Attack(savedTarget);
                numShots++;
            }
            return;
        }
        base.Update();
    }

    protected override void Upgrade(int level)
    {
        switch (level)
        {
            case 0:
                return;
            case 1:
                projectilePierce++;
                damage++;
                projectileSpeed = 300f;
                return;
            case 2:
                projectilePierce += 2;
                cooldown *= 0.75f;
                timeBetweenShots = 0.2f;
                numExtraShotsInBurst++;
                numShots = numExtraShotsInBurst;
                projectileSpeed = 400f;
                return;
            case 3:
                damage += 3;
                cooldown *= 0.75f;
                timeBetweenShots = 0.15f;
                numExtraShotsInBurst++;
                numShots = numExtraShotsInBurst;
                projectileSpeed = 375f;
                return;
            case 4:
                cooldown *= 0.5f;
                damage += 7;
                projectilePierce += 5;
                timeBetweenShots = 0.1f;
                projectileSpeed = 500f;
                return;
            default:
                cooldown *= 0.25f;
                timeBetweenShots = 0.05f;
                damage += 1;
                projectilePierce += 10;
                return;
        }
    }
}
