using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkAnimationController : MonoBehaviour
{
    public static ShrinkAnimationController instance;
    private void Awake()
    {
        instance = this;
    }

    public void ResetSize()
    {
        gameObject.transform.localScale = Vector3.one;
    }

    public void Shrink(float deltaTime)
    {
        if(gameObject.transform.localScale.y > 0.4f)
        {
            gameObject.transform.localScale =
            Vector3.Lerp(
                gameObject.transform.localScale,
                gameObject.transform.localScale + new Vector3(0, -0.3f, 0),
                deltaTime);
        }
    }
}
