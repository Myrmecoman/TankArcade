using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Mirror;


public class MultiScenes : NetworkManager
{
    [Header("MultiScene Setup")]
    public int instances = 3;
    public int PlayersPerRoom = 2;

    [Scene]
    public string gameScene;

    readonly List<Scene> subScenes = new List<Scene>();

    private int[] ScenesPlayers;


    private new void Start()
    {
        ScenesPlayers = new int[instances];
        for (int i = 0; i < ScenesPlayers.Length; i++)
            ScenesPlayers[i] = 0;
    }


    #region Server System Callbacks

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        // This delay is really for the host player that loads too fast for the server to have subscene loaded
        StartCoroutine(AddPlayerDelayed(conn));
    }

    IEnumerator AddPlayerDelayed(NetworkConnection conn)
    {
        yield return new WaitForSeconds(.5f);
        conn.Send(new SceneMessage { sceneName = gameScene, sceneOperation = SceneOperation.LoadAdditive });

        base.OnServerAddPlayer(conn);

        if (subScenes.Count > 0)
        {
            int connectToScene = 0;
            while (connectToScene < instances)
            {
                if (ScenesPlayers[connectToScene] >= PlayersPerRoom)
                    connectToScene++;
                else
                    break;
            }

            if (connectToScene == instances)
                Debug.LogWarning("All rooms are full !");
            else
            {
                Debug.Log("New player ID : " + conn.connectionId + " was added to room n°" + connectToScene);
                ScenesPlayers[connectToScene]++;
                SceneManager.MoveGameObjectToScene(conn.identity.gameObject, subScenes[connectToScene]);
            }
        }
    }

    #endregion

    #region Start & Stop Callbacks

    public override void OnStartServer()
    {
        StartCoroutine(LoadSubScenes());
    }

    IEnumerator LoadSubScenes()
    {
        for (int index = 0; index < instances; index++)
        {
            yield return SceneManager.LoadSceneAsync(gameScene, new LoadSceneParameters { loadSceneMode = LoadSceneMode.Additive, localPhysicsMode = LocalPhysicsMode.Physics3D });
            subScenes.Add(SceneManager.GetSceneAt(index + 1));
        }
    }


    void FixedUpdate()
    {
        for (int i = 0; i < subScenes.Count; i++)
            subScenes[i].GetPhysicsScene().Simulate(Time.fixedDeltaTime * 1 /* timescale */);
    }

    public override void OnStopServer()
    {
        NetworkServer.SendToAll(new SceneMessage { sceneName = gameScene, sceneOperation = SceneOperation.UnloadAdditive });
        StartCoroutine(UnloadSubScenes());
    }

    public override void OnStopClient()
    {
        if (mode == NetworkManagerMode.ClientOnly)
            StartCoroutine(UnloadClientSubScenes());
    }

    IEnumerator UnloadClientSubScenes()
    {
        for (int index = 0; index < SceneManager.sceneCount; index++)
        {
            if (SceneManager.GetSceneAt(index) != SceneManager.GetActiveScene())
                yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(index));
        }
    }

    IEnumerator UnloadSubScenes()
    {
        for (int index = 0; index < subScenes.Count; index++)
            yield return SceneManager.UnloadSceneAsync(subScenes[index]);

        subScenes.Clear();

        yield return Resources.UnloadUnusedAssets();
    }

    #endregion
}