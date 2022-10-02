using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Assertions.Must;
using System.Linq;

public class SettingsMenu : MonoBehaviour
{
    [Header("Mixer")]
    public AudioMixer masterMixer;

    [Header("Dropdowns")]
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;

    [Header("Sliders")]
    public Slider master;
    public Slider sfx;
    public Slider music;

    [Header("Toggles")]
    public BetterToggle fullscreenToggle;
    public BetterToggle vSyncToggle;
    public BetterToggle motionBlurToggle;
    public BetterToggle bloomToggle;

    [Header("PostFX Profile")]
    public Volume postFx;

    private MotionBlur motionBlur;
    private Bloom bloom;

    private Resolution[] resolutions;

    private List<Resolution> resolutionsList = new List<Resolution>();
    private void Awake()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        if (resolutionDropdown)
            GetResolutions();

        postFx.sharedProfile.TryGet(out motionBlur);
        postFx.sharedProfile.TryGet(out bloom);

        SetMasterVolume(PlayerPrefs.GetFloat("masterVolume", 1f));
        if (master)
            master.value = PlayerPrefs.GetFloat("masterVolume", 1f);

        SetSFXVolume(PlayerPrefs.GetFloat("sfxVolume", 1f));
        if (sfx)
            sfx.value = PlayerPrefs.GetFloat("sfxVolume", 1f);

        SetMusicVolume(PlayerPrefs.GetFloat("musicVolume", 1f));
        if (music)
            music.value = PlayerPrefs.GetFloat("musicVolume", 1f);

        if (qualityDropdown)
        {
            SetQuality(PlayerPrefs.GetInt("qualityIndex", 2));
            qualityDropdown.value = PlayerPrefs.GetInt("qualityIndex", 2); ;
            qualityDropdown.RefreshShownValue();
        }

        if (resolutionDropdown)
        {
            SetResolution(PlayerPrefs.GetInt("resIndex", resolutions.Length));
            resolutionDropdown.value = PlayerPrefs.GetInt("resIndex", resolutions.Length);
            resolutionDropdown.RefreshShownValue();
        }
        if (fullscreenToggle)
        {
            SetFullscreen(PlayerPrefs.GetInt("fullscreenBool", 1) != 0);
            fullscreenToggle.isOn = PlayerPrefs.GetInt("fullscreenBool", 1) != 0;
        }
        if (vSyncToggle)
        {
            SetVSync(PlayerPrefs.GetInt("vSyncInt", 1) != 0);
            vSyncToggle.isOn = PlayerPrefs.GetInt("vSyncInt", 1) != 0;
        }
        if (bloomToggle)
        {
            SetBloom(PlayerPrefs.GetInt("bloomBool", 1) != 0);
            bloomToggle.isOn = PlayerPrefs.GetInt("bloomBool", 1) != 0;
        }
        if (motionBlurToggle)
        {
            SetMotionBlur(PlayerPrefs.GetInt("motionBlurBool", 1) != 0);
            motionBlurToggle.isOn = PlayerPrefs.GetInt("motionBlurBool", 1) != 0;
        }
    }

    public void ResetAllSettings()
    {
        PlayerPrefs.DeleteKey("masterVolume");
        PlayerPrefs.DeleteKey("sfxVolume");
        PlayerPrefs.DeleteKey("musicVolume");
        PlayerPrefs.DeleteKey("resIndex");
        PlayerPrefs.DeleteKey("qualityIndex");
        PlayerPrefs.DeleteKey("motionBlurBool");
        PlayerPrefs.DeleteKey("bloomBool");
        PlayerPrefs.DeleteKey("fullscreenBool");
        PlayerPrefs.DeleteKey("vSyncBool");

        LoadSettings();
    }

    public void GetResolutions()
    {
        //get and set the resolutions for the dropdown
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        resolutionsList.Clear();

        var options = new List<string>();

        foreach (Resolution resolution in resolutions)
        {
            string option = resolution.width + "x" + resolution.height;
            if (!options.Contains(option))
            {
                options.Add(option);
                resolutionsList.Add(resolution);
                // Debug.Log(option + " " + i);
            }
        }

        PlayerPrefs.SetInt("resIndex", resolutions.Length); //the last in the list
        PlayerPrefs.Save();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("resIndex");
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resIndex)
    {
        Resolution res = resolutionsList[resIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);

        //Debug.Log(resIndex + " " + res.width + " " + res.height);

        PlayerPrefs.SetInt("resIndex", resIndex);
        PlayerPrefs.Save();
    }

    //set the quality of the game
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);

        PlayerPrefs.SetInt("qualityIndex", qualityIndex);
        PlayerPrefs.Save();
    }

    //control the different volume sliders
    public void SetMasterVolume(float volume)
    {
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);

        PlayerPrefs.SetFloat("masterVolume", volume);
        PlayerPrefs.Save();
    }
    public void SetMusicVolume(float volume)
    {
        masterMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);

        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save();
    }
    public void SetSFXVolume(float volume)
    {
        masterMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);

        PlayerPrefs.SetFloat("sfxVolume", volume);
        PlayerPrefs.Save();
    }

    //set the fullscreen of the game
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

        PlayerPrefs.SetInt("fullscreenBool", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }
    //toggle the vsync of the game
    public void SetVSync(bool toggle)
    {
        int isVsync = toggle ? 1 : 0;
        
        QualitySettings.vSyncCount = isVsync;

        PlayerPrefs.SetInt("vSyncBool", isVsync);
        PlayerPrefs.Save();
    }
    public void SetBloom(bool isBloom)
    {
        bloom.active = isBloom;

        PlayerPrefs.SetInt("bloomBool", isBloom ? 1 : 0);
        PlayerPrefs.Save();
    }
    public void SetMotionBlur(bool isMB)
    {
        motionBlur.active = isMB;

        PlayerPrefs.SetInt("motionBlurBool", isMB ? 1 : 0);
        PlayerPrefs.Save();
    }
}
