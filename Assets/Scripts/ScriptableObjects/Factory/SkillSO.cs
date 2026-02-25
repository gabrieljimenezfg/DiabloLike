using UnityEngine;

[CreateAssetMenu]
public class SkillSO : ScriptableObject
{
    public string skillName;
    public string description;
    public Sprite icon;
    public float manaCost;
    public float cooldown;
    public GameObject skillPrefab;
    public AudioClip skillSound;
    public AudioClip preCastSound;
}