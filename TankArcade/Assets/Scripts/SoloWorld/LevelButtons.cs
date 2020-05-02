using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelButtons : MonoBehaviour
{
    public GameObject wintxt;
    [HideInInspector] public bool win = false;

    private float winTimeAfter = 0;
    private float destroyNb = 0;
    private BotController[] bots;


    void Start()
    {
        DontDestroyOnLoad(this);
        bots = FindObjectsOfType(typeof(BotController)) as BotController[];
        destroyNb = bots.Length;
    }


    void Update()
    {
        if (win)
            winTimeAfter += Time.deltaTime;
        if(winTimeAfter > 5)
            SceneManager.LoadScene(1);
    }


    public void DestroySignal()
    {
        destroyNb -= 1;
        if (destroyNb == 0)
        {
            win = true;
            wintxt.SetActive(true);
        }
    }


    public void Exit()
    {
        SceneManager.LoadScene(1);
    }


    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Destroy(gameObject);
    }
}