using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Camera mainCamera;

    private void Start()
    {
        StartCoroutine(FindMainCamera());
    }

    private IEnumerator FindMainCamera()
    {
        yield return new WaitForSeconds(0.1f);


        GameObject temp = GameObject.Find("PlayerCamera(Clone)");
        if (temp)
            mainCamera = temp.GetComponent<Camera>();

        if (!mainCamera)
            StartCoroutine(FindMainCamera());
    }

    private void Update()
    {

    }

    private void OnGUI()
    {

    }

    private void OnGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(Vector3.zero, new Vector3(50, 50, 50));
    }
}
