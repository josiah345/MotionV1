using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // one global instance any script can access using ScoreManager.instance
    public static ScoreManager instance;

    // the three text boxes on the canvas
    public TMP_Text scoreText;  // shows current score
    public TMP_Text timerText;  // shows time remaining
    public TMP_Text endText;    // shows win or lose message

    // how long each round lasts in seconds
    public float roundTime = 60f;

    // the score the player needs to reach to win
    public int scoreToWin = 50;

    private int score;    // current score
    private float timeLeft; // seconds remaining
    private bool running;  // true while round is active

    // runs before Start — makes this the global instance
    void Awake() { instance = this; }

    // hides the end message and starts the first round
    void Start()
    {
        endText.gameObject.SetActive(false);
        StartRound();
    }

    void Update()
    {
        // if round is over wait for H to restart
        if (!running)
        {
            if (Input.GetKeyDown(KeyCode.H))
                StartRound();
            return;
        }

        // count the timer down every frame
        timeLeft -= Time.deltaTime;

        // display as whole numbers — no decimals
        timerText.text = "Time: " + Mathf.CeilToInt(Mathf.Max(timeLeft, 0f));

        // time ran out — end the round
        if (timeLeft <= 0f)
            EndRound();
    }

    // resets everything and begins a fresh round
    void StartRound()
    {
        score = 0;
        timeLeft = roundTime;
        running = true;

        endText.gameObject.SetActive(false);

        // shows 0 / 50 so the player knows the target from the start
        scoreText.text = score + " / " + scoreToWin;
    }

    // called when time runs out before reaching the target score
    void EndRound()
    {
        running = false;
        timerText.text = "Time: 0";

        endText.gameObject.SetActive(true);
        endText.text = "YOU LOSE!\nScore: " + score + "\nPress H to try again";
    }

    // called by RingZone to add points and PenaltyTarget to subtract points
    public void AddScore(int pts)
    {
        // ignore score changes after the round ends
        if (!running) return;

        score += pts;

        // stop score going below zero
        if (score < 0) score = 0;

        // update display in 0 / 50 format
        scoreText.text = score + " / " + scoreToWin;

        // check if the player has reached the winning score
        if (score >= scoreToWin)
            WinRound();
    }

    // called when the player reaches the target score before time runs out
    void WinRound()
    {
        running = false;

        // freeze the timer at whatever time was left when they won
        timerText.text = "Time: " + Mathf.CeilToInt(Mathf.Max(timeLeft, 0f));

        endText.gameObject.SetActive(true);
        endText.text = "YOU WIN!\nScore: " + score + "\nPress H to play again";
    }

    // lets other scripts read the score without changing it
    public int GetScore() { return score; }

    // lets other scripts check if the round is still active
    public bool IsRoundActive() { return running; }
}