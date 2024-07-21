using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    private void Start()
    {
        dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(dropdown); });
        InitializeDropdown();
    }

    private void InitializeDropdown()
    {
        // Initialize the dropdown with available resolutions
        dropdown.ClearOptions();
        List<string> options = new List<string>();
        Resolution[] resolutions = Screen.resolutions;

        foreach (Resolution res in resolutions)
        {
            string option = res.width + " x " + res.height;
            if (!options.Contains(option))
            {
                options.Add(option);
            }
        }

        dropdown.AddOptions(options);
        // Set the dropdown to the current resolution
        dropdown.value = GetCurrentResolutionIndex();
        dropdown.RefreshShownValue();
    }

    private int GetCurrentResolutionIndex()
    {
        Resolution[] resolutions = Screen.resolutions;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                return i;
            }
        }
        return 0; // Default to the first resolution if no match is found
    }

    private void DropdownValueChanged(TMP_Dropdown dropdown)
    {
        Debug.Log("New Dropdown Value: " + dropdown.value);
        Resolution newResolution = Screen.resolutions[dropdown.value];
        Screen.SetResolution(newResolution.width, newResolution.height, FullScreenMode.Windowed);
    }

    /*
    private void UpdateRefreshRateDropdown()
    {
        currentResolutions = new List<Resolution>();

        string selectedResolution = resolutionDropdown.options[resolutionDropdown.value].text;
        string[] dimensions = selectedResolution.Split('x');
        int width = int.Parse(dimensions[0].Trim());
        int height = int.Parse(dimensions[1].Trim());

        foreach (Resolution res in resolutions)
        {
            if (res.width == width && res.height == height)
            {
                currentResolutions.Add(res);
            }
        }

        refreshRateDropdown.ClearOptions();
        List<string> refreshRateOptions = new List<string>();

        foreach (Resolution res in currentResolutions)
        {
            string option = res.refreshRateRatio + " hz";
            if (!refreshRateOptions.Contains(option))
            {
                refreshRateOptions.Add(option);
            }
        }

        refreshRateDropdown.AddOptions(refreshRateOptions);
        refreshRateDropdown.value = GetCurrentRefreshRateIndex();
        refreshRateDropdown.RefreshShownValue();
    }
    */

    /*
    private int GetCurrentRefreshRateIndex()
    {
        for (int i = 0; i < currentResolutions.Count; i++)
        {
            if (currentResolutions[i].refreshRateRatio.numerator == Screen.currentResolution.refreshRateRatio.numerator &&
                currentResolutions[i].refreshRateRatio.denominator == Screen.currentResolution.refreshRateRatio.denominator)
            {
                return i;
            }
        }

        return 0;
    }
    */



    /*
    private void RefreshRateChanged(TMP_Dropdown dropdown)
    {
        ApplyResolution();
    }

    private void ApplyResolution()
    {
        string selectedResolution = resolutionDropdown.options[resolutionDropdown.value].text;
        string[] dimensions = selectedResolution.Split('x');
        int width = int.Parse(dimensions[0].Trim());
        int height = int.Parse(dimensions[1].Trim());
        int refreshRate = int.Parse(refreshRateDropdown.options[refreshRateDropdown.value].text.Replace(" hz", "").Trim());

        RefreshRate refreshRateStruct = default;
        foreach (var res in currentResolutions)
        {
            if (res.refreshRateRatio.ToString().Equals(refreshRate + " hz"))
            {
                refreshRateStruct = res.refreshRateRatio;
                break;
            }
        }
        Screen.SetResolution(width, height, FullScreenMode.Windowed, refreshRateStruct);
        Debug.Log($"Selected Resolution: {selectedResolution}");
        Debug.Log($"Parsed Width: {width}, Height: {height}");
        Debug.Log($"Parsed Refresh Rate: {refreshRate}");

    }
    */
}
