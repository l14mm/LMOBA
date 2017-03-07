using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{

    public GameObject mainCamera;
    public float horizontalExtent;

    private void Start()
    {
        StartCoroutine(FindMainCamera());
    }

    private IEnumerator FindMainCamera()
    {
        yield return new WaitForSeconds(0.1f);


        mainCamera = GameObject.Find("PlayerCamera(Clone)");

        if (!mainCamera)
            StartCoroutine(FindMainCamera());
    }

    private void Update()
    {
        //if (mainCamera)
        //transform.position = new Vector3(mainCamera.transform.position.x, transform.position.y, mainCamera.transform.position.z);
        if(mainCamera)
        {
            horizontalExtent = mainCamera.GetComponent<Camera>().orthographicSize * Screen.width / Screen.height;
        }
    }

    private void OnGUI()
    {

        GUI.Label(new Rect(25, 25, 100, 30), "hi");
    }

    private void OnGizmos()
    {

    }
}
