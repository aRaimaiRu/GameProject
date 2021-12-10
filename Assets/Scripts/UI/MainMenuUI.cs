using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void OnStartGamePressed()
    {
        SceneManager.LoadScene("Setting");
    }
    public void OnJoinGamePressed()
    {
        SceneManager.LoadScene("Join");
    }
    public void OnQuitPressed()
    {
        Application.Quit();
    }
}
