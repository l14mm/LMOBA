﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private float minFov = 15f;
    private float maxFov = 90f;
    private float scrollSensitivity = 10f;
    private float moveSensitivity = 10f;

    private Camera me;

    public bool cameraLocked = true;
    [HideInInspector]
    public Transform myPlayer;
    private Vector3 initialPosition;

    private float screenWidth;
    private float screenHeight;
    public float boundary = 50;
    public float speed = 5;

    private void Awake()
    {
        me = GetComponent<Camera>();
        initialPosition = transform.position;

        screenWidth = Screen.width;
        screenHeight = Screen.height;

        Cursor.lockState = CursorLockMode.Confined;
    }

    public void SetPlayer(Transform _myPlayer)
    {
        myPlayer = _myPlayer;
    }

    private void Update ()
    {
        float fov = me.fieldOfView;
        fov += Input.GetAxis("Mouse ScrollWheel") * -scrollSensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        me.fieldOfView = fov;

        /*
        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            cameraLocked = false;
            Vector3 translation = new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * moveSensitivity, 0, Input.GetAxis("Vertical") * Time.deltaTime * moveSensitivity);
            transform.position = transform.position + translation;
        }
        */

        float x = 0;
        float z = 0;
        if (Input.mousePosition.x > screenWidth - boundary)
        {
            float multiplier = Input.mousePosition.x - screenWidth + boundary;
            multiplier /= boundary;
            if (multiplier > 1) multiplier = 1;
            x += speed * Time.deltaTime * multiplier; // move on +X axis
        }
        if (Input.mousePosition.x < 0 + boundary)
        {
            float multiplier = boundary - Input.mousePosition.x;
            multiplier /= boundary;
            if (multiplier > 1) multiplier = 1;
            x -= speed * Time.deltaTime * multiplier; // move on -X axis
        }
        if (Input.mousePosition.y > screenHeight - boundary)
        {
            float multiplier = Input.mousePosition.y - screenHeight + boundary;
            multiplier /= boundary;
            if (multiplier > 1) multiplier = 1;
            z += speed * Time.deltaTime * multiplier; // move on +Z axis
        }
        if (Input.mousePosition.y < 0 + boundary)
        {
            float multiplier = boundary - Input.mousePosition.y;
            multiplier /= boundary;
            if (multiplier > 1) multiplier = 1;
            z -= speed * Time.deltaTime * multiplier; // move on -Z axis
        }
        transform.position = transform.position + new Vector3(x, 0, z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            cameraLocked = !cameraLocked;
        }

        if(cameraLocked && myPlayer)
        {
            transform.position = initialPosition + myPlayer.position;
        }
        else
        {

        }
    }
}
