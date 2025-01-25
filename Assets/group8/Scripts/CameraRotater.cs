using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotater : MonoBehaviour
{

    public float speed;
    public float radius;
    // Start is called before the first frame update


    GameObject mainCamera;
    //float minMouseRange = 0.1f; // below this value, player won't move
    //float maxMouseRange = 0.3f; // above this value palyer will move at max speed

    //private float screenCenterX;
    //bool isDragging;
    //bool canMove;

    void Start()
    {
        mainCamera = transform.GetChild(0).gameObject; // for optimizaton purpose do not put this inside update
        //canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        // if clicked
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            //isDragging = true;
            //canMove = false;
            //StartCoroutine(WaitThenMove());
        }

        //if (Input.GetMouseButtonUp(0)) // 0 is the left mouse button
        //{
        //    isDragging = false;
        //}


        //if (isDragging)
        //{
        //    /* Moving by mouse position */
        //    float mouseX = Input.mousePosition.x;
        //    float screenCenterX = Screen.width / 2f;
        //    float distanceFromCenter = Mathf.Abs(mouseX - screenCenterX) / screenCenterX;
        //    float movementSpeed = 0;

        //    if (distanceFromCenter < minMouseRange) movementSpeed = 0;
        //    else if (distanceFromCenter > maxMouseRange) movementSpeed = speed;
        //    else movementSpeed = speed * (distanceFromCenter - minMouseRange) / (maxMouseRange - minMouseRange);


        //    // Check if the mouse has moved left or right from the center
        //    if (mouseX < screenCenterX)
        //    {
        //        // Move the character to the left
        //        transform.Rotate(0, -movementSpeed * Time.deltaTime, 0);
        //    }
        //    else if (mouseX > screenCenterX)
        //    {
        //        // Move the character to the right
        //        transform.Rotate(0, movementSpeed * Time.deltaTime, 0);
        //    }
        //}
        //else
        //{

        //if (canMove)
        //{

            /*  Moving by keyboard input */
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(0, -speed * Time.deltaTime, 0); //(x, y ,z )
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(0, speed * Time.deltaTime, 0); //(x, y ,z )
            }
            //if (Input.GetKey(KeyCode.S))
            //{
            //    mainCamera.transform.localPosition += new Vector3(0, radius * Time.deltaTime, -radius * Time.deltaTime); //(x, y ,z )
            //}

            //if (Input.GetKey(KeyCode.W))
            //{
            //    if (mainCamera.transform.localPosition.y >= 24 && mainCamera.transform.localPosition.z <= -12)
            //    {
            //        mainCamera.transform.localPosition += new Vector3(0, -radius * Time.deltaTime, radius * Time.deltaTime); //(x, y ,z )
            //    }
            //}
        //}
        //}

        //IEnumerator WaitThenMove()
        //{
        //    float delay = 1.5f;
        //    yield return new WaitForSeconds(delay);
        //    canMove = true;

        //}


    }
}
