using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishGameScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highscoreText;
    [SerializeField] private GameObject newHighscoreText;

    public void OnClickPlayAgain()
    {
        GameManager.Instance.ResetGame();
    }

    public void OnClickMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Setup(int currentScore, int highscore, bool newHighscore)
    {
        scoreText.text = "Game Score: " + currentScore.ToString();
        highscoreText.text = "Highscore: " + highscore.ToString();
        
        newHighscoreText.SetActive(newHighscore);
        
    }
    public void ShowScreen()
    {
        gameObject.SetActive(true);
    }
}
