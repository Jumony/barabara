using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {

    }

    public void PlayGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetProgress();
            SceneManager.LoadScene("SampleScene");
        }

        if (GameManager.Instance == null)
        {
            Debug.LogError("Game Manager is null");
        }
    }
}
