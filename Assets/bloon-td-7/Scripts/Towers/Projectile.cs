using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Apple;

public class Projectile : MonoBehaviour
{
    private int dmg=0;
    private float speed=0;
    private Rigidbody rb;
    private int pierce_cap=0;
    private int pierce_counter=0;
    private bool cant_hit = false;
    [SerializeField] private GameObject hitfx;

    public void Initialize(int dmg, float speed, int pierce, Vector3 target)
    {
        this.dmg = dmg;
        this.speed = speed;
        pierce_cap = pierce;
        pierce_counter = 0;
        rb = GetComponent<Rigidbody>();
        Vector3 direction = target-transform.position;
        Vector3 rotation = transform.position - target;
        rb.velocity = new Vector3(direction.x, 0, direction.z).normalized*speed;
    }

    IEnumerator HitFX()
    {
        GameObject newhitfx = Instantiate(hitfx, rb.position, rb.rotation);
        newhitfx.GetComponent<ParticleSystem>().Emit(10);
        yield return new WaitForSeconds(1);
        Destroy(newhitfx);
    }

    IEnumerator Remove() // this is hella jank but it gives time for all the particles to be removed
    {
        cant_hit = true;
        rb.velocity = Vector3.zero;
        //GetComponent<MeshRenderer>().enabled = false;
        var emission = GetComponent<ParticleSystem>().emission;
        emission.rateOverTime = 0;
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "BTD7Enemy" || cant_hit) return;

        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        enemy.Damage(dmg);
        pierce_counter++;
        StartCoroutine(HitFX());
        if (pierce_counter >= pierce_cap) StartCoroutine(Remove());
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }


}
