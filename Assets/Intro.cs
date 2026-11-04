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
            SceneManager.LoadScene(2);
        //}
    }
    public void StartMusic()
    {
        aSource.Play();
        introAnim.SetActive(false);
        introMan.SetActive(true);
    }
}

