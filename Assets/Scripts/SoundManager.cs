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
            if (IsBGM(sound.soundEffect)) {
                sound.source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BGM")[0];
            } else {
                sound.source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
            }
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

    public void SetVolume(float volume, bool isBGM) {
        if (isBGM) {
            audioMixer.SetFloat("BGMVolume", volume);
        } else {
            audioMixer.SetFloat("SFXVolume", volume);
        }
    }

    private Sound GetSound(EnumsManager.SoundEffect soundEffect) {
        Sound sound = System.Array.Find(sounds, s => s.soundEffect == soundEffect);
        return sound;
    }

    private bool IsBGM(EnumsManager.SoundEffect soundEffect) {
        return (
            soundEffect == EnumsManager.SoundEffect._BGM_CREDIT_PANEL ||
            soundEffect == EnumsManager.SoundEffect._BGM_GAME_OVER ||
            soundEffect == EnumsManager.SoundEffect._BGM_GAMEPLAY_1 ||
            soundEffect == EnumsManager.SoundEffect._BGM_GAMEPLAY_2 ||
            soundEffect == EnumsManager.SoundEffect._BGM_GAMEPLAY_3 ||
            soundEffect == EnumsManager.SoundEffect._BGM_INTRO_STORY_MAGICAL ||
            soundEffect == EnumsManager.SoundEffect._BGM_INTRO_STORY_TENSE ||
            soundEffect == EnumsManager.SoundEffect._BGM_MAINMENU
        );
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
