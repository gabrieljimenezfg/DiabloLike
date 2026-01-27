using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    private float hp, mana;
    [SerializeField] private float maxHp, maxMana;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        TestSaveLoad();
    }

    private void TestSaveLoad()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SaveSystem.Save();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SaveSystem.Load();
        }
    }

    public void Save(ref PlayerState playerState)
    {
        playerState.hp = hp;
        playerState.maxHp = maxHp;
        playerState.mana = mana;
        playerState.maxMana = maxMana;
    }

    public void Load(PlayerState playerState)
    {
        hp = playerState.hp;
        maxHp = playerState.maxHp;
        mana = playerState.mana;
        maxMana = playerState.maxMana;
    }
}