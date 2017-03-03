using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour {

    private ParticleSystem ps;
    private float speed = 0;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        ps.Play();
    }

    public void Fire()
    {
        //speed = 1;
        //ps.main.startSpeed = 30;
    }

    private void FixedUpdate()
    {
        //transform.Translate(Vector3.forward);
    }

    public void Remove()
    {
        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
    }
}
