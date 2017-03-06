using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLocalPlayer : MonoBehaviour {

    private GameObject localPlayer;

    private void Start()
    {
        StartCoroutine(FindPlayer());
    }

    private IEnumerator FindPlayer()
    {
        yield return new WaitForSeconds(0.1f);


        localPlayer = GameObject.Find("LOCAL Player");

        if (!localPlayer)
            StartCoroutine(FindPlayer());
    }

    private void Update()
    {
        if(localPlayer)
            transform.position = new Vector3(localPlayer.transform.position.x, transform.position.y, localPlayer.transform.position.z);
    }
}
