using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameData
{
    #region Instanziierung
    private static GameData instance;

    private GameData()
    {
        if (instance != null)
            return;
        instance = this;
    }

    public static GameData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameData();
            }

            return instance;
        }
    }
    #endregion

    // bool to display the current status of the game (is paused or not)
    #region Pause
    private bool is_Paused = false;

    public bool Is_Paused
    {

        get
        {
            return is_Paused;
        }

        set
        {
            is_Paused = value;
        }

    }
    #endregion

    #region Settings
    // Volume of SFX and music
    #region Sound
    private float music_Volume = 0;
    public float Music_Volume
    {
        get
        {
            return music_Volume;
        }
        set
        {
            music_Volume = value;
        }
    }

    private float sfx_Volume = 0;
    public float SFX_Volume
    {
        get
        {
            return sfx_Volume;
        }
        set
        {
            sfx_Volume = value;
        }
    }
    #endregion
    #endregion

    public void Save_Settings()
    {
        BinaryFormatter bF = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameData.dat");

        SaveData_Settings data = new SaveData_Settings();
        data.music_volume = Music_Volume;
        data.sfx_volume = SFX_Volume;

        bF.Serialize(file, data);
        file.Close();
    }

    public void Load(String dataToLoad)
    {
        if (File.Exists(Application.persistentDataPath + "/gameData.dat"))
        {
            BinaryFormatter bF = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.Open);
            SaveData_Settings data = (SaveData_Settings)bF.Deserialize(file);
            file.Close();

            if(dataToLoad == "settings")
            {
                Music_Volume = data.music_volume;
                SFX_Volume = data.sfx_volume;
            }
        }
    }
}

[Serializable]
class SaveData_Settings
{
    public float music_volume;
    public float sfx_volume;
}