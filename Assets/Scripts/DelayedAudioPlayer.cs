using System.Collections;
using UnityEngine;

public class DelayedAudioPlayer : MonoBehaviour
{
    [Header("References")]//audio source used to play the delayed sound
    [SerializeField] private AudioSource audioSource;

    [Header("Audio Settings")]//clip to play after the delay
    [SerializeField] private AudioClip audioClip;

    [Header("Timing Settings")]//how long to wait before playing
    [SerializeField] private float delayTime = 2f;

    [Header("Playback Settings")]//general playback controls
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private float volume = 1f;

    private void Start()
    {
        if (playOnStart)
        {
            StartCoroutine(playDelayed());
        }
    }

    private IEnumerator playDelayed()//wait for the configured delay then play the sound
    {
        yield return new WaitForSeconds(delayTime);

        if (audioSource == null) yield break;

        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip, volume);
        }
    }

    public void playNow()//allow other scripts to trigger the delayed playback
    {
        StartCoroutine(playDelayed());
    }
}