using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private float volume = 1;

    [SerializeField] private PlayerSoundClipsRefsSO playerSoundClipsRefsSO;

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

    private void Start()
    {
        SkillSystem.CastedSkill += CastedSkill;
        BaseSkill.SkillPerformed += BaseSkillOnSkillPerformed;
    }

    private void CastedSkill(object sender, SkillSystem.CastedSkillEventArgs e)
    {
        if (!e.skillSO.preCastSound) return;
        PlaySound(e.skillSO.preCastSound, e.playerCastingPosition);
    }

    private void BaseSkillOnSkillPerformed(object sender, BaseSkill.SkillPerformedEventArgs e)
    {
        if (!e.skillSO.skillSound) return;
        PlaySound(e.skillSO.skillSound, e.position);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }
}