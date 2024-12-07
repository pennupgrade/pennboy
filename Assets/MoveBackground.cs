using System.Collections;
using System.Collections.Generic;
using PennBoy;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    public float speed;

    private RectTransform rectTransform;

    private Vector3 initPos;

    private void Start() {
        rectTransform = GetComponent<RectTransform>();
        initPos = rectTransform.position;
        
        StartCoroutine(BackgroundAnim());
    }

    private IEnumerator BackgroundAnim() {
        while(true) {
            yield return StartCoroutine(Anim.Animate(8.0f, t => {
                rectTransform.position += rectTransform.right * 0.1f * t;
                // Debug.Log("Moving BG");
            }));

            rectTransform.position = initPos;
        }
    }


}
