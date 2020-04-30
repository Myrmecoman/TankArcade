using UnityEngine;
using UnityEngine.UI;


public class UpdateInputs : MonoBehaviour
{
    public Keybindings binds;
    public Text[] texts;

    private string forw;
    private string back;
    private string left;
    private string right;
    private string shoot;


    private void Start()
    {
        forw = PlayerPrefs.GetString("forward");
        if (forw == "")
            forw = "Z";
        back = PlayerPrefs.GetString("backward");
        if (back == "")
            back = "S";
        left = PlayerPrefs.GetString("left");
        if (left == "")
            left = "Q";
        right = PlayerPrefs.GetString("right");
        if (right == "")
            right = "D";
        shoot = PlayerPrefs.GetString("shoot");
        if (shoot == "")
            shoot = "Mouse0";
        binds.keybindingChecks[0].keycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), forw);
        binds.keybindingChecks[1].keycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), back);
        binds.keybindingChecks[2].keycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), left);
        binds.keybindingChecks[3].keycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), right);
        binds.keybindingChecks[5].keycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), shoot);
    }


    public void UpdateTexts()
    {
        for (int i = 0; i < texts.Length; i++)
            texts[i].text = binds.keybindingChecks[i].keycode.ToString();
    }
}