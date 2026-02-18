using UnityEngine;
using System.Collections;

public class FirstBoss : Enemy
{
    private int atkPhase = 0;
    private bool isBase;

    [SerializeField] private GameObject FlashDisk;
    [SerializeField] private float timeInStandard;
    [SerializeField] private float preFlashAttack;
    [SerializeField] private float waitTime;
    [SerializeField] private float flashAttackScale;
    [SerializeField] private float flashAttackDuration;

    void Start()
    {
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
                StartCoroutine(StandardCoroutine());
                break;
            case 2:
                StartCoroutine(FlashAttackCoroutine());
                break;
        }
    }

    IEnumerator WaitingCorouting()
    { 
        yield return new WaitForSeconds(waitTime);
        atkPhase = Random.Range(1, 3);
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
        yield return new WaitForSeconds(preFlashAttack);
        Debug.Log("Flash Attack");
        GameObject DiskPref = Instantiate(FlashDisk, transform.position, Quaternion.Euler(90, 0, 0));
        while (DiskPref.transform.localScale.x < flashAttackScale)
        {
            DiskPref.transform.localScale = Vector3.Lerp(FlashDisk.transform.localScale, new Vector3(flashAttackScale, flashAttackScale, flashAttackScale), flashAttackDuration);
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(WaitingCorouting());
    }
}
