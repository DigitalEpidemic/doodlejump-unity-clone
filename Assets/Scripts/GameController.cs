using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    [SerializeField] Text scoreText;
    [SerializeField] Text gameOverScoreText;
    [SerializeField] Animator platformsAnim;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] Button pauseButton;

    int score;
    float maxHeight;
    bool gameOver = false;

    GameObject player;

    Vector3 topLeft = Vector3.zero;
    Vector3 cameraPos = Vector3.zero;
    
    void Awake () {
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 cameraPos = Camera.main.transform.position;

        // Disable unnecessary warning about topLeft not being used
#pragma warning disable 0219
        Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
#pragma warning restore 0219
    }

    void FixedUpdate() {
        if (!gameOver) {
            // Calculate max height
            if (player.transform.position.y > maxHeight) {
                maxHeight = player.transform.position.y;
            }

            // TODO Check if player has fallen
            if (player.transform.position.y - Camera.main.transform.position.y < GetDestroyDistance()) {
                // TODO Play game over sound

                // Game Over
                EndGame();
                gameOver = true;
            }
        }

        score = (int)(maxHeight * 50);
        scoreText.text = score.ToString();
    }

    public float GetDestroyDistance() {
        return cameraPos.y + topLeft.y -9.05f;
    }

    public int GetScore() {
        return score;
    }

    void EndGame() {
        //Debug.Log("Game is over");
        gameOverScoreText.text = score.ToString();

        // Animate platforms
        platformsAnim.SetTrigger("GameOver");

        // Animate/Enable GameOver panel
        gameOverPanel.SetActive(true);

        // Disable pause button
        pauseButton.interactable = false;
    }

    public void PlayAgainButton() {
        SceneManager.LoadScene("Game");
    }

    public void PauseButton() {
        Time.timeScale = 0f; // Pause the game time
        pausePanel.SetActive(true);
    }

    public void ResumeButton() {
        Time.timeScale = 1f; // Resume the game time
        pausePanel.SetActive(false);
    }

    public void MenuButton() {
        SceneManager.LoadScene("MainMenu");
    }

}
