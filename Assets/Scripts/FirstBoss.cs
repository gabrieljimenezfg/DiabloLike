using UnityEngine;
using System.Collections;
using System;

public class FirstBoss : Enemy
{
    //Se llama first boss pero se puede usar para todos los bosses
    private int atkPhase = 0;
    private bool isBase;

    [SerializeField] private GameObject FlashDisk;
    [SerializeField] private float timeInStandard;
    [SerializeField] private float preFlashAttack;
    [SerializeField] private float waitTime;
    [SerializeField] private float flashAttackScale;
    [SerializeField] private float flashAttackDuration;
    [SerializeField] private float shotRange;
    [SerializeField] private float shotCooldown;
    [SerializeField] private float shotSpeed;
    [SerializeField] private float shotAttack;
    [SerializeField] private Transform slashSpawn;
    
    private float meleeAttack;
    private float meleeRange;
    private float meleeCooldown;
    private float meleeSpeed;

    public event EventHandler<int> ChangeAttackPhase;
    public event EventHandler StartWaiting;

    void Start()
    {
        meleeCooldown = attackCooldown;
        meleeRange = attackRange;
        meleeSpeed = speed;
        meleeAttack = damage;
        base.Start();
        StartCoroutine(WaitingCorouting());
    }

    void Update()
    {
        if (isBase)
        {
            base.Update();
        }
    }

    void ChangeState()
    {
        switch (atkPhase)
        {
            case 1:
                speed = meleeSpeed;
                attackCooldown = meleeCooldown;
                attackRange = meleeRange;
                damage = meleeAttack;
                IsMelee = true;
                StartCoroutine(StandardCoroutine());
                break;
            case 2:
                StartCoroutine(FlashAttackCoroutine());
                break;
            case 3:
                Debug.Log("RANGED ATTACK");
                damage = shotAttack;
                speed = shotSpeed;
                attackCooldown = shotCooldown;
                attackRange = shotRange;
                IsMelee = false;
                StartCoroutine(StandardCoroutine());
                break;
        }
    }

    IEnumerator WaitingCorouting()
    {
        StartWaiting?.Invoke(this, EventArgs.Empty);
        yield return new WaitForSeconds(waitTime);
        atkPhase = UnityEngine.Random.Range(1, 4);
        ChangeAttackPhase?.Invoke(this, atkPhase);
        ChangeState();
    }

    IEnumerator StandardCoroutine()
    {
        isBase = true;
        yield return new WaitForSeconds(timeInStandard);
        isBase = false;
        StartCoroutine(WaitingCorouting());
    }

    IEnumerator FlashAttackCoroutine()
    {
        //FlashAttackPerformed?.Invoke(this, EventArgs.Empty);
        yield return new WaitForSeconds(preFlashAttack);
        Debug.Log("Flash Attack");
        GameObject DiskPref = Instantiate(FlashDisk, slashSpawn.position, slashSpawn.rotation);
        float timer = 0f;
        Vector3 originalScale = DiskPref.transform.localScale;
        while (timer < flashAttackDuration)
        {
            timer += Time.deltaTime;
            DiskPref.transform.localScale = Vector3.Lerp(originalScale, new Vector3(flashAttackScale, flashAttackScale, flashAttackScale), timer / flashAttackDuration);
            yield return null;
        }
        Destroy(DiskPref);
        StartCoroutine(WaitingCorouting());
    }
}
