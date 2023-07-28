using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    private UIManager _uiManager;
    private Board _board;
    private bool _endingRound = false;

    public float roundTime = 60f;
    public int currentScore = 0;
    public float displayScore = 0;
    public float scoreSpeed = 0;

    public int scoreTarget1, scoreTarget2, scoreTarget3;

    void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
        _board = FindObjectOfType<Board>();
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
                _endingRound = true;
            }
        }
        if (_endingRound && _board.currentState == Board.BoardState.Move)
        {
            WinCheck();
            _endingRound=false;
        }
        _uiManager.timeText.text = roundTime.ToString("0.0") + "s";
        displayScore = Mathf.Lerp(displayScore, currentScore, scoreSpeed * Time.deltaTime);
        _uiManager.scoreText.text = displayScore.ToString("0");
    }

    void WinCheck()
    {
        _uiManager.roundOverScreen.SetActive(true);
        _uiManager.winScore.text = currentScore.ToString();
        if (currentScore >= scoreTarget3)
        {
            _uiManager.winText.text = "Congratulations! You earned 3 stars.";
            _uiManager.winStarsThree.SetActive(true);
        }
        if (currentScore < scoreTarget3 && currentScore >= scoreTarget2)
        {
            _uiManager.winText.text = "Congratulations! You earned 2 stars.";
            _uiManager.winStarsTwo.SetActive(true);
        }
        if (currentScore < scoreTarget2 && currentScore >= scoreTarget1)
        {
            _uiManager.winText.text = "Congratulations! You earned 1 star.";
            _uiManager.winStarsOne.SetActive(true);
        }
        if (currentScore < scoreTarget1)
        {
            _uiManager.winText.text = "Oh no! No stars earned this time. Try again?";
        }
    }
}
