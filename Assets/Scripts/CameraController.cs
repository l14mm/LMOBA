using System.Collections;
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


    private void Awake()
    {
        me = GetComponent<Camera>();
        initialPosition = transform.position;
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
        
        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            cameraLocked = false;
            Vector3 translation = new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * moveSensitivity, 0, Input.GetAxis("Vertical") * Time.deltaTime * moveSensitivity);
            transform.position = transform.position + translation;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space");
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
