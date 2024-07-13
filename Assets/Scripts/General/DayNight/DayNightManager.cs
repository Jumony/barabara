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

    public Color dayColor;
    public Color nightColor;

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

    // Color.Lerp -- Should get to using that soon
    IEnumerator ColorTransition(Color startColor, Color endColor, float duration)
    {
        float t = 0;
        float lerpValueR;
        float lerpValueG;
        float lerpValueB;
        float lerpValueA;
        while (t < duration)
        {
            t += Time.deltaTime;
            lerpValueR = Mathf.Lerp(startColor.r, endColor.r, t / duration);
            lerpValueG = Mathf.Lerp(startColor.g, endColor.g, t / duration);
            lerpValueB = Mathf.Lerp(startColor.b, endColor.b, t / duration);
            lerpValueA = Mathf.Lerp(startColor.a, endColor.a, t / duration);

            Color lerpedColor = new Color(lerpValueR, lerpValueG, lerpValueB, lerpValueA);
            globalLight.color = lerpedColor;

            yield return null;
        }

        globalLight.color = endColor;
    }


    // Deprecated - We use ColorTransition instead
    IEnumerator IntensityTransition(float start, float end, float duration)
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
            yield return StartCoroutine(ColorTransition(dayColor, nightColor, transitionTime));
            currentTimeOfDay = TimeOfDay.Night;
            OnNightStart?.Invoke();
            yield return new WaitForSeconds(nighttimeLength);

            // Transition to daytime
            currentTimeOfDay = TimeOfDay.Day;
            yield return StartCoroutine(ColorTransition(nightColor, dayColor, transitionTime));
            yield return new WaitForSeconds(daytimeLength);

        }
    }
}
