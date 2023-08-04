using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;


public class HighScoreManager : MonoBehaviour
{
    public string apiURL = "http://127.0.0.1:8000/api";
    public GameObject contentParent;
    public Text highScoreItemPrefab;
    public RectTransform contentParentRectTransform;
    private string difficulty;


    void Start()
    {
        StartCoroutine(GetHighScores());
        difficulty = PersistObject.Instance.difficulty;
    }

    void Update()
    {
        if (difficulty != PersistObject.Instance.difficulty)
        {
            StartCoroutine(GetHighScores());
            difficulty = PersistObject.Instance.difficulty;
        }
    }
    
    public IEnumerator GetHighScores()
    {
        UnityWebRequest www = new UnityWebRequest(apiURL, "GET");
        www.downloadHandler = new DownloadHandlerBuffer();
        
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            DashboardResponse response = JsonUtility.FromJson<DashboardResponse>(www.downloadHandler.text);
            
            response.dashboard.Sort((score1, score2) => score2.timer.CompareTo(score1.timer));

            // Clear old scores
           foreach (Transform child in contentParent.transform) 
            {
                Destroy(child.gameObject);
            }

            // Iterate over the "dashboard" list
            foreach (var score in response.dashboard)
            {
                if (score.gamemode != PersistObject.Instance.difficulty) continue;
                Text scoreItem = Instantiate(highScoreItemPrefab, contentParent.transform);
                scoreItem.text = score.username + ": " + score.timer;// + score.gamemode; // set the text of the prefab
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentParentRectTransform);


            //contentParent.transform.parent.parent.GameObject.GetComponent<ScrollView>().;
        }
    }
}


[System.Serializable]
public class Score
{
    public int id;
    public string username;
    public float timer;
    public string gamemode; 
}

[System.Serializable]
public class DashboardResponse
{
    public List<Score> dashboard;
}