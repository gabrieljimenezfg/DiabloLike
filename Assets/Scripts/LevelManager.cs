using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public Transform startingPosition;

    private void Awake()
    {
        Instance = this;
    }
}
