using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxfireScript : MonoBehaviour
{
    private MovementScript player;
    [SerializeField] float minBezierDuration;
    [SerializeField] float maxBezierDuration;
    private float duration;
    [SerializeField] float minCurveHeight;
    [SerializeField] float maxCurveHeight;
    private float curveHeight;

    private Vector3 controlPoint;
    private Vector3 endPoint;
    private Vector3 startPoint;
    private float time;
    private Vector3 offset;


    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0);
        player = FindObjectOfType<MovementScript>();
        startPoint = this.transform.parent.transform.position;
        endPoint = player.transform.position;
        duration = Random.Range(minBezierDuration, maxBezierDuration);
        curveHeight = Random.Range(minCurveHeight, maxCurveHeight);
        if (Random.Range(0, 2) < 1)
        {
            curveHeight = -curveHeight;
        }
        controlPoint = (startPoint + endPoint) / 2 + Vector3.up * curveHeight;
    }

    // Update is called once per frame
    void Update()
    {
        startPoint = this.transform.parent.transform.position;
        endPoint = player.transform.position + offset;
        controlPoint = (startPoint + endPoint) / 2 + Vector3.up * curveHeight;
        time += Time.deltaTime;
        float t = Mathf.Clamp01(time / duration);

        Vector3 currPos = calculateBezierPoint(t, startPoint, controlPoint, endPoint);
        transform.position = currPos;

        if (t >= 1f)
        {
            Destroy(gameObject);
        }
    }

    private Vector3 calculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        return u * u * p0 + 2 * u * t * p1 + t * t * p2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        } else
        {
            Destroy(this.gameObject);
        }
    }
}
