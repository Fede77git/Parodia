using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void EscenaJuego()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("NBA");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
