using Mirror.Examples.MultipleAdditiveScenes;
using System.ComponentModel;
using UnityEngine;


namespace Mirror
{
    /// <summary>
    /// An extension for the NetworkManager that displays a default HUD for controlling the network state of the game.
    /// <para>This component also shows useful internal state for the networking system in the inspector window of the editor. It allows users to view connections, networked objects, message handlers, and packet statistics. This information can be helpful when debugging networked games.</para>
    /// </summary>
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
                if (!buildServer && GUILayout.Button("Client"))
                    manager.StartClient();
                manager.networkAddress = GUILayout.TextField(manager.networkAddress);
                GUILayout.EndHorizontal();

                if (buildServer && GUILayout.Button("Server"))
                {
                    #if buildServer || UNITY_EDITOR
                    manager.StartServer();
                    #endif
                }
            }
            else
            {
                // Connecting
                GUILayout.Label("Connecting to " + manager.networkAddress + "...");
                if (GUILayout.Button("Cancel Connection Attempt"))
                    manager.StopClient();
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
}