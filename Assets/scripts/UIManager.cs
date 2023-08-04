using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Mirror;


public class UIManager : NetworkBehaviour
{
    public Text coinText;
    public Text timeText;
    public PlayerController playerController;
    
    [SyncVar]
    private float time;
    
    private string difficulty;
    public string username;
    public bool win;
    private bool endbool;
    private NetworkManager manager;

    private static UIManager _instance;
    public static UIManager Instance
    
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Player is null");
            }
            return _instance;
        }
    }

    void Awake() { _instance = this; }
    // Start is called before the first frame update
    void Start()
    {
        if (PersistObject.Instance != null)
        {
            difficulty = PersistObject.Instance.difficulty;
            username = PersistObject.Instance.username;
        }
        else
        {
            difficulty = "easy";
            username = "Anne Onyme";
        }

        coinText = GetComponent<Text>();
        if (isServer)
        {
            if (difficulty == "hard")
            {
                time = 300;  
            }
            else
            {
                time = 1200;
            }
        }
        
        
        playerController = FindObjectOfType<PlayerController>();
        manager = FindObjectOfType<NetworkManager>();

        win = false;
        endbool = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0 && win == false && isServer)
        {
            time -= Time.deltaTime;
        }
        else if (time<= 0)
        {
            StartCoroutine(playerController.RespawnCoroutine());
        }

        if (time > 0 && win == true && endbool)
        //if(time < 1198 && endbool)
        {
            endbool = false;
            StartCoroutine(EndCoroutine());   
        }

        Updatetimetext(time);
    }
    
    public void UpdateCoinText(int coins)
    {
        coinText.text = $"{coins} / 10";
    }
    public void Updatetimetext(float time)
    {
        int time2 = (int)time;
        timeText.text = $"{time2}";
    }

    public void UpdateWin()
    {
        win = true;
    }
    public IEnumerator EndCoroutine()
    {
        if ((username == "") || (username == null))
            username = "Anne Onyme";
        StartCoroutine(Game_Manager.Instance.PostHighScore(username, time, difficulty));
        yield return new WaitForSeconds(3f);
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            manager.StopHost();
        }
        // stop client if client-only
        else if (NetworkClient.isConnected)
        {
            manager.StopClient();
        }
        // stop server if server-only
        else if (NetworkServer.active)
        {
            manager.StopServer();
        }

        SceneManager.LoadScene("MainMenu");
    }


}
