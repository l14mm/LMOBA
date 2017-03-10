using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MinimapTextureScript : MonoBehaviour, IPointerClickHandler
{
    public Camera minimapCamera;
    public Camera mainCamera;
    public float divisionX = 13.5f;
    public float divisionZ = 100;

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("mouse click");
        if(minimapCamera)
        {
            Vector2 point;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out point);
            
            Vector3 newPosition = point;
            mainCamera.transform.position = new Vector3(newPosition.x / divisionX, mainCamera.transform.position.y, newPosition.y / divisionZ);

            Vector3 one = mainCamera.transform.position;
            Vector3 two = new Vector3(newPosition.x, mainCamera.transform.position.y, newPosition.y);
            Debug.DrawRay(one, two, Color.red, 10);
        }
    }

    private void Start()
    {
        StartCoroutine(FindMinimapCamera());
    }

    private IEnumerator FindMinimapCamera()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject temp = GameObject.Find("Minimap Camera");
        GameObject main = GameObject.Find("PlayerCamera(Clone)");
        if (temp && main)
        {
            minimapCamera = temp.GetComponent<Camera>();
            mainCamera = main.GetComponent<Camera>();
        }
        else
            StartCoroutine(FindMinimapCamera());
    }

    private void OnGUI()
    {
    }
}
