using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DelayedSceneLoader : MonoBehaviour
{
    [Header("Scene Settings")]//scene to load after the timer expires
    [SerializeField] private string sceneName;

    [Header("Timing Settings")]//how long to wait before loading the next scene
    [SerializeField] private float delayTime = 10f;

    private void Start()//begin counting down as soon as the scene starts
    {
        StartCoroutine(loadSceneAfterDelay());
    }

    private IEnumerator loadSceneAfterDelay()//wait for the desired time then load the next scene
    {
        yield return new WaitForSeconds(delayTime);
        DestroyTargetObject();//kill the music manager
        SceneManager.LoadScene(sceneName);
    }
    public void DestroyTargetObject()//finds the music manager to kill it
    {
        // Find the GameObject using the specified tag
        GameObject musicObj = GameObject.FindWithTag("MusicObject");

        if (musicObj != null)
        {
            Destroy(musicObj);
            Debug.Log("MusicObject successfully found and destroyed!");
        }
        else
        {
            Debug.LogWarning("Could not find any GameObject with the tag 'MusicObject'.");
        }
    }
}