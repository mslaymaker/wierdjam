using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAmbientAudioPlayer : MonoBehaviour
{
    [Header("References")]//audio source used for ambient announcements
    [SerializeField] private AudioSource ambientAudioSource;

    [Header("Audio Clips")]//clips that will be randomly played
    [SerializeField] private AudioClip[] audioClips;

    [Header("Playback Settings")]//controls timing and volume
    [SerializeField] private float minInterval = 5f;
    [SerializeField] private float maxInterval = 15f;
    [SerializeField] private float volume = 1f;

    //stores clips that haven't been played during the current cycle
    private List<AudioClip> remainingClips = new List<AudioClip>();

    private void Start()
    {
        refillClipPool();

        StartCoroutine(playRandomAudioLoop());
    }

    private IEnumerator playRandomAudioLoop()//continuously play random clips at random intervals
    {
        while (true)
        {
            float waitTime =
                Random.Range(minInterval, maxInterval);

            yield return new WaitForSeconds(waitTime);

            playRandomClip();
        }
    }

    private void playRandomClip()//play a random clip without repeating until all clips are used
    {
        if (ambientAudioSource == null) return;

        if (audioClips == null || audioClips.Length == 0) return;

        if (remainingClips.Count == 0)
        {
            refillClipPool();
        }

        int randomIndex =
            Random.Range(0, remainingClips.Count);

        AudioClip selectedClip =
            remainingClips[randomIndex];

        remainingClips.RemoveAt(randomIndex);

        ambientAudioSource.PlayOneShot(selectedClip, volume);
    }

    private void refillClipPool()//start a new cycle of available clips
    {
        remainingClips.Clear();

        foreach (AudioClip clip in audioClips)
        {
            if (clip != null)
            {
                remainingClips.Add(clip);
            }
        }
    }
}
