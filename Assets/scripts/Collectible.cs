using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float speed = 50f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);

    }
    
    void OnTriggerEnter2D(Collider2D hit)
    {
		Game_Manager.Instance.AddCoins();
        Destroy(this .gameObject);
    }
    

}
