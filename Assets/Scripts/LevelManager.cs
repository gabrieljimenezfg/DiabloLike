using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public Transform startingPosition;
    public bool bossDefeated;

    private void Awake()
    {
        Instance = this;
    }
}
