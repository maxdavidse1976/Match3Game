using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public float roundTime = 60f;
    public int currentScore = 0;
    public float displayScore = 0;
    public float scoreSpeed = 0;

    private UIManager uiManager;

    private bool endingRound = false;
    private Board board;

    public int scoreTarget1, scoreTarget2, scoreTarget3;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        board = FindObjectOfType<Board>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (roundTime > 0)
        {
            roundTime -= Time.deltaTime;

            if (roundTime <= 0)
            {
                roundTime = 0;
                endingRound = true;
            }
        }
        if (endingRound && board.currentState == Board.BoardState.Move)
        {
            WinCheck();
            endingRound=false;
        }
        uiManager.timeText.text = roundTime.ToString("0.0") + "s";
        displayScore = Mathf.Lerp(displayScore, currentScore, scoreSpeed * Time.deltaTime);
        uiManager.scoreText.text = displayScore.ToString("0");
    }

    private void WinCheck()
    {
        uiManager.roundOverScreen.SetActive(true);
        uiManager.winScore.text = currentScore.ToString();
        if (currentScore >= scoreTarget3)
        {
            uiManager.winText.text = "Congratulations! You earned 3 stars.";
            uiManager.winStarsThree.SetActive(true);
        }
        if (currentScore < scoreTarget3 && currentScore >= scoreTarget2)
        {
            uiManager.winText.text = "Congratulations! You earned 2 stars.";
            uiManager.winStarsTwo.SetActive(true);
        }
        if (currentScore < scoreTarget2 && currentScore >= scoreTarget1)
        {
            uiManager.winText.text = "Congratulations! You earned 1 star.";
            uiManager.winStarsOne.SetActive(true);
        }
        if (currentScore < scoreTarget1)
        {
            uiManager.winText.text = "Oh no! No stars earned this time. Try again?";
        }
    }
}
