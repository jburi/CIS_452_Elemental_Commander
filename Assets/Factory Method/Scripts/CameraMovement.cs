/*
* Jacob Buri
* CameraMovement.cs
* Assignment 6 - Factory Method
* Pans and rotates the camera with the mouse
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float lookSpeedH = 8f;
    public float lookSpeedV = 8f;
    public float zoomSpeed = 8f;
    public float dragSpeed = 8f;

    public bool timerIsRunning = false;
    public float timeRemaining = 1f;

    //Can constrain the camera to boundaries with Mathf.Clamp
    //Vector3 minCamera = new Vector3(-50, 10, -50);
    //Vector3 maxCamera = new Vector3(50, 50, 50);

    private Vector3 origin = new Vector3(0, 0, 0);

    //private float yaw = -13f;
    //private float pitch = 28f;


    void Update()
    {
        /*
        //Look around with Right Mouse
        if (Input.GetMouseButton(1))
        {
            yaw += lookSpeedH * Input.GetAxis("Mouse X");
            pitch -= lookSpeedV * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }
        */

        

        //drag camera around with Middle Mouse
        if (Input.GetMouseButton(2))
        {
            transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed * Camera.main.orthographicSize,
            -Input.GetAxis("Mouse Y") * Time.deltaTime * dragSpeed * Camera.main.orthographicSize, 0);
        }

        //Zoom in and out with Mouse Wheel
        //transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Space.Self);
        if (Camera.main.orthographicSize > 2 && Camera.main.orthographicSize < 20)
        {
            Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        }
        else if (Camera.main.orthographicSize <= 2)
        {
            Camera.main.orthographicSize = 3;
        }
        else if (Camera.main.orthographicSize >= 20)
        {
            Camera.main.orthographicSize = 19;
        }

        //Switch angles
        if (Input.GetKeyDown("r"))
        {
            timerIsRunning = true;
            if (timeRemaining == 0)
            {
                timeRemaining = 1f;
            }
        }

        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                gameObject.transform.RotateAround(origin, Vector3.up, -90 * Time.deltaTime);
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }


        /*
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minCamera.x, maxCamera.x), 
            Mathf.Clamp(transform.position.y, minCamera.y, maxCamera.y), 
            Mathf.Clamp(transform.position.z, minCamera.z, maxCamera.z));

        //Lock Camera Height
        /*
        Vector3 setPosition = transform.position;
        setPosition.x = gameObject.transform.position.x + yaw;
        setPosition.z = gameObject.transform.position.z + pitch;
        transform.position = setPosition;
        */

    }
