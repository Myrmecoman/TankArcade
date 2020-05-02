using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuButtons : MonoBehaviour
{
    public GameObject subMenu;
    public GameObject[] submenuObjs;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(subMenu.activeSelf)
            {
                foreach (GameObject g in submenuObjs)
                    g.SetActive(false);
            }
            subMenu.SetActive(!subMenu.activeSelf);
        }
    }


    public void LoadSolo()
    {
        SceneManager.LoadScene(1);
    }


    public void LoadMulti()
    {
        
    }
}