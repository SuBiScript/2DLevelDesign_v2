using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

// Gestor de configuracions i events dels settings.
public class SettingsController : MonoBehaviour
{

    public Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Dropdown qualityDropdown;
    public Dropdown antiAliasingDropdown;
    public Slider audioSlider;
    public Button applyButton;

    //public AudioManager audioSource;
    //public AudioSource audioSource;
    public Resolution[] resolutionOptions;
    public GameSettings gameSettings;
    //private bool savedData = false;

    void OnEnable()
    {
        gameSettings = new GameSettings();

        //Es subscriu cada toggle, dropdown i Slider al seu event corresponent.
        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        qualityDropdown.onValueChanged.AddListener(delegate { OnQualityChange(); });
        antiAliasingDropdown.onValueChanged.AddListener(delegate { OnAntialiasingChange(); });
        audioSlider.onValueChanged.AddListener(delegate { OnAudioChange(); });
        //applyButton.onClick.AddListener(delegate { OnApplyButtonClick(); });

        resolutionOptions = Screen.resolutions;
        foreach (Resolution resolution in resolutionOptions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }

        LoadSettings();

        /*if (File.Exists(Application.persistentDataPath + "/gamesettings.json") == true)
        {
            LoadSettings();
        }*/
    }

    private void OnDisable()
    {
        SaveSettings();
    }

    public void OnFullscreenToggle()
    {
        if (!fullscreenToggle.isOn)
        {
            gameSettings.fullScreen = 1;
            Screen.fullScreen = true;
        }
        else 
        {
            gameSettings.fullScreen = 0;
            Screen.fullScreen = false;
        }
    }

    public void OnResolutionChange()
    {
        Screen.SetResolution(resolutionOptions[resolutionDropdown.value].width, resolutionOptions[resolutionDropdown.value].height, Screen.fullScreen);
        gameSettings.resolutionIndex = resolutionDropdown.value;
    }

    public void OnQualityChange()
    {
        QualitySettings.masterTextureLimit = gameSettings.quality = qualityDropdown.value;
    }

    public void OnAntialiasingChange()
    {
        QualitySettings.antiAliasing = gameSettings.antiAliasing = (int)antiAliasingDropdown.value;
    }

    public void OnAudioChange()
    {
        AudioListener.volume = gameSettings.audioVolume = audioSlider.value;
    }



    public void OnApplyButtonClick()
    {
        SaveSettings();
    }

    /*
    public void SaveSettings()
    {
        //Data InGame Settings que es guardaran.
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);                  
    }

    public void LoadSettings()
    {
        //Data InGame Settings que es carregaran.
        gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));
        audioSlider.value = gameSettings.audioVolume;
        antiAliasingDropdown.value = gameSettings.antiAliasing;
        qualityDropdown.value = gameSettings.quality;
        resolutionDropdown.value = gameSettings.resolutionIndex;
        fullscreenToggle.isOn = gameSettings.fullScreen;
        Screen.fullScreen = gameSettings.fullScreen;

        resolutionDropdown.RefreshShownValue();
    }
    */

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("fullScreenValue", gameSettings.fullScreen);
        PlayerPrefs.SetInt("resolutionValue", gameSettings.resolutionIndex);
        PlayerPrefs.SetInt("qualityValue", gameSettings.quality);
        PlayerPrefs.SetInt("AAValue", gameSettings.antiAliasing);
        PlayerPrefs.SetFloat("audioSliderValue", gameSettings.audioVolume);
        //Debug.Log("SAVING");
    }

    public void LoadSettings()
    {
        gameSettings.fullScreen = PlayerPrefs.GetInt("fullScreenValue");
        if (gameSettings.fullScreen >= 1)
        {
            fullscreenToggle.isOn = true;
            Screen.fullScreen = true;
        }
        else
        {
            fullscreenToggle.isOn = false;
            Screen.fullScreen = false;
            Debug.Log("Full screen is " + Screen.fullScreen);
        }
        resolutionDropdown.value = gameSettings.resolutionIndex = PlayerPrefs.GetInt("resolutionValue");
        QualitySettings.masterTextureLimit = qualityDropdown.value = gameSettings.quality = PlayerPrefs.GetInt("qualityValue");
        QualitySettings.antiAliasing = antiAliasingDropdown.value = gameSettings.antiAliasing = PlayerPrefs.GetInt("AAValue");
        AudioListener.volume = audioSlider.value = gameSettings.audioVolume = PlayerPrefs.GetFloat("audioSliderValue");
        //Debug.Log("LOADING");
    }
}
