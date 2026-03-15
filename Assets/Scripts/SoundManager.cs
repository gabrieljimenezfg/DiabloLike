using System;
using System.Collections;
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
        PlaySound(e.skillSO.skillSound, e.position, 1f, 3f);
    }

    private void PlaySound(
        AudioClip audioClip,
        Vector3 position,
        float volumeMultiplier = 1f,
        float fadeOutAfterSeconds = -1f,
        float fadeDuration = 0.5f)
    {
        GameObject soundObject = new GameObject($"OneShotAudio_{audioClip.name}");
        soundObject.transform.position = position;

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = volumeMultiplier * volume;
        audioSource.spatialBlend = 1f;
        audioSource.Play();

        StartCoroutine(HandleSoundLifetime(audioSource, soundObject, fadeOutAfterSeconds, fadeDuration));
    }

    private IEnumerator HandleSoundLifetime(
        AudioSource audioSource,
        GameObject soundObject,
        float fadeOutAfterSeconds,
        float fadeDuration)
    {
        if (fadeOutAfterSeconds >= 0f)
        {
            yield return new WaitForSeconds(fadeOutAfterSeconds);

            float startVolume = audioSource.volume;
            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                if (audioSource == null) yield break;

                elapsed += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeDuration);
                yield return null;
            }

            audioSource.volume = 0f;
            audioSource.Stop();
        }
        else
        {
            yield return new WaitForSeconds(audioSource.clip.length);
        }

        Destroy(soundObject);
    }

    public void SetVolume(float _volume)
    {
        volume = _volume;
    }
}