using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject settingsPanel;
    [Header("UI")]
    [SerializeField] private TMP_Dropdown resolutionDrop;
    [SerializeField] private TMP_Dropdown qualityDrop, fpsDrop;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider soundSlider;
    private DataSettings dataSettings;

    void Start()
    {
        LoadSettings();
        SetUIelements();
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("SavedSettings") == true)
        {
            string data = PlayerPrefs.GetString("SavedSettings");
            dataSettings = JsonUtility.FromJson<DataSettings>(data);
        }
        else
        {
            dataSettings = new DataSettings();
            SetDefaultValues();
        }
    }

    private void SetDefaultValues()
    {
        dataSettings.soundVolume = 1f;
        dataSettings.fullscreen = true;
        dataSettings.quality = 1;
        dataSettings.fps = 1;

        Resolution[] resolutions = Screen.resolutions;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                dataSettings.resolution = i;
                break;
            }
        }
    }

    private void SaveSettings()
    {
        string data = JsonUtility.ToJson(dataSettings);
        PlayerPrefs.SetString("SavedSettings", data);
    }

    private void SetUIelements()
    {
        //Sliders
        /*musicSlider.value = dataSettings.musicVolume;
        sfxSlider.value = dataSettings.sfxVolume;*/

        //Toggle
        fullscreenToggle.isOn = dataSettings.fullscreen;

        //DropdownFPS
        fpsDrop.value = dataSettings.fps;

        //DropdownResolution
        resolutionDrop.ClearOptions();
        Resolution[] resolOptions = Screen.resolutions;
        for (int i = 0; i < resolOptions.Length; i++)
        {
            string option = resolOptions[i].width.ToString() + " x " + resolOptions[i].height.ToString();
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(option);
            resolutionDrop.options.Add(optionData);
        }
        resolutionDrop.value = dataSettings.resolution;

        //DropdownQuality
        qualityDrop.ClearOptions();
        List<TMP_Dropdown.OptionData> optionsQuality = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < QualitySettings.names.Length; i++)
        {
            optionsQuality.Add(new TMP_Dropdown.OptionData(QualitySettings.names[i]));
        }
        qualityDrop.AddOptions(optionsQuality);
        qualityDrop.value = dataSettings.quality;
    }

    public void ApplyButton()
    {
        //Audio volume
        dataSettings.soundVolume = soundSlider.value;
        SoundManager.Instance.SetVolume(dataSettings.soundVolume);

        //Graphic settings
        //Fullscreen
        dataSettings.fullscreen = fullscreenToggle.isOn;
        Screen.fullScreen = dataSettings.fullscreen;
        //FPS
        dataSettings.fps = fpsDrop.value;
        switch (dataSettings.fps)
        {
            case 0:
                Application.targetFrameRate = 30;
                break;
            case 1:
                Application.targetFrameRate = 60;
                break;
            case 2:
                Application.targetFrameRate = 120;
                break;
            case 3:
                Application.targetFrameRate = 240;
                break;
            case 4:
                Application.targetFrameRate = -1;
                break;
        }
        //Quality
        dataSettings.quality = qualityDrop.value;
        QualitySettings.SetQualityLevel(dataSettings.quality);
        //Resolution
        dataSettings.resolution = resolutionDrop.value;
        Resolution resolution = Screen.resolutions[dataSettings.resolution];
        Screen.SetResolution(resolution.width, resolution.height, dataSettings.fullscreen);

        SaveSettings();
    }

    public void BackButton()
    {
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}

public class DataSettings
{
    public float soundVolume;
    public bool fullscreen;
    public int fps;
    public int quality;
    public int resolution;
}
