using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    float minFov = 15f;
    float maxFov = 90f;
    float sensitivity = 10f;

    private Camera me;

    private void Awake()
    {
        me = GetComponent<Camera>();
    }

    void Update ()
    {
        float fov = me.fieldOfView;
        fov += Input.GetAxis("Mouse ScrollWheel") * -sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        me.fieldOfView = fov;

        Vector3 trans = new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime, Input.GetAxis("Vertical") * Time.deltaTime, 0);
        transform.Translate(trans);
    }
}
