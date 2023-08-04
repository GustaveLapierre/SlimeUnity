using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flag : MonoBehaviour
{
    public int a;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D hit)
    {
        if (Game_Manager.Instance._coins == 10 ){
            //Game_Manager.Instance.AddCoins();
            UIManager.Instance.UpdateWin();
            Destroy(this.gameObject);
		}
    }
}
