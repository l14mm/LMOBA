using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class NetworkedPlayerScript : NetworkBehaviour
{
    public ShootingScript shootingScript;
    public GameObject skin;
    public TextMesh nameTag;
    public Transform healthBar;
    Transform healthBarHUD;

    Text playersList;

    Renderer[] renderers;

    [SyncVar]
    public string playerName;
    [SyncVar]
    public Color playerColour;
    [SyncVar]
    public float health;

    [SyncVar]
    public int playerCount;

    [SyncVar]
    public string names;

    public SyncListString namesList;
    private Transform spawnPoint;

    public Camera p_Camera;
    private Camera myCamera;

    //[SyncVar]
    //public List<string> namesList;

    private void FixedUpdate()
    {
        if (health < 100)
            health += 0.5f;
    }

    void Update()
    {
        // Set player name and color of model
        nameTag.text = playerName;
        skin.GetComponent<Renderer>().material.color = playerColour;

        // Hit Screen


        // Healthbar
        //healthBar.localScale = new Vector3(health * 0.005f, 0.2f, 1);
        //healthBarHUD.localScale = new Vector3(health * 0.005f, 0.2f, 1);

        if (namesList != null && false)
        {
            if (namesList.Count == 0)
                playersList.text = "Players: " + playerCount.ToString();
            if (namesList.Count == 1)
                playersList.text = "Players: " + playerCount.ToString() + "\n" + namesList[0].ToString();
            if (namesList.Count == 2)
                playersList.text = "Players: " + playerCount.ToString() + "\n" + namesList[0].ToString() + "\n" + namesList[1].ToString();
            if (namesList.Count == 3)
                playersList.text = "Players: " + playerCount.ToString() + "\n" + namesList[0].ToString() + "\n" + namesList[1].ToString() + "\n" + namesList[2].ToString();
            if (namesList.Count == 4)
                playersList.text = "Players: " + playerCount.ToString() + "\n" + namesList[0].ToString() + "\n" + namesList[1].ToString() + "\n" + namesList[2].ToString() + "\n" + namesList[3].ToString();
        }

        //Debug.Log("namesList Length: " + namesList.Count);

        //if (!isLocalPlayer)
            //return;

        //Debug.Log("playerName: " + playerName.ToString());
        //Debug.Log("names: " + names.ToString());

    }

    void Awake()
    {

        renderers = GetComponentsInChildren<Renderer>();
        health = 100;

        //healthBarHUD = GameObject.Find("HealthHUD").GetComponent<Transform>();
        //playersList = GameObject.Find("Players").GetComponent<Text>();

        namesList = new SyncListString();

        if(spawnPoint == null)
            spawnPoint = transform;
    }

    [Command]
    void CmdSendName(string _playerName)
    {
        //Debug.Log("_playerNameStart: " + _playerName.ToString());
        GetComponent<ServerScript>().RpcUpdateCount(_playerName);
    }

    public override void OnStartLocalPlayer()
    {
        //fpsController.enabled = true;
        shootingScript.enabled = true;
        //candyMaterialSwitcher.SwitchMaterial(true);

        gameObject.name = "LOCAL Player";

        playerName = "Liam";

        //Debug.Log("playerNameStart: " + playerName.ToString());
        CmdSendName(playerName);
        Debug.Log("creating camera");
        myCamera = Instantiate(p_Camera);

        base.OnStartLocalPlayer();        
    }

    void ToggleRenderer(bool isAlive)
    {
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].enabled = isAlive;
    }

    void ToggleControls(bool isAlive)
    {
        shootingScript.enabled = isAlive;
    }

    [ClientRpc]
    public void RpcResolveHit(int damage)
    {
        Debug.Log("ouch");
        health -= damage;
        if (health <= 0)
        {
            //ToggleRenderer(false);
            gameObject.SetActive(false);

            if (isLocalPlayer)
            {
                Transform spawn = NetworkManager.singleton.GetStartPosition();
                transform.position = spawn.position;
                transform.rotation = spawn.rotation;

                ToggleControls(false);
            }
            Invoke("Respawn", 2f);
        }
    }

    void Respawn()
    {
        //ToggleRenderer(true);
        gameObject.SetActive(true);
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        health = 100;

        if (isLocalPlayer)
        {
            ToggleControls(true);
        }
    }
}