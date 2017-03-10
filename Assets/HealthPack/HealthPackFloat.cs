using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackFloat : MonoBehaviour {

	private void FixedUpdate()
    {
        transform.position= new Vector3(transform.position.x, 1 + Mathf.Sin(Time.time) / 2, transform.position.z);
        transform.Rotate(new Vector3(0, 0.5f, 0));
    }
}
