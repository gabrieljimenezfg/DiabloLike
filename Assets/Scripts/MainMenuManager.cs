using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject settingsPanel;

    public void StartButton()
    {
        SceneManagerScript.instance.LoadLevel1();
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
}
