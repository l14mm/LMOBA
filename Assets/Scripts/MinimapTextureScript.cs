using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MinimapTextureScript : MonoBehaviour, IPointerClickHandler
{
    public Camera minimapCamera;
    public Camera mainCamera;
    private float division = 8;

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("mouse click");
        if(minimapCamera)
        {
            Vector2 point;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out point);
            
            Vector3 newPosition = point;
            mainCamera.transform.position = new Vector3(newPosition.x / division, mainCamera.transform.position.y, newPosition.y / division);
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
