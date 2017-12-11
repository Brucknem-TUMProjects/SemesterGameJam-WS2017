using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    // Awake() function is in here!!!
    #region singleton-like pattern   
    public static Main_Menu instance;

    void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(transform.gameObject);
            instance = this;
            GameData.Instance.LoadHighscore();
            Load_Settings();
            //music.value = GameData.Instance.Music_Volume;
            //sfx.value = GameData.Instance.SFX_Volume;
            SetMusicLevel(GameData.Instance.Music_Volume);
            SetSfxLevel(GameData.Instance.SFX_Volume);
            dropdown.onValueChanged.AddListener(delegate { GameData.Instance.inputType = dropdown.value; });
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        music_Manager = GetComponent<Music_Manager>();
    }
    #endregion

    // global Variables; also includes functions to show and hide panels
    #region Vars
    private bool in_Main_Menu = true;
    private Music_Manager music_Manager;

    public Animator fadeAnim;
    public AnimationClip fade;

    public AudioMixer main_Mixer;
    public int scene_To_Load;

    public Text score_Text;
    public Dropdown dropdown;
    //public Slider music;
    //public Slider sfx;

    #region Panels
    public GameObject main_Panel;
    public GameObject pause_Panel;
    public GameObject death_Panel;
    public GameObject settings_Panel;

    #region Panelmanagement
    public void ShowMainPanel()
    {
        main_Panel.SetActive(true);
    }
    public void HideMainPanel()
    {
        main_Panel.SetActive(false);
    }

    public void ShowPausePanel()
    {
        pause_Panel.SetActive(true);
    }
    public void HidePausePanel()
    {
        pause_Panel.SetActive(false);
    }

    public void ShowDeathPanel()
    {
        death_Panel.SetActive(true);
    }
    public void HideDeathPanel()
    {
        death_Panel.SetActive(false);
    }

    public void ShowSettingsPanel()
    {
        settings_Panel.SetActive(true);
    }
    public void HideSettingsPanel()
    {
        if(settings_Panel.activeSelf)
            settings_Panel.SetActive(false);
    }
    #endregion
    #endregion
    #endregion

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !in_Main_Menu && !GameData.Instance.Is_Paused && death_Panel.activeSelf != true)
        {
            Pause_Game();
        }
        else if(Input.GetButtonDown("Cancel") && !in_Main_Menu && GameData.Instance.Is_Paused && death_Panel.activeSelf != true)
        {
            Unpause_Game();
        }

        if (!GameData.Instance.IsAlive)
        {
            On_Death();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            GameData.Instance.Invincible = !GameData.Instance.Invincible;
            print("invincible " + GameData.Instance.Invincible);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += SceneWasLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneWasLoaded;
    }

    void SceneWasLoaded(Scene scene, LoadSceneMode mode)
    {
        music_Manager.PlayLevelMusic();
    }

    // functions sole to the main menu
    #region  Main Menu
    // Start the game
    public void Start_Game()
    {
        Cursor.visible = false;
        music_Manager.FadeDown(fade.length);
        Invoke("LoadDelayed", fade.length * .5f);
        fadeAnim.SetTrigger("FadeScene");
        in_Main_Menu = false;
    }

    void LoadDelayed()
    {
        HideMainPanel();
        SceneManager.LoadScene(scene_To_Load);
    }

    // End the game / return to the desktop
    public void End_Game()
    {   
            //If we are running in a standalone build of the game
        #if UNITY_STANDALONE
            //Quit the application
            Application.Quit();
        #endif

            //If we are running in the editor
        #if UNITY_EDITOR
            //Stop playing the scene
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    #endregion

    // functions sole to the pause menu
    #region Pause Menu
    public void Pause_Game()
    {
        GameData.Instance.Is_Paused = true;

        Cursor.visible = true;
        ShowPausePanel();
        Time.timeScale = 0;
    }
    public void Unpause_Game()
    {
        GameData.Instance.Is_Paused = false;

        Cursor.visible = false;
        HidePausePanel();
        Time.timeScale = 1;
    }
    
    public void Return_toMenu()
    {
        Time.timeScale = 1;
        GameData.Instance.Is_Paused = false;

        music_Manager.FadeDown(fade.length);
        Invoke("BackToMenuDelayed", fade.length * .5f);
        fadeAnim.SetTrigger("FadeScene");
        in_Main_Menu = true;
    }
    void BackToMenuDelayed()
    {
        if(pause_Panel.activeSelf == true)
            HidePausePanel();
        if (death_Panel.activeSelf == true)
            HideDeathPanel();
        SceneManager.LoadScene(0);
        ShowMainPanel();
    }
    #endregion

    //
    #region Death Screen
    public void On_Death()
    {
        int temp = GameData.Instance.highscore;
        if(GameData.Instance.highscore < GameData.Instance.Score)
        {
            GameData.Instance.highscore = GameData.Instance.Score;
            GameData.Instance.Save_Highscore();
        }

        GameData.Instance.IsAlive = true;
        GameData.Instance.Is_Paused = true;
        score_Text.text = "your score: " + GameData.Instance.Score + " /nlast highscore: " + temp;
        Cursor.visible = true;
        ShowDeathPanel();
        Time.timeScale = 0;
    }
    #endregion

    // functions to modify game settings like audio etc.
    #region Settings
    // Call this function and pass in the float parameter musicLvl to set the volume of the AudioMixerGroup Music in mainMixer
    public void SetMusicLevel(float musicLvl)
    {
        main_Mixer.SetFloat("musicVol", musicLvl);
        GameData.Instance.Music_Volume = musicLvl;
    }

    // Call this function and pass in the float parameter sfxLevel to set the volume of the AudioMixerGroup SoundFx in mainMixer
    public void SetSfxLevel(float sfxLevel)
    {
        main_Mixer.SetFloat("sfxVol", sfxLevel);
        GameData.Instance.SFX_Volume = sfxLevel;
    }

    public void Open_Settings()
    {
        if (in_Main_Menu)
        {
            HideMainPanel();
            ShowSettingsPanel();
        }
        else if(!in_Main_Menu)
        {
            HidePausePanel();
            ShowSettingsPanel();
        }
    }

    public void Close_Settings()
    {
        if (in_Main_Menu)
        {
            HideSettingsPanel();
            ShowMainPanel();
        }
        else if (!in_Main_Menu)
        {
            HideSettingsPanel();
            ShowPausePanel();
        }

        GameData.Instance.Save_Settings();
    }

    //save settings persistent via GameData
    public void Save_Settings()
    {
        GameData.Instance.Save_Settings();
    }

    public void Load_Settings()
    {
        GameData.Instance.LoadSettings();
    }

    #endregion
}
