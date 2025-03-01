using System.Collections;
using System.Collections.Generic;
using PennBoy;
using UnityEngine;
using UnityEngine.UI;

public class MoveBackground : MonoBehaviour
{
    public float speed;

    private RawImage rectTransform;

    private void Start() {
        rectTransform = GetComponent<RawImage>();
    }

    private void Update() {
        rectTransform.uvRect = new Rect(rectTransform.uvRect.x + Time.deltaTime * 0.1f, rectTransform.uvRect.y, rectTransform.uvRect.width, rectTransform.uvRect.height);
    }


}
