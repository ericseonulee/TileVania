using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour {
    [SerializeField] private int _playerLives = 3;
    [SerializeField] private int _playerScore = 0;
    [SerializeField] private TextMeshProUGUI _livesText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    void Awake() {
        int numGameSession = FindObjectsOfType<GameSession>().Length;
        
        if (numGameSession > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }

    }

    void Start() {
        _livesText.text = _playerLives.ToString();
        _scoreText.text = _playerScore.ToString();
    }

    public void ProcessPlayerDeath() {
        if (_playerLives > 1) {
            TakeLife();
        }
        else {
            ResetGameSession();
        }
    }

    private void ResetGameSession() {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void TakeLife() {
        _playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        SceneManager.LoadScene(currentSceneIndex);
        _livesText.text = _playerLives.ToString();
    }

    public void AddToScore(int pointsToAdd) {
        _playerScore += pointsToAdd;
        _scoreText.text = _playerScore.ToString();
    }
}
