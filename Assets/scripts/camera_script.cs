using UnityEngine;
using Mirror;

public class PlayerCamera : NetworkBehaviour
{
    public Camera playerCamera;

    // This is called when the player object is instantiated
    public override void OnStartAuthority()
    {
        playerCamera.enabled = true;
    }
}