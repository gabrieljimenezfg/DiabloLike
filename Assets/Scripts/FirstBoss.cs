using UnityEngine;
using System.Collections;

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
                IsMelee = true;
                StartCoroutine(StandardCoroutine());
                break;
            case 2:
                StartCoroutine(FlashAttackCoroutine());
                break;
            case 3:
                IsMelee = false;
                StartCoroutine(StandardCoroutine());
                break;
        }
    }

    IEnumerator WaitingCorouting()
    { 
        yield return new WaitForSeconds(waitTime);
        atkPhase = Random.Range(1, 4);
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
        GameObject DiskPref = Instantiate(FlashDisk, transform.position, transform.rotation);
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
