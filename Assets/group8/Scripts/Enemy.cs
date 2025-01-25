using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BombGame
{
    public class Enemy : MonoBehaviour
    {
        [Header("Climb Variables")]
        public float phaseLength = 10f;
        public float climbHeight = 3f;
        public float initialClimbHeight = 3f;
        public float variation = 0.5f;
        public float initalVariation = 2f;
        private bool isFirstClimb = true;

        [Header("Flight Variables")]
        public float flyLength = 3f;
        public float flyDuration = 2f;

        [Header("Miscellaneous")]
        private ScoreManager scoreManager;
        public Material[] materials;
        public int colorChoice = 0;

        [Header("Damage Variables")]
        private EnemyUI enemyUI;
        public float LV = 1;
        public float maxHP;
        public float HP = 1;
        private bool burning = false;
        private IEnumerator burn;
        public bool dead = false;
        public float burnDPS = 10f;
        public float onDeathFirebombRadius = 1f;
        public GameObject BombVFX;

        public string effectStatus = "normal";
        int freezeCounter = 0;
        public int freezeStatusDamage = 1;

        private Rigidbody rb;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            scoreManager = FindObjectOfType<ScoreManager>();
            enemyUI = GetComponentInChildren<EnemyUI>();
            maxHP = HP;
            if (enemyUI != null)
            {
                enemyUI.UpdateHealthBar(HP, maxHP);
                enemyUI.UpdateLv(LV);
            }
            //if (colorChoice == -1)
            //    ChooseColor(Random.Range(0,2));
            //else
            //ChooseColor(colorChoice);
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void ClimbUp(float duration)
        {
            // if frozen should not climb
            if (freezeCounter > 0)
            {
                freezeCounter--;
                TakeDamage(freezeStatusDamage);
                return;
            }
            // should climb up every wave
            float upMagnitude = (climbHeight + Random.Range(-1f, 1f) * variation);
            if (isFirstClimb)
            {
                upMagnitude = (initialClimbHeight + Random.Range(-1f, 1f) * initalVariation);
                isFirstClimb = false;
            }
            Vector3 upward = Vector3.up * upMagnitude;
            StartCoroutine(MoveOverTime(transform.position, transform.position + upward, duration));
        }

        public void TakeDamage(float dmg)
        {
            if (!dead)
            {
                HP -= dmg;
                Debug.Log("damage taken " + HP);
                if (enemyUI != null) enemyUI.UpdateHealthBar(HP, maxHP);
                if (HP <= 0f)
                {
                    dead = true;
                    if (burning)
                    {
                        //DebugExplosionSphere(onDeathFirebombRadius);
                        OnDeathFireBomb(onDeathFirebombRadius);
                    }
                    FlyAway();
                }
            }
        }

        IEnumerator FireBurn(float duration)
        {
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                TakeDamage(burnDPS * Time.deltaTime);
                yield return null;
            }
            burning = false;
        }

        public void DebugExplosionSphere(float radius)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = transform.position;
            sphere.transform.localScale *= radius / 0.5f;
        }

        public void OnDeathFireBomb(float radius)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
            GameObject explosion = Instantiate(BombVFX);
            explosion.transform.localScale *= radius / 2.5f;
            explosion.transform.position = transform.position;
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.tag == "Enemy" && gameObject != hitCollider.gameObject)
                {
                    if (!hitCollider.GetComponent<Enemy>().dead)
                    {
                        hitCollider.GetComponent<Enemy>().onDeathFirebombRadius = onDeathFirebombRadius * 0.9f;
                        hitCollider.GetComponent<Enemy>().TouchedByBomb(0);
                    }
                }
            }
        }


        public void TouchedByBomb(int type, float damage = 3)
        {
            // executes bomb effect
            switch (type)
            {
                case 0: // Firebomb
                    TakeDamage(1);
                    break;
                case 1: // Firebomb
                    burning = true;
                    burn = FireBurn(10f);
                    TakeDamage(3);
                    if (burning)
                    {
                        StopCoroutine(burn);
                    }
                    StartCoroutine(burn);
                    break;
                case 4: // Electricity type
                    if (ElectricityStorage.Instance != null)
                        ElectricityStorage.Instance.EffectElectricity(gameObject);
                    break;
                case 5: // ice type
                    if (IceStorage.Instance != null)
                        IceStorage.Instance.EffectIce(gameObject);
                    break;
                default:
                    TakeDamage(damage);
                    break;
            }
        }


        public void iceFreeze(int count = 3)
        {
            effectStatus = "freeze";
            freezeCounter = count;
        }

        public void FlyAway()
        {
            // activated when affected by bomb; should pretty much fly away in death

            //StartCoroutine(MoveOverTime(transform.position, transform.position + Random.onUnitSphere * flyLength, flyDuration));
            //Destroy(gameObject);
            dead = true;
            rb.isKinematic = false;
            rb.velocity = Random.onUnitSphere * flyLength;
            StartCoroutine(DelayedDeath(flyDuration));

            // add score
            int score = 100;
            if (scoreManager != null) scoreManager.addScore(score);
        }


        public void RushToCenter()
        {
            // called when enemy reached top
            // make the enemy run to the center, and get to the trap door in the middle
            Vector3 start = transform.position;
            Vector3 end = new Vector3(0, transform.position.y, 0);
            float duration = 1.36f;
            float clipDuration = 0.36f;
            StartCoroutine(MoveOverTime(start, end, duration, clipDuration));
        }

        // Coroutine to move the object from startPosition to endPosition over the given duration
        IEnumerator MoveOverTime(Vector3 start, Vector3 end, float duration, float clipDuration = 0)
        {

            float elapsedTime = 0f;

            while (elapsedTime < (duration - clipDuration))
            {
                // Lerp position from start to end based on elapsed time
                transform.position = Vector3.Lerp(start, end, elapsedTime / duration);

                // Increment elapsed time
                elapsedTime += Time.deltaTime;

                // Wait for the next frame
                yield return null;
            }

            // Ensure the object reaches the exact end position after the movement
            if (clipDuration == 0) transform.position = end;

        }

        // Function to change the object's material
        public void ChooseColor(int choice = 0)
        {
            Renderer renderer = GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.material = materials[choice];
            }
            else
            {
                Debug.LogWarning("Renderer component not found on this GameObject.");
            }
        }

        IEnumerator DelayedDeath(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);

        }
    }
    // it seems its possible to access destroyed enemy due to race condition, hence this delay
    //yield return new WaitForSeconds(delay + 1);
    //Destroy(gameObject);

}
