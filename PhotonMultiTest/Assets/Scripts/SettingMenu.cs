using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    //Resolution[] resolutions;

    //void Start()
    //{
    //    Screen.SetResolution(1920, 1080, Screen.fullScreen);

    //    resolutions = Screen.resolutions;

    //    List<string> options = new List<string>();

    //    for (int i = 0; i < resolutions.Length; i++)
    //    {
    //        string option = resolutions[i].width + " x " + resolutions[i].height;
    //        options.Add(option);
    //    }

    //}

    //public void SetResolution(int resolutionIndex)
    //{
    //    Resolution resolution = resolutions[resolutionIndex];
    //    Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    //}

    public void SetVolume(float volume)
    {
        //Debug.Log(volume);
        audioMixer.SetFloat("volume", volume);
    }
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
