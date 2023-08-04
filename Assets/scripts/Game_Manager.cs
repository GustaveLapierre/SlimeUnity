using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Text;



public class Game_Manager : MonoBehaviour
{
    [SerializeField] public int _coins;
    private int numberofcoins;
    private string apiURL = "http://127.0.0.1:8000/api";
    private static Game_Manager _instance;
    public static Game_Manager Instance
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

    void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _coins = 0;
        UIManager.Instance.UpdateCoinText(_coins);
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void AddCoins()
    {
        _coins++;
        UIManager.Instance.UpdateCoinText(_coins);
    }

    public int Coins
    {
        get
        {
            return _coins;
        }
    }
    public IEnumerator PostHighScore(string username, float timer, string difficulty)
    {
        // Create JSON data
        string json = string.Format("{{\"username\": \"{0}\", \"timer\": {1}, \"gamemode\": \"{2}\"}}", username, timer,
            difficulty);
        Debug.Log(username);

        // Convert to bytes
        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

        // Create a new UnityWebRequest
        UnityWebRequest www = new UnityWebRequest(apiURL, "POST");

        // Set the uploadHandler to the raw json bytes and set the content type to JSON
        www.uploadHandler = new UploadHandlerRaw(jsonBytes);
        www.uploadHandler.contentType = "application/json";

        // Set the downloadHandler to a new DownloadHandlerBuffer
        www.downloadHandler = new DownloadHandlerBuffer();

        // Send the request
        yield return www.SendWebRequest();

        // Error handling
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("HighScore posted successfully");
        }
    }
}
