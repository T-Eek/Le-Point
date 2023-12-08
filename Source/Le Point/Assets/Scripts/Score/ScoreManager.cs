using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject GameOverScreen;

    public static int scoreCount;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
    }

    void UpdateScore()
    {
        // Update UI text
        scoreText.text = "Score: " + scoreCount;
    }
}
