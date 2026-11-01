using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerSceneLoader : MonoBehaviour
{
    [Header("Scene Settings")]//scene that should load when the player enters the trigger
    [SerializeField] private string sceneName;

    private void OnTriggerEnter(Collider other)//load the desired scene when the player enters the trigger
    {
        if (!other.CompareTag("Player")) return;

        SceneManager.LoadScene(sceneName);
    }
}