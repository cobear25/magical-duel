using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60; 
    }

    public void StartSinglePlayer() {
        PlayerPrefs.SetInt("SinglePlayer", 1);
        SceneManager.LoadScene("GameScene");
    }

    public void StartMultiplayer() {
        PlayerPrefs.SetInt("SinglePlayer", 0);
        SceneManager.LoadScene("GameScene");
    }
}
