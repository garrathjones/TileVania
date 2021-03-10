using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    [SerializeField] Text livesText;
    [SerializeField] Text scoreText;
    [SerializeField] float timeToWait = 2f;

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numGameSessions>1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            score = 0;
            scoreText.text = score.ToString();
            playerLives = 3;
            livesText.text = playerLives.ToString();
        }
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString();
    }

    public void ProcessPlayerDeath()
    {
        StartCoroutine(WaitThenProcessDeath());
    }

    IEnumerator WaitThenProcessDeath()
    {
        yield return new WaitForSeconds(timeToWait);
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            GoToLoseScreen();
        }
    }

    private void GoToLoseScreen()
    {
        SceneManager.LoadScene("You Lose");
        Destroy(gameObject);
    }

    private void TakeLife()
    {
        playerLives--;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();
    }
}
