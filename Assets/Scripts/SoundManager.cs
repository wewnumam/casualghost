using UnityEngine.Audio;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager Instance;

    public AudioMixer audioMixer;
    public Sound[] sounds;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    public void PlaySound(EnumsManager.SoundEffect soundEffect) {
        Sound sound = GetSound(soundEffect);
        if (sound == null) {
            Debug.LogWarning("Sound " + soundEffect.ToString() + " not found.");
            return;
        }

        sound.source.Play();
    }

    public void StopSound(EnumsManager.SoundEffect soundEffect) {
        Sound sound = GetSound(soundEffect);
        if (sound == null) {
            Debug.LogWarning("Sound " + soundEffect.ToString() + " not found.");
            return;
        }

        sound.source.Stop();
    }

    public void SetVolume(float volume) {
        audioMixer.SetFloat("Volume", volume);
    }

    private Sound GetSound(EnumsManager.SoundEffect soundEffect) {
        Sound sound = System.Array.Find(sounds, s => s.soundEffect == soundEffect);
        return sound;
    }
}

[System.Serializable]
public class Sound {
    public EnumsManager.SoundEffect soundEffect;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0.1f, 3f)]
    public float pitch = 1f;
    public bool loop;
    [HideInInspector]
    public AudioSource source;
}
