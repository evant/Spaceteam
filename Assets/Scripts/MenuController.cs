using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
     public void playGame() 
     {
        SceneManager.LoadScene("SampleScene");
     }

    public void loadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void loadInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void gameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
 
    public void exitGame() {
        Application.Quit();
    }
}
