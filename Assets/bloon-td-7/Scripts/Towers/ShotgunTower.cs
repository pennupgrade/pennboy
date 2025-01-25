using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunTower : Tower
{
    protected float projectileSpeed = 240f;
    protected int projectilePierce = 2;
    protected int numExtraBullets = 1;
    protected float attackSpread = 25f;
    protected override void Start()
    {
        base.Start();

        UpgradeCosts = new int[] { 250, 350, 550, 750, 1000};
        cost = 150;
    }
    protected override void Attack(GameObject target)
    {
        if (target == null) return;
        Vector3 targetPos = target.transform.position;
        Projectile projectile = Instantiate(BTD7.GameManager.instance.projectile, transform.position, transform.rotation).GetComponent<Projectile>();
        projectile.Initialize(damage, projectileSpeed, projectilePierce, targetPos);
        float angle = Mathf.Atan2(targetPos.z-transform.position.z, targetPos.x-transform.position.x);
        float magnitude = (targetPos-new Vector3(0, targetPos.y, 0)).magnitude;
        float radAttackSpread = Mathf.Deg2Rad * attackSpread;
        for (int i = 1; i <= numExtraBullets; ++i)
        {
            Debug.Log("Angle: " + (Mathf.Rad2Deg * angle) + " : A1: " + (Mathf.Rad2Deg*(angle + radAttackSpread * i)) + " : A2: " + (Mathf.Rad2Deg * (angle - radAttackSpread * i)));
            Projectile projectile2 = Instantiate(BTD7.GameManager.instance.projectile, transform.position, transform.rotation).GetComponent<Projectile>();
            projectile2.Initialize(damage, projectileSpeed, projectilePierce, new Vector3(transform.position.x+Mathf.Cos(angle+radAttackSpread*i), 0, transform.position.z+Mathf.Sin(angle+radAttackSpread*i)));
            Projectile projectile3 = Instantiate(BTD7.GameManager.instance.projectile, transform.position, transform.rotation).GetComponent<Projectile>();
            projectile3.Initialize(damage, projectileSpeed, projectilePierce, new Vector3(transform.position.x+Mathf.Cos(angle-radAttackSpread*i), 0, transform.position.z+Mathf.Sin(angle-radAttackSpread*i)));
        }
    }

    protected override void Upgrade(int level)
    {
        Debug.Log("Upgraded to level " + level);
        switch (level)
        {
            case 0:
                return;
            case 1:
                numExtraBullets = 2;
                attackSpread = 17f;
                return;
            case 2:
                projectilePierce = 4;
                damage = 3;
                cooldown = 1;
                return;
            case 3:
                numExtraBullets = 3;
                attackSpread = 9f;
                return;
            case 4:
                projectilePierce = 10;
                cooldown = 0.8f;
                damage = 7;
                return;
            default:
                numExtraBullets = 5;
                attackSpread = 3f;
                return;
        }
    }
}
