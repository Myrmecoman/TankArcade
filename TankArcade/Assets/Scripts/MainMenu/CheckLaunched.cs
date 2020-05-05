using UnityEngine;


public class CheckLaunched : MonoBehaviour
{
    public bool DisableScript = true;


    void Start()
    {
        if (DisableScript)
            return;

        int run = PlayerPrefs.GetInt("isRunning", 0);
        if (run == 1)
            Application.Quit();
        else
            PlayerPrefs.SetInt("isRunning", 1);
    }


    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("isRunning", 0);
    }
}