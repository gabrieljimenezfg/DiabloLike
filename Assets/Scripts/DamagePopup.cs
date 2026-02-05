using System;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public static DamagePopup Create(Vector3 position, float damageAmount)
    {
        var yOffset = 3f;
        position += Vector3.up * yOffset;
        var damagePopupTransform = Instantiate(GameAssets.i.damagePopupPrefab, position, Quaternion.identity);        
        damagePopupTransform.LookAt(Camera.main.transform);
        var damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount);

        return damagePopup;
    }

    private static int sortingOrder;

    private const float DISAPPEAR_TIMER_MAX = 1f;
    
    private TextMeshPro damageText;
    [SerializeField] private float disappearTimer = 1f;
    [SerializeField] private float moveYSpeed = 5f;
    [SerializeField] private float yOffset = 5f;
    private Color textColor;

    private void Awake()
    {
        damageText = GetComponent<TextMeshPro>();
        textColor = damageText.color;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
        
        disappearTimer -= Time.deltaTime;
        if (disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
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
       disappearTimer = DISAPPEAR_TIMER_MAX;
       sortingOrder++;
       damageText.sortingOrder = sortingOrder;
    }
}
