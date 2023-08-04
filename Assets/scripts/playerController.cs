using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
//using UnityEditor.PackageManager;


public class PlayerController : NetworkBehaviour
{
    public List<Sprite> anims;
    public float speed = 5f;
    public float jumpForce = 0f;
    public bool isJumping = false;
    public bool readytojump = false;
    public float  timer = 0f;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public bool move = true;
    private float maxJumpForce = 15f;
    private Vector3 starting_position;
    public Color32 objColor;
    private float originalGravity;
    public string difficulty;

    private AudioListener audioListener;
    
    private NetworkManager manager;

    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        originalGravity = rb.gravityScale;
        starting_position = rb.position;
        objColor = sr.color;
        Camera[] cameras = GetComponentsInChildren<Camera>();
        audioListener = GetComponentInChildren<AudioListener>();

        if (PersistObject.Instance != null)
        {
            difficulty = PersistObject.Instance.difficulty;
        }
        else
            difficulty = "easy";
        
        if (!isLocalPlayer)
        {
            // disable the cameras and AudioListener for this player
            foreach(Camera cam in cameras)
            {
                cam.enabled = false;
            }
            audioListener.enabled = false;
        }
        
        manager = FindObjectOfType<NetworkManager>();

    }

    void Update()
    {
        if (this.isLocalPlayer)
        {
            float moveX = Input.GetAxis("Horizontal");

            if (move)
            {
                rb.velocity = new Vector2(moveX * speed, rb.velocity.y);
            }

            if (Input.GetButton("Jump") && !isJumping)
            {
                readytojump = true;
                timer += Time.deltaTime;
                if (timer >= 1.5)
                {
                    sr.color = Color.red;
                }

                move = false;
            }

            if (Input.GetButtonUp("Jump") && !isJumping)
            {
                jumpForce = 10f * timer;
                if (jumpForce > maxJumpForce)
                {
                    jumpForce = maxJumpForce;
                }

                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                isJumping = true;
                readytojump = false;
                move = true;
                timer = 0.8f;
                sr.color = objColor;

            }


            if (Input.GetKeyDown(KeyCode.Escape))
            {
                
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
            
            change_sprite();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isJumping = false;

        }
    }
    
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Respawn"))
        {
            StartCoroutine(RespawnCoroutine());
        }
        if (other.gameObject.CompareTag("coin"))
        {
            //StartCoroutine(CoinCoroutine());
        }
        
    }

    void change_sprite()
    {
        if (this.isLocalPlayer)
        {
            if (move && !isJumping && !readytojump)
            {
                sr.sprite = anims[0];
            }
            else if (isJumping && !readytojump)
            {
                sr.sprite = anims[2];
            }
            else if (!isJumping && readytojump)
            {
                sr.sprite = anims[1];
            }
            else
            {
                sr.sprite = anims[0];
            }
        }
        
    }

    public IEnumerator RespawnCoroutine()
    {
        rb.velocity = Vector2.zero;
        move = false;
        rb.isKinematic = true;
        sr.color = Color.gray;
        yield return new WaitForSeconds(0.9f);
        rb.position = starting_position;
        rb.isKinematic = false;
        move = true;
        sr.color = objColor;
        rb.gravityScale = originalGravity;
        if(difficulty == "hard")
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator CoinCoroutine()
    {
        rb.velocity = Vector2.zero;
        rb.position = starting_position;
        move = false;
        yield return new WaitForSeconds(0.5f);
        move = true;
    }
    
}

