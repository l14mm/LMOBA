using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ShootingScript : NetworkBehaviour
{
    public ParticleSystem _muzzleFlash;
    public AudioSource _gunAudio;
    public GameObject _impactPrefab;
    public Transform cameraTransform;

    [HideInInspector]
    public bool isCastingSpell = false;

    public GameObject p_fireball;
    public GameObject p_fire;
    public Transform t_shoot;
    private Text fireballTimer;
    [HideInInspector]
    public float fireballTimeValue = 0;
    private RawImage fireballIcon;

    public GameObject p_AutoAttack;

    private GameObject fire;
    ParticleSystem.Particle[] m_Particles;
    public float fireScale { get; private set ;}
    private GameObject fireball;

    private float lastAutoTime = 0;
    private float autoDelay = 1;
    [HideInInspector]
    public Vector3 autoTarget;
    [HideInInspector]
    public float autoRange { get; private set; }
    //[HideInInspector]
    public Transform target = null;
    [HideInInspector]
    public bool isCasting = false;

    public GameObject p_Lightning;
    private float lightningStartTime;
    private float lightningCastTime = 1;
    private bool lightningCasting = false;
    GameObject lightning;
    Transform lightningTarget;

    public GameObject p_BlinkLight;
    private Text blinkTimer;
    private RawImage blinkIcon;
    private float blinkTimeValue = 0;

    public MeshRenderer attackRangeMesh;

    public MeshRenderer orbPullMesh;
    private float orbPullRange = 15;
    public float orbPullStrength = 1;

    private void Awake()
    {
        blinkTimer = GameObject.Find("BlinkTimer").GetComponent<Text>();
        blinkIcon = GameObject.Find("BlinkIcon").GetComponent<RawImage>();
        fireballTimer = GameObject.Find("FireballTimer").GetComponent<Text>();
        fireballIcon = GameObject.Find("FireballIcon").GetComponent<RawImage>();

        autoRange = 10;

        if (!isLocalPlayer)
            return;

        attackRangeMesh.enabled = false;
        orbPullMesh.enabled = false;
    }

    public override void OnStartLocalPlayer()
    {

        attackRangeMesh.enabled = false;
        orbPullMesh.enabled = false;
    }

    void Update()
    {

        if (isLocalPlayer)  blinkTimer.text = (Mathf.Ceil(blinkTimeValue)).ToString();
        if (blinkTimeValue > 0)
        {
            blinkTimeValue -= Time.deltaTime;
            if (isLocalPlayer)
            {
                blinkTimer.enabled = true;
                blinkIcon.color = new Color(0.1f, 0.1f, 0.1f, 1);
            }
        }
        else
        {
            if (isLocalPlayer)
            {
                blinkIcon.color = new Color(1, 1, 1, 1);
                blinkTimer.enabled = false;
            }
        }

        if (isLocalPlayer) fireballTimer.text = (Mathf.Ceil(fireballTimeValue)).ToString();
        if (fireballTimeValue > 0)
        {
            fireballTimeValue -= Time.deltaTime;
            if (isLocalPlayer)
            {
                fireballTimer.enabled = true;
                fireballIcon.color = new Color(0.1f, 0.1f, 0.1f, 1);
            }
        }
        else
        {
            if (isLocalPlayer)
            {
                fireballIcon.color = new Color(1, 1, 1, 1);
                fireballTimer.enabled = false;
            }
        }

        if (lightningCasting)
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


        if (!isLocalPlayer)
            return;

        attackRangeMesh.transform.localScale = new Vector3(autoRange * 0.226f, 1, autoRange * 0.226f);
        
        if (Input.GetMouseButtonDown(0))
        {
            // Attack move
            if (attackRangeMesh.enabled)
            {
                // Find closest enemy that we can see
                RaycastHit hit;
                if (Physics.Raycast(GetComponent<NetworkedPlayerScript>().myCamera.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    Transform closestEnemy = null;
                    float closestDistance = 21;
                    Collider[] hitColliders = Physics.OverlapSphere(hit.point, 20);
                    for (int i = 0; i < hitColliders.Length; i++)
                    {
                        GameObject temp = hitColliders[i].gameObject;
                        if (temp.transform.parent && temp.transform.parent.GetComponent<NetworkedPlayerScript>() && temp.transform.parent.GetComponent<NetworkedPlayerScript>().netId.Value != GetComponent<NetworkedPlayerScript>().netId.Value)
                        {
                            float distance = Vector3.Distance(transform.position, temp.transform.parent.position);
                            if (distance < closestDistance)
                            {
                                closestDistance = distance;
                                closestEnemy = temp.transform.parent;
                            }
                        }
                    }
                    if (closestEnemy)
                    {
                        target = closestEnemy.transform;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            attackRangeMesh.enabled = true;
        }
        else if (Input.anyKeyDown)
        {
            attackRangeMesh.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.W))
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateFire();
        }
        if (Input.GetKey(KeyCode.Q))
        {
            IncreaseFire();
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            CreateFireBall();
        }
        // Flash
        if (Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit hit;
            if (Physics.Raycast(GetComponent<NetworkedPlayerScript>().myCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Vector3 direction = (hit.point - transform.position);
                Blink(direction);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            // Set target
            RaycastHit hit;
            if (Physics.Raycast(GetComponent<NetworkedPlayerScript>().myCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.transform.GetComponent<NetworkedPlayerScript>() && hit.transform.GetComponent<NetworkedPlayerScript>().netId != netId)
                {
                    target = hit.transform;
                }
                else if (hit.transform.GetComponent<BaseScript>())
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
        if (Input.GetKey(KeyCode.E))
        {
            //GetComponent<Rigidbody>().AddForce(transform.forward * 100 * Time.deltaTime, ForceMode.Impulse);

            orbPullMesh.transform.localScale = new Vector3(orbPullRange * 0.226f, 1, orbPullRange * 0.226f);
            orbPullMesh.enabled = true;
            List<Transform> players = new List<Transform>();
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, orbPullRange);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                GameObject temp = hitColliders[i].gameObject;
                if (temp.transform.parent && temp.transform.parent.GetComponent<NetworkedPlayerScript>() && temp.transform.parent.GetComponent<NetworkedPlayerScript>().netId.Value != GetComponent<NetworkedPlayerScript>().netId.Value)
                {
                    players.Add(temp.transform.parent);
                }
            }
            for(int i = 0; i < players.Count; i++)
            {
                Vector3 dir = Vector3.MoveTowards(players[i].transform.position, transform.position, 25 * orbPullStrength * Time.deltaTime) - players[i].transform.position;
                //Debug.DrawRay(players[i].transform.position, dir, Color.red, 0.1f);
                players[i].GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);
            }
        }
        else
        {
            orbPullMesh.enabled = false;
        }
    }

    public void Blink(Vector3 direction)
    {
        if (blinkTimeValue <= 0)
        {
            Instantiate(p_BlinkLight, transform.position, transform.rotation);
            transform.position = transform.position + direction.normalized * 5;
            Instantiate(p_BlinkLight, transform.position, transform.rotation);
            blinkTimeValue = 5;
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
            autoAttack.GetComponent<AutoAttackScript>().damage = GetComponent<NetworkedPlayerScript>().attackDamge;
            autoAttack.GetComponent<AutoAttackScript>().creator = GetComponent<NetworkedPlayerScript>();
            lastAutoTime = Time.time;
        }
    }

    public void CreateFire()
    {
        //if (Time.time > lastCastTime + castDelay)
        if (fireballTimeValue <= 0 && !isCastingSpell)
        {

            if (GetComponent<NetworkedPlayerScript>().anim)
            {
                GetComponent<NetworkedPlayerScript>().anim.Play("CreateFire");
            }

            isCastingSpell = true;
            fireScale = 0.5f;
            fire = Instantiate(p_fire, t_shoot.position, t_shoot.rotation, transform);
            GetComponent<UnityEngine.AI.NavMeshAgent>().speed *= 0.5f;
            //lastCastTime = Time.time;
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

        if(GetComponent<NetworkedPlayerScript>().anim)
        {
            GetComponent<NetworkedPlayerScript>().anim.Play("CreateFireball");
        }

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
        fireballTimeValue = 2;
        isCastingSpell = false;
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
        fireballTimeValue = 2;
        isCastingSpell = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, autoRange);
    }
}