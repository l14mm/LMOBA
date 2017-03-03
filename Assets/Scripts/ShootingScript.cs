using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class ShootingScript : NetworkBehaviour
{
    public ParticleSystem _muzzleFlash;
    public AudioSource _gunAudio;
    public GameObject _impactPrefab;
    public Transform cameraTransform;

    public GameObject p_fireball;
    public GameObject p_fire;
    public Transform t_shoot;

    float timeBetweenBullets = 0.15f;

    private GameObject fire;
    ParticleSystem.Particle[] m_Particles;
    private float fireScale = 1;
    private GameObject fireball;

    void Start()
    {

    }
    
    void Update()
    {
        if (!isLocalPlayer)
            return;

        
        if (Input.GetKeyDown("1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                t_shoot.LookAt(new Vector3(hit.point.x, t_shoot.transform.position.y, hit.point.z));
            }
            fireScale = 1;
            fire = Instantiate(p_fire, t_shoot.position, t_shoot.rotation, transform);
            GetComponent<UnityEngine.AI.NavMeshAgent>().speed *= 0.5f;
        }
        if (Input.GetKey("1"))
        {
            if(fire != null)
            {
                fireScale += Time.deltaTime;
                fire.transform.localScale = new Vector3(fireScale, fireScale, fireScale);
                
            }
        }
        if (Input.GetKeyUp("1"))
        {
            Destroy(fire);

            fireball = Instantiate(p_fireball, t_shoot.position, t_shoot.rotation, null);
            fireball.transform.localScale = new Vector3(fireScale, fireScale, fireScale);
            fireball.GetComponent<FireballScript>().Remove();
            GetComponent<UnityEngine.AI.NavMeshAgent>().speed *= 2.0f;
        }
    }

    [Command]
    void CmdHitPlayer(GameObject hit, int damage)
    {
        Debug.Log("hit player");
        hit.GetComponent<NetworkedPlayerScript>().RpcResolveHit(damage);
    }
}