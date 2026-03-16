using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource musicSource;
    private AudioSource ambientSource;
    [SerializeField]
    private GameObject SFX;
    private float musicVolume;
    private float sfxVolume;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        musicSource = gameObject.AddComponent<AudioSource>();
        ambientSource = gameObject.AddComponent<AudioSource>();
        sfxVolume = 1f;
    }
    public void PlayMusic(AudioClip _music, float _volume = 1)
    {
        musicSource.clip = _music;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayAmbient(AudioClip _ambient, float _volume = 1)
    {
        ambientSource.clip = _ambient;
        ambientSource.volume = sfxVolume;
        ambientSource.Play();
    }
    public void StopAmbient()
    {
        ambientSource.Stop();
    }

    public void PlaySFX(AudioClip _sfx, Vector3 _position, float _volume = 1)
    {
        GameObject SFXClone = Instantiate(SFX, _position, Quaternion.identity);
        SFXClone.GetComponent<AudioSource>().clip = _sfx;
        SFXClone.GetComponent<AudioSource>().volume = sfxVolume;
        SFXClone.GetComponent<AudioSource>().Play();
        Destroy(SFXClone, _sfx.length);
    }


    public void SetMusicVolume(float _volume)
    {
        musicVolume = _volume;
        musicSource.volume = _volume;
    }

    public void SetSFXVolume(float _volume)
    {
        sfxVolume = _volume;
        ambientSource.volume = _volume;
    }
}
