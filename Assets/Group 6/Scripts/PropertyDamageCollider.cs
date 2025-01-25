using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyDamageCollider : MonoBehaviour
{
    [SerializeField] private float damageSlope = 1;
    [SerializeField] private float damageOffset = 0;
    [SerializeField] private float maxDamage = 100;
    [SerializeField] private float cooldownTime;
    [SerializeField] private AudioSource crashSound;
    private float currCooldownTime = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currCooldownTime > 0) {
            currCooldownTime = Mathf.Clamp(currCooldownTime - Time.deltaTime, 0, cooldownTime);
        }
    }


    public int calculateDamage(float impulse) {
        crashSound.Play(0);
        int damage = Mathf.RoundToInt(Mathf.Clamp(damageSlope * impulse + damageOffset, 0, maxDamage));
        if (damage > 0 && currCooldownTime == 0) {
            currCooldownTime = cooldownTime;
            return damage;
        }
        return 0;
    }
}
