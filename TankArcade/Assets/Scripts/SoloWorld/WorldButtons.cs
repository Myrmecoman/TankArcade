using UnityEngine;
using UnityEngine.SceneManagement;


public class WorldButtons : MonoBehaviour
{
    public CharacterManager character;


    public void QuitToLobby()
    {
        character.SavePlayer();
        SceneManager.LoadScene(0);
    }
}