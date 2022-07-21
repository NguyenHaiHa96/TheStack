using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject menuPanel, gameOverPanel;
    public int currentScore = 0;
    public Text scoreText, highScoreText;

    private void Start()
    {
        MovingBlock.OnBlockSpawned += MovingBlock_OnBlockSpawned;
    }
    public int GetCurrentScore()
    {
        return currentScore;
    }

    private void MovingBlock_OnBlockSpawned()
    {
        currentScore++;
        SetScoreText();
    }

    public void SetHighScore()
    {
        if (PlayerPrefs.GetInt("HighScore") < currentScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
        }
    }

    public void ShowGameOverPanel(bool isShow)
    {
        gameOverPanel.SetActive(isShow);
    }

    private void OnDestroy()
    {
        MovingBlock.OnBlockSpawned -= MovingBlock_OnBlockSpawned;
    }

    public void Unsubscribe()
    {
        MovingBlock.OnBlockSpawned -= MovingBlock_OnBlockSpawned;
    }

    public void SetScoreText()
    {
        if (scoreText)
            scoreText.text = currentScore.ToString();
    }

    public void ShowScoreText(bool isShow)
    {
        if (scoreText)
            scoreText.gameObject.SetActive(isShow);
    }

    public void ShowHighScoreText(bool isShow)
    {
        if (highScoreText)
        {
            highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
            highScoreText.gameObject.SetActive(isShow);
        }     
    }

    public void ShowMenuPanel(bool isShow)
    {
        if (menuPanel)
            menuPanel.SetActive(isShow);
    }
}
