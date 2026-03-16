using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;
using TMPro;

public class Player : MonoBehaviour, IDamageable
{
    public static Player Instance;
    [SerializeField] private Projectile projectile;
    [SerializeField] private float hp = 100f, mana = 100f;
    [SerializeField] private float maxHp, maxMana;
    [SerializeField] private Transform castSpawnPoint;
    [SerializeField] private float lockedTurnSpeed;
    [SerializeField] private GameObject staffLight;
    [SerializeField] private GameObject staffModel;
    [SerializeField] private GameObject popUpCanvas;
    private bool hasStaff;
    public bool playerWin;
    public bool invincible = false;
    private bool arePositionAndRotationLocked;
    public List<int> keyList = new List<int>();

    public float HP => hp;
    public float Mana => mana;
    public float MaxHP => maxHp;
    public float MaxMana => maxMana;
    public Transform CastSpawnPoint => castSpawnPoint;
    public bool ArePositionAndRotationLocked => arePositionAndRotationLocked;
    public bool HasStaff => hasStaff;

    private Inventory inventory;
    private SkillSystem skillSystem;

    public event EventHandler PlayerHPChanged;
    public event EventHandler PlayerManaChanged;
    public event EventHandler PlayerTookDamage;
    public event EventHandler PlayerDied;
    public event EventHandler PlayerReset;
    public event EventHandler PlayerWin;

    private void Awake()
    {
        //Instance = this;
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
        skillSystem = GetComponent<SkillSystem>();
    }

    private void Start()
    {
        GameInput.Instance.SkillPerformed += OnSkillPerformed;
        GameInput.Instance.HealPotionUsed += OnHealPotionUsed;
        GameInput.Instance.ManaPotionUsed += OnManaPotionUsed;
    }

    private void OnDestroy()
    {
        GameInput.Instance.SkillPerformed -= OnSkillPerformed;
        GameInput.Instance.HealPotionUsed -= OnHealPotionUsed;
        GameInput.Instance.ManaPotionUsed -= OnManaPotionUsed;
    }

    private void LateUpdate()
    {
        if (playerWin)
        {
            PlayerWin?.Invoke(this, EventArgs.Empty);
            playerWin = false;
        }
    }

    private Vector3 GetDirectionToMouse()
    {
        var mousePosition = MouseWorldUtils.GetMouseWorldPositionOnPlane(transform.position);
        Vector3 direction = (mousePosition - transform.position).normalized;
        direction.y = 0f;
        return direction;
    }
    
    public void SlowlyTurnTowardsMouse()
    {
        var direction = GetDirectionToMouse();
        var targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, lockedTurnSpeed * Time.deltaTime);
    }

    private void LookTowardsMouse()
    {
        var direction = GetDirectionToMouse();
        SetLookDirection(direction);
    }
    
    public void SetLookDirection(Vector3 direction)
    {
        transform.forward = direction;
    }

    public void TogglePositionRotationLock(bool isLocked)
    {
        arePositionAndRotationLocked = isLocked;
        if (isLocked)
        {
            LookTowardsMouse();
        }
    }
    
    private void OnHealPotionUsed(object sender, EventArgs e)
    {
        ConsumeHealingPotion();
    }

    private void OnManaPotionUsed(object sender, EventArgs e)
    {
        ConsumeManaPotion();
    }

    private void OnSkillPerformed(object sender, GameInput.SkillPerformedEventArgs e)
    {
        skillSystem.CastSkill(e.slotId);
    }


    private void ConsumeHealingPotion()
    {
        var hasPotion = inventory.TryConsumeHealingPotion();
        if (hasPotion)
        {
            var healAmount = inventory.GetHealingPotionHealthAmount();
            Heal(healAmount);
        }
    }

    private void ConsumeManaPotion()
    {
        var hasPotion = inventory.TryConsumeManaPotion();
        if (hasPotion)
        {
            var manaRecoverAmount = inventory.GetManaPotionRecoverAmount();
            RecoverMana(manaRecoverAmount);
        }
    }

    public void TakeDamage(float amount)
    {
        if (!invincible)
        {
            ReduceHP(amount);
            DamagePopup.Create(transform.position, amount);
            if (hp <= 0)
            {
                PlayerHPChanged?.Invoke(this, EventArgs.Empty);
                PlayerDied?.Invoke(this, EventArgs.Empty);
                GameInput.Instance.DisableActions();
            }
            else
            {
                PlayerHPChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        else {
            Debug.Log("(!!!)");
        }
    }

    private void ReduceHP(float amount)
    {
        hp = Mathf.Max(0, hp - amount);
    }

    private void Heal(float healAmount)
    {
        if (hp == maxHp) return;
        hp = Mathf.Min(hp + healAmount, maxHp);
        PlayerHPChanged?.Invoke(this, EventArgs.Empty);
    }

    private void RecoverMana(float recoverAmount)
    {
        if (mana == maxMana) return;
        mana = Mathf.Min(mana + recoverAmount, maxMana);
        PlayerManaChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UseMana(float manaUsageAmount)
    {
        mana = Mathf.Max(mana - manaUsageAmount, 0);
        PlayerManaChanged?.Invoke(this, EventArgs.Empty);
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

    public SkillSystem GetSkillSystem()
    {
        return skillSystem;
    }

    public void AddKey(int _key)
    {
        keyList.Add(_key);
    }

    public void ShowMessage(string _message)
    {
        StartCoroutine(MessageCoroutine(_message));
    }

    public void HideMessage()
    {
        popUpCanvas.SetActive(false);
    }

    IEnumerator MessageCoroutine(string message)
    {
        popUpCanvas.SetActive(true);
        popUpCanvas.GetComponentInChildren<TextMeshProUGUI>().text = message;
        yield return new WaitForSeconds(2);
        popUpCanvas.SetActive(false);
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Key")
        {
            AddKey(other.gameObject.GetComponent<KeyScript>().keyIndex);
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Staff")
        {
            gameObject.GetComponent<SkillSystem>().AcquireNewSkill(1);
            staffLight.SetActive(true);
            staffModel.SetActive(true);
            hasStaff = true;
            projectile.damage = projectile.maxDamage;
            Destroy(other.gameObject);
        }
    }

    public void ResetPlayer()
    {
        hp = maxHp;
        mana = maxMana;
        GetComponent<Inventory>().ResetPotions();
        hasStaff = false;
        staffLight.SetActive(false);
        staffModel.SetActive(false);
        skillSystem.ClearSkills();
        skillSystem.AcquireNewSkill(0);
        PlayerHPChanged?.Invoke(this, EventArgs.Empty);
        PlayerManaChanged?.Invoke(this, EventArgs.Empty);
        PlayerReset?.Invoke(this, EventArgs.Empty);
        keyList.Clear();
        projectile.damage = 16f;
    }
}