using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    private int mainMenuID = 0;
    private int level1ID = 1;
    private int level2ID = 2;
    private int level3ID = 3;
    public static SceneManagerScript instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuID);
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene(level1ID);
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene(level2ID);
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene(level3ID);
    }

    public void CloseApplication()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif  
    }
}
