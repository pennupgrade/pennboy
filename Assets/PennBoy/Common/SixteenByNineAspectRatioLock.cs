using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SixteenByNineAspectRatioLock : MonoBehaviour
{
    public Camera mainCam;
    void Awake() {
        RescaleMainCamera();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCam.rect.width != Screen.width && mainCam.rect.height != Screen.height) {
            RescaleMainCamera();
        }
    }

    void RescaleMainCamera() {
        float scaleFactorX = ((float) Screen.width) / 16;
        float scaleFactorY = ((float) Screen.height) / 9;
        float xSize;
        float ySize;
        if (scaleFactorX < scaleFactorY) {
            xSize = Screen.width;
            ySize = 9 * scaleFactorX;
        } else {
            ySize = Screen.height;
            xSize = 16 * scaleFactorY;
        }


        float xAmount = xSize / Screen.width;
        float yAmount = ySize / Screen.height;

        mainCam.rect = new((1 - xAmount)/2, (1-yAmount)/2, xAmount, yAmount);
    }
}
