using UnityEngine;

public enum PotionType
{
    Health,
    Mana,
}

[CreateAssetMenu]
public class PotionSO : ScriptableObject
{
    public PotionType potionType;
    public float recoverAmount;
    public GameObject prefab;
    public Sprite sprite;
}