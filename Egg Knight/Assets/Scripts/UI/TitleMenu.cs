using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    public void StartGame() {
        Debug.Log("STARTING GAME");
        SceneManager.LoadScene("Scenes/LevelsScene");
    }

    public void QuitGame() {
        Debug.Log("QUITTING GAME");
        Application.Quit();
    }
}
