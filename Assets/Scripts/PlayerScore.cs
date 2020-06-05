﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class PlayerScore : MonoBehaviour
{
    public int runningScore = 100;

    public int score { get; private set; }

    private Text scoreText;
    private bool gameStart = false;

    public float elapsedTime;
    private float secondClock;

    void Awake()
    {
        scoreText = GetComponent<Text>();
    }

    void FixedUpdate()
    {
        if(gameStart == true)
        {
            secondClock += Time.deltaTime;
            while(secondClock > 1f)
            {
                secondClock -= 1f;
                AddScore(runningScore);
            }

            elapsedTime += Time.deltaTime;

            if(elapsedTime > GameManager.GameplayLength)
            {
                gameStart = false;
                GameManager.instance.EndGame(score);
            }
        }
    }

    public void AddScore(int runningScore)
    {
        score += runningScore;
        UpdateScoreText();
    }

    public void StartGame()
    {
        elapsedTime = 0;
        gameStart = true;
    }

    private void UpdateScoreText()
    {
        scoreText.text = "SCORE: " + score.ToString();
    }
}
