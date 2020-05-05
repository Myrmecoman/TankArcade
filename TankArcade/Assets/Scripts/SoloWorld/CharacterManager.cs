using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public class CharacterManager : MonoBehaviour
{
    public int startLevelIndex;
    public Material disabled;
    public Text timeText;
    public Transform[] SlotList;
    [HideInInspector] public double[] times;
    [HideInInspector] public int CurrentIndex;
    [HideInInspector] public int maxIndex;

    private InputManager im;


    private void Start()
    {
        im = InputManager.instance;
        times = new double[SlotList.Length];
        for (int i = 0; i < SlotList.Length; i++)
            times[i] = -1;
        CurrentIndex = 0;
        maxIndex = 0;

        try
        {
            LoadPlayer();
        }
        catch
        {
            SavePlayer();
            LoadPlayer();
        }

        LevelButtons lb = FindObjectOfType<LevelButtons>();
        if (lb)
        {
            if (lb.win)
            {
                if(CurrentIndex == maxIndex && maxIndex < SlotList.Length - 1)
                    maxIndex += 1;
                if (times[CurrentIndex] == -1 || lb.completeTime < times[CurrentIndex])
                    times[CurrentIndex] = lb.completeTime;
            }
            Destroy(lb.gameObject);
        }
        if(times[CurrentIndex] != -1)
            timeText.text = "Best time : " + String.Format("{0:0.00}", times[CurrentIndex]);
        else
            timeText.text = "No time yet";
        SavePlayer();
        SetSlots();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SavePlayer();
            SceneManager.LoadScene(startLevelIndex + CurrentIndex);
        }

        if ((im.GetKey(KeybindingActions.left) || Input.GetKeyDown(KeyCode.LeftArrow)) && CurrentIndex > 0)
        {
            CurrentIndex--;
            transform.position = new Vector3(SlotList[CurrentIndex].position.x, SlotList[CurrentIndex].position.y + 2.3f, SlotList[CurrentIndex].position.z);
            if (times[CurrentIndex] != -1)
                timeText.text = "Best time : " + String.Format("{0:0.00}", times[CurrentIndex]);
            else
                timeText.text = "No time yet";
        }

        if ((im.GetKey(KeybindingActions.right) || Input.GetKeyDown(KeyCode.RightArrow)) && CurrentIndex < maxIndex)
        {
            CurrentIndex++;
            transform.position = new Vector3(SlotList[CurrentIndex].position.x, SlotList[CurrentIndex].position.y + 2.3f, SlotList[CurrentIndex].position.z);
            if (times[CurrentIndex] != -1)
                timeText.text = "Best time : " + String.Format("{0:0.00}", times[CurrentIndex]);
            else
                timeText.text = "No time yet";
        }

        if(Input.GetKeyDown(KeyCode.X))
            Debug.Log("current index : " + CurrentIndex + "; max index : " + maxIndex + "; time here : " + times[CurrentIndex]);
    }


    public void SetSlots()
    {
        for(int i = 0; i < SlotList.Length; i++)
        {
            if (i > maxIndex)
                SlotList[i].GetComponent<Renderer>().material = disabled;
        }
    }


    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }


    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        CurrentIndex = data.Currentindex;
        maxIndex = data.maxIndex;
        for(int i = 0; i <= maxIndex; i++)
            times[i] = data.times[i];
        transform.position = new Vector3(SlotList[CurrentIndex].position.x, SlotList[CurrentIndex].position.y + 2.3f, SlotList[CurrentIndex].position.z);
    }
}