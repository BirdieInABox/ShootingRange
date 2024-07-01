//Author: Kim Effie Proestler
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Menu : MonoBehaviour, IEventListener
{
    [SerializeField] //Popup menus
    private GameObject mainMenu,
        settingsMenu,
        bg;

    [SerializeField] //Settings sliders
    private Slider MVolumeSlider,
        BGMVolumeSlider,
        SFXVolumeSlider;

    [SerializeField] //Settings slider value texts
    private TMP_Text MVolumeText,
        BGMVolumeText,
        SFXVolumeText;

    [SerializeField] //Sound sources
    private AudioMixer audioMixer;

    public static bool gameIsPaused = false;

    //Called by Exit Game button
    public void Exit()
    {
        Application.Quit();
    }

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gameIsPaused = false;
        //Check if registry key already exists
        if (!PlayerPrefs.HasKey("MasterVolume"))
        {
            //If not, set standard values and create keys
            PlayerPrefs.SetFloat("MasterVolume", 0.5f);
            PlayerPrefs.SetFloat("BGMVolume", 0.5f);
            PlayerPrefs.SetFloat("SFXVolume", 0.5f);
            PlayerPrefs.SetInt("Highscore", 0);
            PlayerPrefs.Save();
        }

        //Get saved preference for master volume and set slider and value text to it
        MVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(MVolumeSlider.value) * 20);
        MVolumeText.SetText((int)(MVolumeSlider.value * 100) + "%");

        //Get saved preference for music volume and set slider and value text to it
        BGMVolumeSlider.value = PlayerPrefs.GetFloat("BGMVolume");
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(BGMVolumeSlider.value) * 20);
        BGMVolumeText.SetText((int)(BGMVolumeSlider.value * 100) + "%");

        //Get saved preference for effects volume and set slider and value text to it
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(SFXVolumeSlider.value) * 20);
        SFXVolumeText.SetText((int)(SFXVolumeSlider.value * 100) + "%");
    }

    void Start()
    {
        //Add this as listener to event system
        EventManager.MainStatic.AddListener(this);
    }

    //Called when value of MVolumeSlider changes
    public void ChangeMasterVolume()
    {
        //Set AudioListener's volume to value
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(MVolumeSlider.value) * 20);
        //Update value text
        MVolumeText.SetText((int)(MVolumeSlider.value * 100) + "%");
        //Save preference
        PlayerPrefs.SetFloat("MasterVolume", MVolumeSlider.value);
        PlayerPrefs.Save();
    }

    //Called when value of BGMVolume changes
    public void ChangeBGMVolume()
    {
        //Set Audiosource's volume to value
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(BGMVolumeSlider.value) * 20);
        //Update value text
        BGMVolumeText.SetText((int)(BGMVolumeSlider.value * 100) + "%");
        //Save preference
        PlayerPrefs.SetFloat("BGMVolume", BGMVolumeSlider.value);
        PlayerPrefs.Save();
    }

    //Called when value of SFXVolume changes
    public void ChangeSFXVolume()
    {
        //Set Audiosource's volume to value
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(SFXVolumeSlider.value) * 20);
        //Update value text
        SFXVolumeText.SetText((int)(SFXVolumeSlider.value * 100) + "%");
        //Save preference
        PlayerPrefs.SetFloat("SFXVolume", SFXVolumeSlider.value);
        PlayerPrefs.Save();
    }

    //Called by Escape key
    public void OnToggle(InputValue value)
    {
        //If Settings are shown
        if (settingsMenu.activeSelf)
        {
            gameIsPaused = !gameIsPaused;
            //Hide all menus
            settingsMenu.SetActive(false);
            bg.SetActive(false);
            mainMenu.SetActive(false);
            ToggleCursor();
        }
        else //If Settings aren't shown
        {
            ToggleMenu();
        }
        Time.timeScale = (gameIsPaused ? 0 : 1);
    }

    //Checks if either menu or settings are open
    public bool IsMenuOpen()
    {
        return mainMenu.activeSelf || settingsMenu.activeSelf;
    }

    //Called by Resume button
    public void Resume()
    {
        EventManager.MainStatic.FireEvent(new EventData(EventTypes.GamePaused));
    }

    // Turns menu on/off
    public void ToggleMenu()
    {
        gameIsPaused = !gameIsPaused;
        mainMenu.SetActive(!mainMenu.activeSelf);
        bg.SetActive(!bg.activeSelf);
        ToggleCursor();
    }

    //Hides/Shows cursor and locks/unlocks it
    private void ToggleCursor()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    //Called by Settings button
    public void ToggleSettings()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
        settingsMenu.SetActive(!settingsMenu.activeSelf);
    }

    /// <summary>
    /// Called upon EventSystem sending an event
    /// </summary>
    /// <param name="receivedEvent">the received event, including type and content</param>
    public void OnEventReceived(EventData receivedEvent)
    {
        //if event received is of type "GamePaused"
        if (receivedEvent.Type == EventTypes.GamePaused)
        {
            //pause/unpause game
            OnToggle(null);
        }
    }
}
