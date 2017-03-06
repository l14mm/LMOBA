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

    public GameObject p_AutoAttack;

    private GameObject fire;
    ParticleSystem.Particle[] m_Particles;
    private float fireScale = 1;
    private GameObject fireball;

    private float lastCastTime = 0;
    private float castDelay = 1.0f;

    private float lastAutoTime = 0;
    private float autoDelay = 1;
    [HideInInspector]
    public Vector3 autoTarget;
    [HideInInspector]
    public float autoRange = 5;
    [HideInInspector]
    public Transform target = null;
    [HideInInspector]
    public bool isCasting = false;

    public GameObject p_Lightning;
    private float lightningStartTime;
    private float lightningCastTime = 1;
    private bool lightningCasting = false;
    GameObject lightning;
    Transform lightningTarget;

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if(lightningCasting)
        {
            if(Time.time > lightningStartTime + lightningCastTime)
            {
                Destroy(lightning);
                lightningCasting = false;
            }
            else
            {
                lightning.transform.FindChild("LightningStart").transform.position = transform.position;
                lightning.transform.FindChild("LightningEnd").transform.position = lightningTarget.position;
                lightningTarget.GetComponent<NetworkedPlayerScript>().RpcResolveHit(0.25f);
                GetComponent<NetworkedPlayerScript>().mana -= 0.2f;
            }
        }
        if (Input.GetKeyDown("2"))
        {
            if (!lightningCasting)
            {
                RaycastHit hit;
                if (Physics.Raycast(GetComponent<NetworkedPlayerScript>().myCamera.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    if (hit.transform.GetComponent<NetworkedPlayerScript>() && hit.transform.GetComponent<NetworkedPlayerScript>().netId != netId)
                    {
                        CreateLightning(hit.transform);
                    }
                }
            }
        }
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
        if(Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(GetComponent<NetworkedPlayerScript>().myCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.transform.GetComponent<NetworkedPlayerScript>() && hit.transform.GetComponent<NetworkedPlayerScript>().netId != netId)
                {
                    target = hit.transform;
                }
            }
        }
        if(target)
        {
            if (Vector3.Distance(transform.position, target.position) < autoRange)
            {
                isCasting = true;
                AutoAttack(target);
                autoTarget = target.position;
                StartCoroutine(StopCasting(0.5f));
                //
                target = null;
            }
            else
            {
                // If we are not in range to auto attack, move closer to target
                //GetComponent<PlayerMovement>().waypoint = target.position;
            }
        }
    }

    private IEnumerator StopCasting(float time)
    {
        yield return new WaitForSeconds(time);
        isCasting = false;
    }

    [Command]
    void CmdHitPlayer(GameObject hit, int damage)
    {
        hit.GetComponent<NetworkedPlayerScript>().RpcResolveHit(damage);
    }

    public void CreateLightning(Transform targetT)
    {
        lightningStartTime = Time.time;
        lightning = Instantiate(p_Lightning);
        lightningTarget = targetT;
        lightningCasting = true;
    }

    public void AutoAttack(Transform targetT)
    {
        if (Time.time > lastAutoTime + autoDelay)
        {
            GameObject autoAttack = Instantiate(p_AutoAttack, t_shoot.position, t_shoot.rotation, null);
            autoAttack.GetComponent<AutoAttackScript>().target = targetT;
            autoAttack.GetComponent<AutoAttackScript>().creator = GetComponent<NetworkedPlayerScript>();
            lastAutoTime = Time.time;
        }
    }

    public void CreateFire()
    {
        if (Time.time > lastCastTime + castDelay)
        {
            fireScale = 0.5f;
            fire = Instantiate(p_fire, t_shoot.position, t_shoot.rotation, transform);
            GetComponent<UnityEngine.AI.NavMeshAgent>().speed *= 0.5f;
            lastCastTime = Time.time;
        }
    }

    public void IncreaseFire()
    {
        if (!fire)
            return;

        if (fire != null && fireScale < 2 && GetComponent<NetworkedPlayerScript>().mana > 0)
        {
            GetComponent<NetworkedPlayerScript>().mana -= 0.5f;
            fireScale += Time.deltaTime * 0.5f;
            fire.transform.localScale = new Vector3(fireScale, fireScale, fireScale);
        }
    }

    public void CreateFireBall()
    {
        if (!fire)
            return;

        Destroy(fire);

        RaycastHit hit;
        if (Physics.Raycast(GetComponent<NetworkedPlayerScript>().myCamera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            t_shoot.LookAt(new Vector3(hit.point.x, t_shoot.transform.position.y, hit.point.z));
        }

        fireball = Instantiate(p_fireball, t_shoot.position, t_shoot.rotation, null);
        fireball.transform.localScale = new Vector3(fireScale, fireScale, fireScale);
        fireball.GetComponent<FireballScript>().Remove();
        fireball.GetComponent<FireballScript>().creator = GetComponent<NetworkedPlayerScript>();
        GetComponent<UnityEngine.AI.NavMeshAgent>().speed *= 2.0f;
    }

    public void CreateFireBallAI(Vector3 pos)
    {
        if (!fire)
            return;

        Destroy(fire);

        t_shoot.LookAt(new Vector3(pos.x, t_shoot.transform.position.y, pos.z));

        fireball = Instantiate(p_fireball, t_shoot.position, t_shoot.rotation, null);
        fireball.transform.localScale = new Vector3(fireScale, fireScale, fireScale);
        fireball.GetComponent<FireballScript>().Remove();
        fireball.GetComponent<FireballScript>().creator = GetComponent<NetworkedPlayerScript>();
        GetComponent<UnityEngine.AI.NavMeshAgent>().speed *= 2.0f;
    }
}