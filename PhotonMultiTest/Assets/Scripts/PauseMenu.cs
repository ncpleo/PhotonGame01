using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject OptionPanel;

    private bool isPaused = false;


    void Update()
    {
        CheckPause();
    }

    public void Pause()
    {
        PausePanel.SetActive(true);
        isPaused = true;
    }

    public void Continue()
    {
        PausePanel.SetActive(false);
    }

    public void Option()
    {
        PausePanel.SetActive(false);
        OptionPanel.SetActive(true);
    }

    public void Back()
    {
        OptionPanel.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    void CheckPause()
    {
        //if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false)
        //{
        //    PausePanel.SetActive(true);
        //    isPaused = true;
        //}

        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == true)
        {
            PausePanel.SetActive(false);
            isPaused = false;
        }
    }
}
