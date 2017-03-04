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
            CreateFire();
        }
        if (Input.GetKey("1"))
        {
            IncreaseFire();
        }
        if (Input.GetKeyUp("1"))
        {
            CreateFireBall();
        }
    }

    [Command]
    void CmdHitPlayer(GameObject hit, int damage)
    {
        Debug.Log("hit player");
        hit.GetComponent<NetworkedPlayerScript>().RpcResolveHit(damage);
    }

    public void CreateFire()
    {
        fireScale = 0.5f;
        fire = Instantiate(p_fire, t_shoot.position, t_shoot.rotation, transform);
        GetComponent<UnityEngine.AI.NavMeshAgent>().speed *= 0.5f;
    }

    public void IncreaseFire()
    {
        if (fire != null && fireScale < 2)
        {
            fireScale += Time.deltaTime * 0.5f;
            fire.transform.localScale = new Vector3(fireScale, fireScale, fireScale);
        }
    }

    public void CreateFireBall(Transform enemy = null)
    {
        Destroy(fire);

        if (enemy == null)
        {
            RaycastHit hit;
            if (Physics.Raycast(GetComponent<NetworkedPlayerScript>().myCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                t_shoot.LookAt(new Vector3(hit.point.x, t_shoot.transform.position.y, hit.point.z));
            }
        }
        else
        {
            t_shoot.LookAt(new Vector3(enemy.position.x, t_shoot.transform.position.y, enemy.position.z));
        }

        fireball = Instantiate(p_fireball, t_shoot.position, t_shoot.rotation, null);
        fireball.transform.localScale = new Vector3(fireScale, fireScale, fireScale);
        fireball.GetComponent<FireballScript>().Remove();
        fireball.GetComponent<FireballScript>().creator = GetComponent<NetworkedPlayerScript>();
        GetComponent<UnityEngine.AI.NavMeshAgent>().speed *= 2.0f;
    }
}