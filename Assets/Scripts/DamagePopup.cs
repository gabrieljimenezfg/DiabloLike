using System;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public static DamagePopup Create(Vector3 position, float damageAmount)
    {
        var damagePopupTransform = Instantiate(GameAssets.i.damagePopupPrefab, position, Quaternion.identity);        
        damagePopupTransform.rotation = Camera.main.transform.rotation;
        var damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount);

        return damagePopup;
    }

    private static int sortingOrder;

    
    private TextMeshPro damageText;
    [SerializeField] private float disappearTimerMax = 1f;
    private float disappearTimer;
    [SerializeField] private float moveYSpeed = 5f;
    private Color textColor;

    private void Awake()
    {
        damageText = GetComponent<TextMeshPro>();
        textColor = damageText.color;
    }

    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
        transform.position += transform.up * (moveYSpeed * Time.deltaTime);
        
        disappearTimer -= Time.deltaTime;
        if (disappearTimer > disappearTimerMax * .5f)
        {
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * (increaseScaleAmount * Time.deltaTime);
        }
        else
        {
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * (decreaseScaleAmount * Time.deltaTime);
            
        }
        if (disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            damageText.color = textColor;

            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Setup(float damageAmount)
    {
       damageText.SetText(damageAmount.ToString());
       disappearTimer = disappearTimerMax;
       sortingOrder++;
       damageText.sortingOrder = sortingOrder;
    }
}
