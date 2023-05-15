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
    }
}
