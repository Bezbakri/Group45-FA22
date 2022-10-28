using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    //Creating my variables.
    
    private int score;
    public TMP_Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        //Setting up score to be 0 when the game starts.
        score = 0;
        scoreText.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateScore(int points)
    {
        //Adding some points everytime a row is cleared.
        score += points;
        scoreText.text = score.ToString();
    }
}
