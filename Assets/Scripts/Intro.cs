using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    [SerializeField] AudioSource aSource;
    [SerializeField] GameObject introAnim;
    [SerializeField] GameObject introMan;


    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }


    public void ClipDone()
    {
        //if (!introAnim)
        //{
        DestroyTargetObject();
            SceneManager.LoadScene(2);
        //}
    }
    public void StartMusic()
    {
        aSource.Play();
        introAnim.SetActive(false);
        introMan.SetActive(true);
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

