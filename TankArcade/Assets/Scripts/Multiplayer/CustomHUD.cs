using Mirror.Examples.MultipleAdditiveScenes;
using System.ComponentModel;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
[RequireComponent(typeof(MultiScenes))]
[EditorBrowsable(EditorBrowsableState.Never)]
public class CustomHUD : MonoBehaviour
{
    MultiScenes manager;
    public bool buildServer = false;
    public bool showGUI = true;
    public int offsetX;
    public int offsetY;

    void Awake()
    {
        manager = GetComponent<MultiScenes>();
    }

    void OnGUI()
    {
        if (!showGUI)
            return;

        GUILayout.BeginArea(new Rect(10 + offsetX, 40 + offsetY, 215, 9999));
        if (!NetworkClient.isConnected && !NetworkServer.active)
            StartButtons();
        else
            StatusLabels();

        // client ready
        if (NetworkClient.isConnected && !ClientScene.ready)
        {
            if (GUILayout.Button("Client Ready"))
            {
                ClientScene.Ready(NetworkClient.connection);
                if (ClientScene.localPlayer == null)
                    ClientScene.AddPlayer(NetworkClient.connection);
            }
        }

        StopButtons();

        GUILayout.EndArea();
    }

    void StartButtons()
    {
        if (!NetworkClient.active)
        {
            // Client + IP
            GUILayout.BeginHorizontal();
            if (!buildServer)
                manager.StartClient();
            manager.networkAddress = "84.102.229.136";
            GUILayout.EndHorizontal();

            if (buildServer && GUILayout.Button("Server"))
                manager.StartServer();
        }
        else
        {
            // Connecting
            GUILayout.Label("Trying to connect to server...\nIt may not be live !");
            if (GUILayout.Button("Cancel Connection Attempt"))
            {
                manager.StopClient();
                SceneManager.LoadScene(0);
            }
        }
    }


    void StatusLabels()
    {
        // server status message
        if (NetworkServer.active)
            GUILayout.Label("Server: active. Transport: " + Transport.activeTransport);
    }

    void StopButtons()
    {
        // stop server if we are server
        if (NetworkServer.active)
        {
            if (GUILayout.Button("Stop Server"))
                manager.StopServer();
        }
    }
}