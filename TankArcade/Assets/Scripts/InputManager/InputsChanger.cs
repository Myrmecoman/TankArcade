using UnityEngine;
using UnityEngine.UI;


public class InputsChanger : MonoBehaviour
{
    public Keybindings binds;

    public Text forwardTxt;
    public Text backwardTxt;
    public Text leftTxt;
    public Text rightTxt;
    public Text shootTxt;
    public Text camLeft;
    public Text camRight;

    private bool selected = false;
    private string nameButton;


    public void ButtonPressed(string nameB)
    {
        nameButton = nameB;
    }


    void Update()
    {
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(vKey))
            {
                if (nameButton == "forward")
                {
                    forwardTxt.text = vKey.ToString();
                    binds.keybindingChecks[0].keycode = vKey;
                    PlayerPrefs.SetString("forward", vKey.ToString());
                }
                if (nameButton == "backward")
                {
                    backwardTxt.text = vKey.ToString();
                    binds.keybindingChecks[1].keycode = vKey;
                    PlayerPrefs.SetString("backward", vKey.ToString());
                }
                if (nameButton == "left")
                {
                    leftTxt.text = vKey.ToString();
                    binds.keybindingChecks[2].keycode = vKey;
                    PlayerPrefs.SetString("left", vKey.ToString());
                }
                if (nameButton == "right")
                {
                    rightTxt.text = vKey.ToString();
                    binds.keybindingChecks[3].keycode = vKey;
                    PlayerPrefs.SetString("right", vKey.ToString());
                }
                if (nameButton == "shoot")
                {
                    shootTxt.text = vKey.ToString();
                    binds.keybindingChecks[4].keycode = vKey;
                    PlayerPrefs.SetString("shoot", vKey.ToString());
                }
                if (nameButton == "camLeft")
                {
                    rightTxt.text = vKey.ToString();
                    binds.keybindingChecks[5].keycode = vKey;
                    PlayerPrefs.SetString("camLeft", vKey.ToString());
                }
                if (nameButton == "camRight")
                {
                    shootTxt.text = vKey.ToString();
                    binds.keybindingChecks[6].keycode = vKey;
                    PlayerPrefs.SetString("camRight", vKey.ToString());
                }
                selected = true;
            }
        }

        if (selected)
        {
            selected = false;
            gameObject.SetActive(false);
        }
    }
}