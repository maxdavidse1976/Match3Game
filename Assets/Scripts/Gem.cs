using System.Collections;
using UnityEngine;

public partial class Gem : MonoBehaviour
{
    private Vector2Int _positionIndex;
    [HideInInspector] public Board board;

    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;

    private bool mousePressed;
    private float swipeAngle = 0;

    private Gem otherGem;
    public GemType type;

    public bool isMatched;
    
    private Vector2Int previousPosition;

    void Start()
    {
        
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, _positionIndex) > .01f)
        {
            transform.position = Vector2.Lerp(transform.position, _positionIndex, board.gemSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(_positionIndex.x, _positionIndex.y, 0f);
            board.allGems[_positionIndex.x, _positionIndex.y] = this;
        }

        if (mousePressed && Input.GetMouseButtonUp(0))
        {
            mousePressed = false;
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }

    public void SetupGem(Vector2Int position, Board theBoard)
    {
        _positionIndex = position;
        board = theBoard;
    }

    private void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePressed = true;
    }
    
    private void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x);
        swipeAngle = swipeAngle * 180 / Mathf.PI;

        if (Vector3.Distance(firstTouchPosition, finalTouchPosition) > 0.5f)
        {
            MovePieces();
        }
    }

    private void MovePieces()
    {
        previousPosition = _positionIndex;
        // If angle between 45 and -45 move the piece to the right
        if (swipeAngle < 45 && swipeAngle > -45 && _positionIndex.x < board.width - 1)
        {
            otherGem = board.allGems[_positionIndex.x + 1, _positionIndex.y];
            otherGem._positionIndex.x--;
            _positionIndex.x++;
        }
        // If angle between 135 and 45 move the piece up
        else if (swipeAngle > 45 && swipeAngle <= 135 && _positionIndex.x < board.height - 1)
        {
            otherGem = board.allGems[_positionIndex.x, _positionIndex.y + 1];
            otherGem._positionIndex.y--;
            _positionIndex.y++;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && _positionIndex.y > 0)
        {
            otherGem = board.allGems[_positionIndex.x, _positionIndex.y - 1];
            otherGem._positionIndex.y++;
            _positionIndex.y--;
        }
        else if (swipeAngle > 135 || swipeAngle < -135 && _positionIndex.x > 0)
        {
            otherGem = board.allGems[_positionIndex.x - 1, _positionIndex.y];
            otherGem._positionIndex.x++;
            _positionIndex.x--;
        }
        board.allGems[_positionIndex.x, _positionIndex.y] = this;
        board.allGems[otherGem._positionIndex.x, otherGem._positionIndex.y] = otherGem;

        StartCoroutine(CheckMoveCoroutine());
    }

    public IEnumerator CheckMoveCoroutine()
    {
        yield return new WaitForSeconds(.5f);

        board.matchFinder.FindAllMatches();

        if (otherGem != null)
        {
            if (!isMatched && !otherGem.isMatched)
            {
                otherGem._positionIndex = _positionIndex;
                _positionIndex = previousPosition;

                board.allGems[_positionIndex.x, _positionIndex.y] = this;
                board.allGems[otherGem._positionIndex.x, otherGem._positionIndex.y] = otherGem;
            }
        }
    }
}
