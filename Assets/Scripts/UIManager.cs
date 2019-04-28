using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject IntroPanel;
    [SerializeField]
    private GameObject InfoPanel;
    [SerializeField]
    private GameObject LostPanel;
    [SerializeField]
    private GameObject WinPanel;
    [SerializeField]
    private GameObject PartialWinPanel;
    [SerializeField]
    private GameObject QuitPanel;
    private bool needRestart = false;

    [SerializeField]
    private Text lostReasonText;

    // Start is called before the first frame update
    void Start()
    {
        IntroPanel.SetActive(true);
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (IntroPanel.activeSelf)
        {
            if (Input.GetKeyUp(KeyCode.H) || Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.Space))
            {
                InfoPanel.SetActive(true);
                IntroPanel.SetActive(false);
            }
        }
        else if (InfoPanel.activeSelf)
        {
            if (Input.GetKeyUp(KeyCode.H) || Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.Space))
            {
                InfoPanel.SetActive(false);
                IntroPanel.SetActive(false);
                Time.timeScale = 1;
            }
        }
        else if(needRestart)
        {
            if(Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
        }
        else if(Input.GetKeyUp(KeyCode.Escape))
        {
            Time.timeScale = 0;
            if(QuitPanel.activeSelf)
            {
                Application.Quit();
            }
            else
            {
                QuitPanel.SetActive(true);
            }
        }
        else if (QuitPanel.activeSelf)
        {
            if (Input.GetKeyUp(KeyCode.H) || Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.Space))
            {
                QuitPanel.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    public void Lost(string reason)
    {
        Time.timeScale = 0;
        lostReasonText.text = reason;
        LostPanel.SetActive(true);
        needRestart = true;
    }

    public void Win()
    {
        WinPanel.SetActive(true);
        needRestart = true;
    }

    public void PartialWin()
    {
        PartialWinPanel.SetActive(true);
        needRestart = true;
    }
}
