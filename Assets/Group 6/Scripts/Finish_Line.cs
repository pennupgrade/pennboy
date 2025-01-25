using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish_Line : MonoBehaviour
{
    [SerializeField]
    Game level;

    [SerializeField]
    GameObject luggageCart;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col) {
        if(col.gameObject == luggageCart) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            level.win();
        }
    }
}
