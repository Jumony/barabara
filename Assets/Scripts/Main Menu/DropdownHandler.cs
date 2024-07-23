using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownHandler : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown refreshRateDropdown;
    public TMP_Dropdown fullscreenModeDropdown;

    private Dictionary<string, List<RefreshRate>> resolutionToRefreshRates = new Dictionary<string, List<RefreshRate>>();
    private RefreshRate selectedRefreshRate;
    private FullScreenMode fullscreenMode;

    private void Start()
    {
        resolutionDropdown.onValueChanged.AddListener(delegate { ResolutionValueChanged(resolutionDropdown); });
        refreshRateDropdown.onValueChanged.AddListener(delegate { RefreshRateChanged(refreshRateDropdown); });
        fullscreenModeDropdown.onValueChanged.AddListener(delegate { FullscreenModeChanged(fullscreenModeDropdown); });

        Screen.SetResolution(1920, 1080, true);
        InitializeDropdown();
    }

    private void InitializeDropdown()
    {
        resolutionDropdown.ClearOptions();
        refreshRateDropdown.ClearOptions();
        resolutionToRefreshRates.Clear();

        List<string> resolutionOptions = new List<string>();
        Resolution[] resolutions = Screen.resolutions;

        foreach (Resolution res in resolutions)
        {
            string key = $"{res.width} x {res.height}";
            if (!resolutionToRefreshRates.ContainsKey(key))
            {
                resolutionToRefreshRates[key] = new List<RefreshRate>();
            }

            if (!resolutionToRefreshRates[key].Contains(res.refreshRateRatio))
            {
                resolutionToRefreshRates[key].Add(res.refreshRateRatio);
            }
        }

        foreach (string key in resolutionToRefreshRates.Keys)
        {
            resolutionOptions.Add(key);
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = GetCurrentResolutionIndex();
        resolutionDropdown.RefreshShownValue();
        ResolutionValueChanged(resolutionDropdown); // Initialize the refresh rate dropdown
    }

    private int GetCurrentResolutionIndex()
    {
        string currentResolution = $"{Screen.currentResolution.width} x {Screen.currentResolution.height}";
        return resolutionDropdown.options.FindIndex(option => option.text == currentResolution);
    }

    private void ResolutionValueChanged(TMP_Dropdown dropdown)
    {
        Debug.Log("New Dropdown Value: " + dropdown.value);
        string selectedOption = dropdown.options[dropdown.value].text;

        // Gets the values of the 'resolution' key and puts it into a list of refresh rate objects
        if (resolutionToRefreshRates.TryGetValue(selectedOption, out List<RefreshRate> refreshRates))
        {
            refreshRateDropdown.ClearOptions();
            List<string> refreshRateOptions = new List<string>();

            foreach (RefreshRate rate in refreshRates)
            {
                refreshRateOptions.Add($"{rate.numerator / rate.denominator} hz");
            }

            refreshRateDropdown.AddOptions(refreshRateOptions);
            refreshRateDropdown.value = GetCurrentRefreshRateIndex(refreshRates);
            refreshRateDropdown.RefreshShownValue();
        }
    }

    private int GetCurrentRefreshRateIndex(List<RefreshRate> refreshRates)
    {
        string currentRefreshRate = $"{Screen.currentResolution.refreshRateRatio.numerator / Screen.currentResolution.refreshRateRatio.denominator} hz";
        return refreshRateDropdown.options.FindIndex(option => option.text == currentRefreshRate);
    }

    private void RefreshRateChanged(TMP_Dropdown dropdown)
    {
        string selectedRefreshRateStr = refreshRateDropdown.options[refreshRateDropdown.value].text.Replace(" hz", "");
        if (int.TryParse(selectedRefreshRateStr, out int selectedRefreshRateValue))
        {
            selectedRefreshRate = new RefreshRate { numerator = (uint)selectedRefreshRateValue, denominator = 1 };
            Debug.Log($"Selected refresh rate: {selectedRefreshRateValue} hz");
        }
        else
        {
            Debug.LogError("Failed to parse selected refresh rate");
        }
    }

    private void FullscreenModeChanged(TMP_Dropdown dropdown)
    {
        switch (dropdown.value)
        {
            case 0:
                fullscreenMode = FullScreenMode.ExclusiveFullScreen;
                break;

            case 1:
                fullscreenMode = FullScreenMode.FullScreenWindow;
                break;

            case 2: 
                fullscreenMode = FullScreenMode.Windowed;
                break;
        }
    }

    public void ApplyResolution()
    {
        string selectedResolution = resolutionDropdown.options[resolutionDropdown.value].text;
        string[] dimensions = selectedResolution.Split('x');
        if (dimensions.Length == 2 && int.TryParse(dimensions[0].Trim(), out int width) && int.TryParse(dimensions[1].Trim(), out int height))
        {
            Screen.SetResolution(width, height, fullscreenMode, selectedRefreshRate);
            Debug.Log($"Applied resolution: {width}x{height} @ {selectedRefreshRate.numerator / selectedRefreshRate.denominator} hz");
        }
        else
        {
            Debug.LogError("Failed to parse selected resolution");
        }
    }
}
