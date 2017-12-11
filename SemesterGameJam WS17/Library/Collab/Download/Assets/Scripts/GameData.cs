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

    public float difficulty = 1;

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

    #region Score
    private int score = 0;
    public int highscore;

    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
        }
    }
    #endregion

    public bool Invincible = false;
    public bool IsAlive = true;

    // orientation values between 0 and 3
    #region Orientation
    private int orientation = 0;

    public int Orientation
    {
        get
        {
            return orientation;
        }
        set
        {
            orientation = value;
        }
    }
    #endregion

    #region Settings

    public int inputType = 1;

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
        FileStream file = File.Create(Application.persistentDataPath + "/gameSettings.dat");

        SaveData_Settings data = new SaveData_Settings();
        data.music_volume = Music_Volume;
        data.sfx_volume = SFX_Volume;

        bF.Serialize(file, data);
        file.Close();
    }

    public void Save_Highscore()
    {
        BinaryFormatter bF = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameScore.dat");

        SaveData_Highscore data = new SaveData_Highscore();
        data.highscore = highscore;

        bF.Serialize(file, data);
        file.Close();
    }

    public void LoadSettings()
    {
        if (File.Exists(Application.persistentDataPath + "/gameSettings.dat"))
        {
            BinaryFormatter bF = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameSettings.dat", FileMode.Open);
            SaveData_Settings data = (SaveData_Settings)bF.Deserialize(file);
            file.Close();

            Music_Volume = data.music_volume;
            SFX_Volume = data.sfx_volume;
            inputType = data.inputType;
        }
    }

    public void LoadHighscore()
    {
        if (File.Exists(Application.persistentDataPath + "/gameScore.dat"))
        {
            BinaryFormatter bF = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameScore.dat", FileMode.Open);
            SaveData_Highscore data = (SaveData_Highscore)bF.Deserialize(file);
            file.Close();

            highscore = data.highscore;
        }
    }

    private int laneWidth = 2;

    public Vector2 LaneAttraction(int lane, int orientation)
    {
        Vector2 attractionPoint = Vector2.zero;
        switch (orientation)
        {
            case 0:
                switch (lane)
                {
                    case -1:
                        attractionPoint = new Vector2(-laneWidth, -4);
                        break;

                    case 0:
                        attractionPoint = new Vector2(0, -4);
                        break;

                    case 1:
                        attractionPoint = new Vector2(laneWidth, -4);
                        break;

                    default:
                        break;
                }
                break;

            case 1:
                switch (lane)
                {
                    case -1:
                        attractionPoint = new Vector2(4, -laneWidth);
                        break;

                    case 0:
                        attractionPoint = new Vector2(4, 0);
                        break;

                    case 1:
                        attractionPoint = new Vector2(4, laneWidth);
                        break;

                    default:
                        break;
                }
                break;

            case 2:
                switch (lane)
                {
                    case -1:
                        attractionPoint = new Vector2(-laneWidth, 4);
                        break;

                    case 0:
                        attractionPoint = new Vector2(0, 4);
                        break;

                    case 1:
                        attractionPoint = new Vector2(laneWidth, 4);
                        break;

                    default:
                        break;
                }
                break;

            case 3:
                switch (lane)
                {
                    case -1:
                        attractionPoint = new Vector2(-4, -laneWidth);
                        break;

                    case 0:
                        attractionPoint = new Vector2(-4, 0);
                        break;

                    case 1:
                        attractionPoint = new Vector2(-4, laneWidth);
                        break;

                    default:
                        break;
                }
                break;

            default:
                break;
        }

        return attractionPoint;
    }
}

[Serializable]
class SaveData_Settings
{
    public float music_volume;
    public float sfx_volume;
    public int inputType;
}

[Serializable]
class SaveData_Highscore
{
    public int highscore;
}