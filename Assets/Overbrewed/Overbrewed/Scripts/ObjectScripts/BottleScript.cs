using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleScript : MonoBehaviour
{
    public GameObject player;
    public ButtonAttribute button;
    private bool isInRange = false;
    public MugScript mug;
    public int milkAmount = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        button.buttonName = "Oat Milk";
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRange && mug.mugHeld != null)
        {
        }
    }
}

public class ButtonHandler : MonoBehaviour
{
    public void OnButtonClicked()
    {
        Debug.Log("Button was clicked!");
    }
}
