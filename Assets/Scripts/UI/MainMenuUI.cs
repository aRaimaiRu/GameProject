using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  //คำสั่งในการเรียกใช้ SceneManager

public class MainMenuUI : MonoBehaviour
{
    public void OnStartGamePressed()
    {
        SceneManager.LoadScene("Setting");      // เปลี่ยนไปหน้า Setting
    }
    public void OnJoinGamePressed()
    {
        SceneManager.LoadScene("Join");         //เปลี่ยนไปหน้า Join
    }
    public void OnQuitPressed()
    {
        Application.Quit();                     //จบการทำงาน
    }
}
