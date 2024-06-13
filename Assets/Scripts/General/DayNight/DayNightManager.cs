using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightManager : MonoBehaviour
{
    public enum TimeOfDay
    {
        Day,
        Night
    }
    public TimeOfDay currentTimeOfDay;

    public Light2D globalLight;
    private EnemySpawner enemySpawner;

    private float lerpValue;
    public float transitionTime;
    public float daytimeLength;
    public float nighttimeLength;

    public static event Action OnNightStart;

    private void Start()
    {
        enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        currentTimeOfDay = TimeOfDay.Day;
        StartCoroutine(DayNightCycle());
    }

    IEnumerator Transition(float start, float end, float duration)
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            lerpValue = Mathf.Lerp(start, end, t / duration);
            globalLight.intensity = lerpValue;
            yield return null;
        }
        globalLight.intensity = end;
    }

    IEnumerator DayNightCycle()
    {
        while(true)
        {
            // Start with nighttime
            currentTimeOfDay = TimeOfDay.Day;
            yield return new WaitForSeconds(daytimeLength);

            // Transition to nighttime
            yield return StartCoroutine(Transition(1f, 0.3f, transitionTime));
            currentTimeOfDay = TimeOfDay.Night;
            OnNightStart?.Invoke();
            yield return new WaitForSeconds(nighttimeLength);

            // Transition to daytime
            currentTimeOfDay = TimeOfDay.Day;
            yield return StartCoroutine(Transition(0.3f, 1f, transitionTime));
            yield return new WaitForSeconds(daytimeLength);

        }
    }
}
