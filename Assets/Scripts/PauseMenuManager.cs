using System;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private GameObject winPanel;
    private bool gamePaused;

    private void Start()
    {
        GameInput.Instance.PauseGamePerformed += OnPauseGame;
        Player.Instance.PlayerDied += OnPlayerDied;
        Player.Instance.PlayerWin += OnPlayerWin;
    }

    private void OnDestroy()
    {
        GameInput.Instance.PauseGamePerformed -= OnPauseGame;
        Player.Instance.PlayerDied -= OnPlayerDied;
        Player.Instance.PlayerWin -= OnPlayerWin;
    }

    private void OnPauseGame(object sender, EventArgs e)
    {
        if(!gamePaused)
        {
            mainPanel.SetActive(true);
            Time.timeScale = 0f;
            gamePaused = true;
        }
        else
        {
            ResumeButton();
        }
    }

    public void ResumeButton()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    public void SettingsButton()
    {
        settingsPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void ExitButton()
    {
        SceneManagerScript.instance.CloseApplication();
    }

    public void MainMenuButton()
    {
        SceneManagerScript.instance.LoadMainMenu();
    }

    public void RetryButton()
    {
        SceneManagerScript.instance.LoadLevel1();
    }

    private void OnPlayerDied(object sender, EventArgs e)
    {
        deathPanel.SetActive(true);
    }
    
    private void OnPlayerWin(object sender, EventArgs e)
    {
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
