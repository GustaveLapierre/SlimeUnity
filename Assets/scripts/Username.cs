using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Username : MonoBehaviour
{
    public string username;

    //public InputField mainInputField;
    //[SerializeField] 
    //public TextMeshProUGUI usernameText;
    public TMP_InputField usernameText;


    void Start()
    {

        if (PersistObject.Instance != null)
        {
            username = PersistObject.Instance.username;

            if (username != "")

        	{
                usernameText.text = username;
            }
        }
        else
        {
            PersistObject.Instance.username = "Anne Onyme";
            usernameText.text = "username";

        }
    }

    public void SubmitName(string arg0)
    {
        //Debug.Log("End Edit on ["+arg0+"]");
        PersistObject.Instance.username = arg0;
    }
}