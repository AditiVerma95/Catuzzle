using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMusicPlayer : MonoBehaviour
{
    public List<AudioClip> musicClips;
    public AudioSource audioSource;

    [Range(0.1f, 3f)]
    public float fadeDuration = 1f;

    private void Start()
    {
        if (musicClips.Count == 0 || audioSource == null) return;
        StartCoroutine(PlayMusicLoop());
    }

    IEnumerator PlayMusicLoop()
    {
        while (true)
        {
            AudioClip nextClip = musicClips[Random.Range(0, musicClips.Count)];
            yield return StartCoroutine(FadeIn(nextClip));

            yield return new WaitForSeconds(nextClip.length - fadeDuration);
            yield return StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeIn(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.volume = 0f;
        audioSource.Play();

        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = 0.29f;
    }

    IEnumerator FadeOut()
    {
        float startVolume = 0.29f;
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = 0.29f;
    }
}