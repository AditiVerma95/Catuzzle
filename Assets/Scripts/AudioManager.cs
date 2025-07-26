using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmAudioSource;
    public AudioSource buttonTapAudioSource;
    public AudioSource sfxAudioSource;

    [Header("Volume Sliders")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    [Header("Audio Clips")]
    public List<AudioClip> bgmClips; // Assign 3 BGM clips in inspector

    private string bgm = "Bgm";
    private string sfx = "Sfx";

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private int lastPlayedIndex = -1;

    private void Start() {
        // existing volume setup...
        StartCoroutine(PlayRandomBgmLoop());
    }

    private IEnumerator PlayRandomBgmLoop() {
        while (true) {
            if (bgmClips.Count == 0) yield break;

            int newIndex;
            do {
                newIndex = Random.Range(0, bgmClips.Count);
            } while (newIndex == lastPlayedIndex && bgmClips.Count > 1);

            lastPlayedIndex = newIndex;
            bgmAudioSource.clip = bgmClips[newIndex];
            bgmAudioSource.Play();

            // Wait until this clip finishes
            yield return new WaitForSeconds(bgmClips[newIndex].length);
        }
    }


    public void SetBgm() {
        float value = bgmSlider.value;
        bgmAudioSource.volume = value;
        PlayerPrefs.SetFloat(bgm, value);
        PlayerPrefs.Save();
    }

    public void SetSfx() {
        float value = sfxSlider.value;
        sfxAudioSource.volume = value;
        buttonTapAudioSource.volume = value;
        PlayerPrefs.SetFloat(sfx, value);
        PlayerPrefs.Save();
    }


    // 🔊 Play tap sound on UI button click
    public void PlayButtonTapSound() {
        if (buttonTapAudioSource != null && buttonTapAudioSource.volume > 0) {
            buttonTapAudioSource.Play();
        }
    }
}
