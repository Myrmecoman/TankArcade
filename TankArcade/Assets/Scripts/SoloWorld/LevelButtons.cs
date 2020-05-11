using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class LevelButtons : MonoBehaviour
{
    public GameObject wintxt;
    public TextMeshProUGUI time;
    [HideInInspector] public bool win = false;
    [HideInInspector] public double completeTime;

    private float winTimeAfter = 0;
    private float destroyNb = 0;
    private BotController[] bots;


    void Start()
    {
        completeTime = 0;
        DontDestroyOnLoad(this);
        bots = FindObjectsOfType(typeof(BotController)) as BotController[];
        destroyNb = bots.Length;
    }


    void Update()
    {
        if (!win)
        {
            completeTime += Time.deltaTime;
            time.text = String.Format("{0:0.00}", completeTime);
        }
        if (win)
            winTimeAfter += Time.deltaTime;
        if(winTimeAfter > 5)
            SceneManager.LoadScene(3);
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
        SceneManager.LoadScene(3);
    }


    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Destroy(gameObject);
    }
}