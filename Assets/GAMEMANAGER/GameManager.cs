using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerProgression playerProgression;

    private void Awake()
    {
        LoadProgress();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            playerProgression = new PlayerProgression();
            Debug.Log("GameManager initialized");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Duplicate GameManager destroyed");
        }
    }

    public void ResetProgress()
    {
        SaveLoad.ResetSave();
    }

    public void SaveProgress()
    {
        SaveLoad.Save(playerProgression);
    }    

    public void LoadProgress()
    {
        SaveLoad.Load();
    }

    public void GameOver()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
