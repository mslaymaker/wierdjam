using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    //Scenes/MainMenu				0
    //Scenes/Intro				    1
    //Scenes/MainScene01			2
    //Scenes/WinScene				3
    //Scenes/LoseScene			    4
    //Scenes/Credits				5

    public void MainMenu() 
    {
        SceneManager.LoadScene(0);
    }
    public void TryAgain() 
    {
        SceneManager.LoadScene(2);
    }
    public void StartGame() 
    {
        SceneManager.LoadScene(1);
    }

    public void Credits() 
    {
        SceneManager.LoadScene(5);
    }


    public void QuitGame()
    {

        Application.Quit();

    }


}
