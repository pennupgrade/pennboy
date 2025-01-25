using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixedSingleScreenMenuAutoScalingCanvasBecauseUnitySucks : MonoBehaviour
{
    private int defaultWidth;
    private int defaultHeight;

    [SerializeField] private Canvas backgroundCanvas;
    [SerializeField] private Canvas uiCanvas;

    [SerializeField] private RawImage backgroundImage;

    // Current width and height
    private int currWidth;
    private int currHeight;

    // Start is called before the first frame update
    void Start()
    {
        defaultWidth = backgroundImage.mainTexture.width;
        defaultHeight = backgroundImage.mainTexture.height;
        currWidth = defaultWidth;
        currHeight = defaultHeight;
        backgroundImage.rectTransform.sizeDelta = new Vector2(defaultWidth, defaultHeight);
    }

    // Update is called once per frame
    void Update()
    {
        int width = Screen.width;
        int height = Screen.height;
        if (width != currWidth || height != currHeight) {
            currHeight = height;
            currWidth = width;
            float widthScale = ((float) currWidth) / defaultWidth;
            float heightScale = ((float) currHeight) / defaultHeight;
            backgroundCanvas.scaleFactor = Mathf.Max(widthScale, heightScale);
            uiCanvas.scaleFactor = Mathf.Min(widthScale, heightScale);
        }
    }
}
