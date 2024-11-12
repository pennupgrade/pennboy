using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using PennBoy;
using UnityEngine;
using UnityEngine.UI;

public class HomePageManager : MonoBehaviour
{
    public GameObject repeatingbg;
    public GameObject bgMask;
    private (RectTransform rectTransform, Image image) bgAttrs;

    public AnimationCurve bgTransitionCurve;    

    public Transform targetTransitionTransform;

    bool idk = false;

    private void Awake() {
        bgAttrs = (repeatingbg.GetComponent<RectTransform>(), repeatingbg.GetComponent<Image>());
    }

    private IEnumerator Start() {
        bgAttrs.image.color = Theme.Up[10];

        yield return new WaitForSeconds(1f);

        float originalScale = bgMask.transform.localScale.x;
        Vector3 originalPos = bgMask.transform.position;

        StartCoroutine(Anim.Animate(0.5f, t => {
            float targetScale = targetTransitionTransform.localScale.x;
            Vector3 targetPosition = targetTransitionTransform.position;
            float curveT = bgTransitionCurve.Evaluate(t);

            bgMask.transform.localScale = new Vector3(
                targetScale * curveT + (1-curveT) * originalScale,
                targetScale * curveT + (1-curveT) * originalScale,
                targetScale * curveT + (1-curveT) * originalScale
            );
            bgMask.transform.position = curveT * targetTransitionTransform.position  + (1-curveT) * originalPos;
        }));

        idk = true;
    }

    private void Update() {
        if (idk) {
            bgMask.transform.position = targetTransitionTransform.position;
            bgMask.transform.localScale = targetTransitionTransform.localScale;
        }
    }
}
