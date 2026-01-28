using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    private float hp, mana;
    [SerializeField] private float maxHp, maxMana;

    private Inventory inventory;

    public event EventHandler PlayerHealed;

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

        inventory = GetComponent<Inventory>();
    }

    private void Start()
    {
        GameInput.Instance.SkillPerformed += OnSkillPerformed;
    }

    private void OnSkillPerformed(object sender, GameInput.SkillPerformedEventArgs e)
    {
        Debug.Log("Skill slot performed");
        Debug.Log(e.slotId);
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
    }

    private void ConsumeHealingPotion()
    {
        var hasPotion = inventory.PickupHealingPotion();
        if (hasPotion)
        {
            Heal(inventory.GetHealingPotionHealthAmount());
        }
    }

    private void Heal(float healAmount)
    {
        hp += Mathf.Min(hp + healAmount, maxHp);
        PlayerHealed?.Invoke(this, EventArgs.Empty);
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

    public Inventory GetInventory()
    {
        return inventory;
    }
}