using UnityEngine;
using Mirror;

public class start_script : MonoBehaviour
{
    // Reference to the NetworkManager
    private NetworkManager manager;

    private void Start()
    {

        manager = FindObjectOfType<NetworkManager>();

    }

    public void StartAsHost() 
    {
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.LogError("No Internet connection found, unable to start game.");
                return;
            }

            // Start as host
            manager.StartHost();
            manager.ServerChangeScene("Main_game");
        }
    }

    public void ConnectAsClient() 
    {
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.LogError("No Internet connection found, unable to start game.");
                return;
            }

            // Connect as client
            manager.StartClient();
        }
    }

}