using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class Settings : MonoBehaviour
{
    public Dropdown ResolutionDropdown;
    public Dropdown QualityDropdown;
    public Toggle fullScreenToggle;
    public RenderPipelineAsset LowQ;
    public RenderPipelineAsset MediumQ;
    public RenderPipelineAsset HighQ;


    void Start()
    {
        int res = PlayerPrefs.GetInt("screenRes", 6);
        int qual = PlayerPrefs.GetInt("gameQuality", 2);

        ResolutionDropdown.value = res;
        ResolutionDropdown.RefreshShownValue();

        QualityDropdown.value = qual;
        QualityDropdown.RefreshShownValue();

        if (Screen.fullScreen)
            fullScreenToggle.isOn = true;
        else
            fullScreenToggle.isOn = false;
    }


    public void SetResolution()
    {

        int a = 1920;
        int b = 1080;
        if (ResolutionDropdown.value == 0)
        {
            a = 800;
            b = 600;
        }
        if (ResolutionDropdown.value == 1)
        {
            a = 1024;
            b = 720;
        }
        if (ResolutionDropdown.value == 2)
        {
            a = 1280;
            b = 800;
        }
        if (ResolutionDropdown.value == 3)
        {
            a = 1360;
            b = 768;
        }
        if (ResolutionDropdown.value == 4)
        {
            a = 1600;
            b = 900;
        }
        if (ResolutionDropdown.value == 5)
        {
            a = 1680;
            b = 1050;
        }
        if (ResolutionDropdown.value == 6)
        {
            a = 1920;
            b = 1080;
        }
        Screen.SetResolution(a, b, Screen.fullScreen);
        PlayerPrefs.SetInt("screenRes", ResolutionDropdown.value);
    }


    public void SetQuality()
    {
        if (QualityDropdown.value == 0)
            GraphicsSettings.renderPipelineAsset = LowQ;
        if (QualityDropdown.value == 1)
            GraphicsSettings.renderPipelineAsset = MediumQ;
        if (QualityDropdown.value == 2)
            GraphicsSettings.renderPipelineAsset = HighQ;
        PlayerPrefs.SetInt("gameQuality", QualityDropdown.value);
    }


    public void SetFullscreen()
    {
        if(fullScreenToggle.isOn && !Screen.fullScreen)
            Screen.fullScreen = true;
        if (!fullScreenToggle.isOn && Screen.fullScreen)
            Screen.fullScreen = false;
    }
}