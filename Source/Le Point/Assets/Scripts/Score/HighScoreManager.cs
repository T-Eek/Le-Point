using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public TextMeshProUGUI hiScoreText;

    public static int hiScoreCount; // Expose this variable to the Unity Editor

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

        if(ScoreManager.scoreCount > hiScoreCount)
        {
            hiScoreCount = ScoreManager.scoreCount;
            PlayerPrefs.SetInt("HighScore", hiScoreCount);
        }
        UpdateScore();
        // Log the current high score to the console
        Debug.Log("Current High Score: " + hiScoreCount);
    }

    void UpdateScore()
    {
        // Update UI text
        hiScoreText.text = "Hi-Score: " + hiScoreCount;
    }
}
