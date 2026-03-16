using UnityEngine;
using UnityEngine.AI;
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
        Time.timeScale = 1.0f;
    }

    public void LoadLevel1()
    {
        SceneManager.sceneLoaded += OnLevel1Loaded;
        SceneManager.LoadScene(level1ID);
        Time.timeScale = 1.0f;
    }

    private void OnLevel1Loaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnLevel1Loaded;

        Player.Instance.GetComponent<NavMeshAgent>().Warp(LevelManager.Instance.startingPosition.position);
        Player.Instance.ResetPlayer();
        GameInput.Instance.EnableActions();
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene(level2ID);
        SceneManager.sceneLoaded += OnLevel2Loaded;
        Time.timeScale = 1.0f;
    }

    private void OnLevel2Loaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnLevel2Loaded;

        Player.Instance.GetComponent<NavMeshAgent>().Warp(LevelManager.Instance.startingPosition.position);
        GameInput.Instance.EnableActions();
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene(level3ID);
        SceneManager.sceneLoaded += OnLevel3Loaded;
        Time.timeScale = 1.0f;
    }

    private void OnLevel3Loaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnLevel3Loaded;

        Player.Instance.GetComponent<NavMeshAgent>().Warp(LevelManager.Instance.startingPosition.position);
        GameInput.Instance.EnableActions();
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
