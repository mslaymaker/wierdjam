using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Scene Settings")]//scene that should load when space is pressed
    [SerializeField] private string sceneName;

    private void Update()
    {
        loadSceneOnSpacePress();
    }

    private void loadSceneOnSpacePress()//load the desired scene when the player presses space
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}