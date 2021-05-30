using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject youWinText;
    public GameObject youLoseText;
    public GameObject playerOneWinsText;
    public GameObject playerTwoWinsText;
    public GameObject playAgainButton;
    public GameObject quitButton;
    public Wizard player1;
    public Wizard player2;

    bool singlePlayer = true;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("SinglePlayer") == 1) {
            singlePlayer = true;
            player1.life = 1;
            player2.life = 5;
            player2.isHuman = false;
        } else {
            singlePlayer = false;
            player1.life = 1;
            player2.life = 1;
            player2.isHuman = true;
        }
    }

    public void PlayerDied(int playerNumber) {
        if (singlePlayer) {
            if (playerNumber == 1) {
                youLoseText.SetActive(true);
            } else {
                youWinText.SetActive(true);
            }
        } else {
            if (playerNumber == 1) {
                playerTwoWinsText.SetActive(true);
            } else {
                playerOneWinsText.SetActive(true);
            }
        }
        playAgainButton.SetActive(true);
        quitButton.SetActive(true);
    } 

    public void PlayAgain() {
        SceneManager.LoadScene("GameScene");
    }

    public void Quit() {
        SceneManager.LoadScene("HomeScene");
    }
}
