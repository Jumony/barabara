using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public float pauseSpeed = 0.2f;
    public bool isPaused = false;
    private Coroutine pauseCoroutine;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            StartPause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            UnpauseGame();
        }
    }

    private void StartPause()
    {
        if (pauseCoroutine != null)
        {
            StopCoroutine(pauseCoroutine);
        }
        pauseCoroutine = StartCoroutine(LerpTimeScale(1, 0, pauseSpeed));
        isPaused = true;
    }

    private IEnumerator LerpTimeScale(float start, float end, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            Time.timeScale = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }

        Time.timeScale = end;
    }

    private void UnpauseGame()
    {
        if (pauseCoroutine != null)
        {
            StopCoroutine(pauseCoroutine);
        }
        Time.timeScale = 1f;
        isPaused = false;
    }
}
