using UnityEngine;
using System.Collections;
using Mirror;

public class MovingPlatform : NetworkBehaviour
{
    private float speed = 1f;
    private float distance = 5f;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 targetPosition;
    private SpriteRenderer spriteRenderer;

    [SyncVar(hook = nameof(OnFlipChanged))]
    private bool isFlipped = false;
    
    private GameObject playerOnPlatform = null;

    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + new Vector3(distance, 0, 0);
        targetPosition = endPosition;

        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(MovePlatform());
        
    }
    
    
    [ClientRpc]
    private void RpcMovePlayer(NetworkIdentity player, Vector3 movement)
    {
        
        // server tells all clients to update this player's position
        if (player.isLocalPlayer)
        {
            player.transform.position += movement;
        }    
    }

    IEnumerator MovePlatform()
    {
        while (true)
        {
            Vector3 oldPosition = transform.position;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            Vector3 movement = transform.position - oldPosition;
            if (playerOnPlatform != null && isServer)
            {
                RpcMovePlayer(playerOnPlatform.GetComponent<NetworkIdentity>(), movement);
            }

            if (transform.position == targetPosition)
            {
                // Swap the target position
                targetPosition = targetPosition == startPosition ? endPosition : startPosition;

                // flip the sprite on server and sync with clients
                isFlipped = !isFlipped;

                // Wait for a short moment before moving the platform again
                yield return new WaitForSeconds(0.1f);
            }
            yield return null;
        }
    }


    private void OnFlipChanged(bool oldIsFlipped, bool newIsFlipped)
    {
        // flip the sprite on all clients
        if (spriteRenderer == null)
        {
            Debug.Log("no sprite renderer");
            return;
        }
        spriteRenderer.flipX = newIsFlipped;
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (isServer)
        {
            playerOnPlatform = col.gameObject;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (isServer && playerOnPlatform == col.gameObject)
        {
            playerOnPlatform = null;
        }
    }
    
    
}

