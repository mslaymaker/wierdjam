using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TriggerSoundPlayer : MonoBehaviour
{
    [Header("Audio")]//sound settings for this trigger
    [SerializeField] private AudioClip triggerSound;
    [SerializeField] private float targetVolume = 1f;

    [Header("Fade Settings")]//controls how quickly the sound fades in and out
    [SerializeField] private float fadeInTime = 1f;
    [SerializeField] private float fadeOutTime = 1f;

    [Header("Behavior")]//general trigger behavior settings
    [SerializeField] private bool loopWhileInside = false;
    [SerializeField] private bool playOnlyOnce = false;

    //tracks whether this trigger has already fired
    private bool hasPlayed;

    //tracks whether the player is currently inside the trigger
    private bool playerInside;

    private AudioSource audioSource;

    private Coroutine activeAudioRoutine;

    private void Awake()//grab required references
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.volume = 0f;
    }

    private void OnTriggerEnter(Collider other)//player entered the trigger area
    {
        if (!other.CompareTag("Player")) return;

        //don't replay if this trigger should only happen once
        if (playOnlyOnce && hasPlayed) return;

        playerInside = true;
        hasPlayed = true;

        //stop any existing routine before starting a new one
        if (activeAudioRoutine != null)
        {
            StopCoroutine(activeAudioRoutine);
        }

        activeAudioRoutine = StartCoroutine(playSound());
    }

    private void OnTriggerExit(Collider other)//player left the trigger area
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;

        //for looping sounds start fading out when the player leaves
        if (loopWhileInside)
        {
            if (activeAudioRoutine != null)
            {
                StopCoroutine(activeAudioRoutine);
            }

            activeAudioRoutine = StartCoroutine(fadeOut());
        }
    }

    private IEnumerator playSound()//play the configured audio clip
    {
        if (triggerSound == null) yield break;

        audioSource.clip = triggerSound;
        audioSource.loop = loopWhileInside;
        audioSource.volume = 0f;

        audioSource.Play();

        //fade in from silence
        float timer = 0f;

        while (timer < fadeInTime)
        {
            timer += Time.deltaTime;

            audioSource.volume = Mathf.Lerp(0f, targetVolume, timer / fadeInTime);

            yield return null;
        }

        audioSource.volume = targetVolume;

        //if we're looping just sit here until the player leaves
        if (loopWhileInside)
        {
            while (playerInside)
            {
                yield return null;
            }

            yield break;
        }

        //for one-shot sounds wait until it's time to fade out
        float holdTime = triggerSound.length - fadeInTime - fadeOutTime;

        if (holdTime > 0f)
        {
            yield return new WaitForSeconds(holdTime);
        }

        yield return fadeOut();
    }

    private IEnumerator fadeOut()//smoothly reduce volume before stopping
    {
        float startVolume = audioSource.volume;
        float timer = 0f;

        while (timer < fadeOutTime)
        {
            timer += Time.deltaTime;

            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeOutTime);

            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }
}