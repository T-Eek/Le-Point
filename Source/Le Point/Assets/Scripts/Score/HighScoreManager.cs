using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public TextMeshProUGUI hiScoreText;

    public static int hiScoreCount; // Expose this variable to the Unity Editor

    private bool shouldResetHighScore = false; // Set this to true when you want to reset the high score

    // Start is called before the first frame update
    private void Start()
    {
        // If there is a stored HighScore in PlayerPrefs, load it
        if (PlayerPrefs.HasKey("HighScore"))
        {
            hiScoreCount = PlayerPrefs.GetInt("HighScore");
        }
        UpdateScore();
    }

    // Update is called once per frame
    private void Update()
    {
        //Checks if shouldResetHighScore is true, then resets the high score
        if (shouldResetHighScore)
        {
            ResetHighScore();// Method to reset the high score
            shouldResetHighScore = false;
        }
        //Link between HighScoreManager and ScoreManager for the hiScoreCount
        else if (ScoreManager.scoreCount > hiScoreCount)//Gets te ScoreManager scorecount 
        {
            hiScoreCount = ScoreManager.scoreCount; //Gets the scoreCount from script ScoreManager
            PlayerPrefs.SetInt("HighScore", hiScoreCount);
        }
        UpdateScore();
        // Log the current high score to the console
        Debug.Log("Current High Score: " + hiScoreCount);
    }

    void UpdateScore()
    {
        // Update UI text only if hiScoreCount is not default value (0)
        hiScoreText.text = hiScoreCount != default(int) ? "Hi-Score: " + hiScoreCount : "";
    }

    public void ResetHighScore()
    {
        hiScoreCount = 0;
        UpdateScore();
    }
}
