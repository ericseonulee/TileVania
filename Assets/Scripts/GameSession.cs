using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour {
    [SerializeField] private int _playerLives = 3;
    void Awake() {
        int numGameSession = FindObjectsOfType<GameSession>().Length;
        
        if (numGameSession > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }

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
    }
}
