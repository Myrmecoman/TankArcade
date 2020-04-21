using UnityEngine;
using MLAPI;


public class ClientOrServer : MonoBehaviour
{
	public bool isServer;
	private bool buttonClicked = false;


	void MakeServer()
	{
		NetworkingManager.Singleton.OnClientConnectedCallback += (clientId) => { Debug.Log($"Client connected {clientId}"); };
		NetworkingManager.Singleton.OnClientDisconnectCallback += (clientId) => { Debug.Log($"Client disconnected {clientId}"); };
		NetworkingManager.Singleton.OnServerStarted += () => { Debug.Log("Server started"); };
		NetworkingManager.Singleton.StartServer();
	}


	void MakeClient()
	{
		NetworkingManager.Singleton.OnClientConnectedCallback += ClientConnected;
		NetworkingManager.Singleton.OnClientDisconnectCallback += ClientDisconnected;
		NetworkingManager.Singleton.StartClient();
	}


	void ClientConnected(ulong clientId)
	{
		Debug.Log($"I'm connected {clientId}");
	}


	void ClientDisconnected(ulong clientId)
	{
		Debug.Log($"I'm disconnected {clientId}");
		NetworkingManager.Singleton.OnClientDisconnectCallback -= ClientDisconnected;   // remove these else they will get called multiple time if we reconnect this client again
		NetworkingManager.Singleton.OnClientConnectedCallback -= ClientConnected;
	}


	void OnGUI()
	{
		if (!buttonClicked)
		{
			GUI.skin.label.fontSize = GUI.skin.box.fontSize = GUI.skin.button.fontSize = GUI.skin.textField.fontSize = 40;
			if (isServer && GUI.Button(new Rect(40, 40, 280, 80), "Create Server"))
			{
				MakeServer();
				buttonClicked = true;
			}

			if (!isServer && GUI.Button(new Rect(40, 40, 280, 80), "Join server"))
			{
				MakeClient();
				buttonClicked = true;
			}
		}
		else if(NetworkingManager.Singleton.IsServer && GUI.Button(new Rect(40, 40, 560, 80), "Server is live, close server ?"))
			Application.Quit();
	}
}