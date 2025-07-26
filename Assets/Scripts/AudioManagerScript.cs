using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI elements like Slider

public class AudioManagerScript : MonoBehaviour
{
    // Music settings
    public List<AudioClip> musicClips;
    public AudioSource musicAudioSource; // Renamed for clarity
    [Range(0.1f, 3f)]
    public float fadeDuration = 1f;
    [Range(0f, 0.29f)] // Maximum music volume is now 0.29f
    public float musicVolume = 0.2f; // Default music volume (adjusted for new max)

    // SFX settings
    public AudioSource sfxAudioSource; // Renamed for clarity
    public AudioClip buttonClickSFX; // Specific clip for button clicks
    [Range(0f, 1f)]
    public float sfxVolume = 1f; // Default SFX volume (SFX can still go up to 1f)

    private void Awake()
    {
        // Set initial volumes
        if (musicAudioSource != null)
        {
            musicAudioSource.volume = musicVolume;
        }
        if (sfxAudioSource != null)
        {
            sfxAudioSource.volume = sfxVolume;
        }
    }

    private void Start()
    {
        if (musicClips.Count == 0 || musicAudioSource == null) return;
        StartCoroutine(PlayMusicLoop());
    }

    // Coroutine for playing music randomly with fades
    IEnumerator PlayMusicLoop()
    {
        while (true)
        {
            AudioClip nextClip = musicClips[Random.Range(0, musicClips.Count)];
            yield return StartCoroutine(FadeIn(nextClip));

            // Wait for the remaining duration of the clip after fade-in
            yield return new WaitForSeconds(nextClip.length - fadeDuration);
            yield return StartCoroutine(FadeOut());
        }
    }

    // Method to play a specific SFX clip
    public void PlaySFX(AudioClip clip)
    {
        if (sfxAudioSource != null && clip != null)
        {
            sfxAudioSource.PlayOneShot(clip, sfxAudioSource.volume); // PlayOneShot uses the current sfxAudioSource.volume
        }
    }

    // Public method to be called by a UI Button's OnClick event for button clicks
    public void PlayButtonClickSFX()
    {
        PlaySFX(buttonClickSFX);
    }

    // Public method to set music volume from a UI Slider
    public void SetMusicVolume(float volume)
    {
        // Clamp the volume to the new max
        musicVolume = Mathf.Clamp(volume, 0f, 0.29f);
        if (musicAudioSource != null)
        {
            musicAudioSource.volume = musicVolume;
        }
    }

    // Public method to set SFX volume from a UI Slider
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        if (sfxAudioSource != null)
        {
            sfxAudioSource.volume = sfxVolume;
        }
    }

    // Fade In coroutine
    IEnumerator FadeIn(AudioClip clip)
    {
        musicAudioSource.clip = clip;
        musicAudioSource.volume = 0f; // Start from 0 for fade in
        musicAudioSource.Play();

        float t = 0;
        float currentTargetVolume = musicVolume; // Fade in to the current musicVolume setting

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            musicAudioSource.volume = Mathf.Lerp(0f, currentTargetVolume, t / fadeDuration);
            yield return null;
        }
        musicAudioSource.volume = currentTargetVolume; // Ensure it reaches the target volume
    }

    // Fade Out coroutine
    IEnumerator FadeOut()
    {
        float startVolume = musicVolume; // Start from the current music volume
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            musicAudioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        musicAudioSource.Stop();
        musicAudioSource.volume = musicVolume; // Reset volume to current setting after stopping, for the next fade-in
    }
}