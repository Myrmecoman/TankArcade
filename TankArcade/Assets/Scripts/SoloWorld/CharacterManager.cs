using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    public int startLevelIndex;
    public Transform[] SlotList;
    [HideInInspector] public int CurrentIndex;
    [HideInInspector] public int maxIndex;

    private InputManager im;


    private void Start()
    {
        im = InputManager.instance;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
            if(lb.win)
                maxIndex += 1;
            Destroy(lb.gameObject);
        }
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
        }

        if ((im.GetKey(KeybindingActions.right) || Input.GetKeyDown(KeyCode.RightArrow)) && CurrentIndex < maxIndex)
        {
            CurrentIndex++;
            transform.position = new Vector3(SlotList[CurrentIndex].position.x, SlotList[CurrentIndex].position.y + 2.3f, SlotList[CurrentIndex].position.z);
        }
    }


    public int GetMaxIndex()
    {
        return 0;
    }


    public void SetSlots(int index)
    {
        
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
        transform.position = new Vector3(SlotList[CurrentIndex].position.x, SlotList[CurrentIndex].position.y + 2.3f, SlotList[CurrentIndex].position.z);
    }
}