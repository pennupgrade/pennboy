using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using PennBoy;
using UnityEngine;
using UnityEngine.UI;

public class HomePageManager : MonoBehaviour
{
    public GameObject repeatingBg;
    private (RectTransform rectTransform, RawImage image) bgAttrs;

    public Image transitionObj;

    private void Awake() {
        bgAttrs = (repeatingBg.GetComponent<RectTransform>(), repeatingBg.GetComponent<RawImage>());
    }

    private IEnumerator Start() {
        bgAttrs.image.color = Theme.Up[5];

        transitionObj.color = new Color(transitionObj.color.r, transitionObj.color.g, transitionObj.color.b, 1.0f);

        yield return new WaitForSeconds(2f);

        StartCoroutine(Anim.Animate(0.6f, t => {
            transitionObj.color = new Color(transitionObj.color.r, transitionObj.color.g, transitionObj.color.b, 1-t);
        }));

        
    }

}
