using System.Collections;
using UnityEngine;

public partial class Gem : MonoBehaviour
{
    [HideInInspector] public Vector2Int positionIndex;
    [HideInInspector] public Board board;

    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;

    private bool mousePressed;
    private float swipeAngle = 0;

    private Gem otherGem;
    public GemType type;

    public bool isMatched;
    
    private Vector2Int previousPosition;

    public GameObject destroyEffect;

    public int blastRadius = 2;

    void Start()
    {
        
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, positionIndex) > .01f)
        {
            transform.position = Vector2.Lerp(transform.position, positionIndex, board.gemSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(positionIndex.x, positionIndex.y, 0f);
            board.allGems[positionIndex.x, positionIndex.y] = this;
        }

        if (mousePressed && Input.GetMouseButtonUp(0))
        {
            mousePressed = false;
            if (board.currentState == Board.BoardState.Move)
            {
                finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CalculateAngle();
            }
        }
    }

    public void SetupGem(Vector2Int position, Board theBoard)
    {
        positionIndex = position;
        board = theBoard;
    }

    private void OnMouseDown()
    {
        if (board.currentState == Board.BoardState.Move)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePressed = true;
        }
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
        previousPosition = positionIndex;
        // If angle between 45 and -45 move the piece to the right
        if (swipeAngle < 45 && swipeAngle > -45 && positionIndex.x < board.width - 1)
        {
            otherGem = board.allGems[positionIndex.x + 1, positionIndex.y];
            otherGem.positionIndex.x--;
            positionIndex.x++;
        }
        // If angle between 135 and 45 move the piece up
        else if (swipeAngle > 45 && swipeAngle <= 135 && positionIndex.x < board.height - 1)
        {
            otherGem = board.allGems[positionIndex.x, positionIndex.y + 1];
            otherGem.positionIndex.y--;
            positionIndex.y++;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && positionIndex.y > 0)
        {
            otherGem = board.allGems[positionIndex.x, positionIndex.y - 1];
            otherGem.positionIndex.y++;
            positionIndex.y--;
        }
        else if (swipeAngle > 135 || swipeAngle < -135 && positionIndex.x > 0)
        {
            otherGem = board.allGems[positionIndex.x - 1, positionIndex.y];
            otherGem.positionIndex.x++;
            positionIndex.x--;
        }
        board.allGems[positionIndex.x, positionIndex.y] = this;
        board.allGems[otherGem.positionIndex.x, otherGem.positionIndex.y] = otherGem;

        StartCoroutine(CheckMoveCoroutine());
    }

    public IEnumerator CheckMoveCoroutine()
    {
        board.currentState = Board.BoardState.Wait;

        yield return new WaitForSeconds(.5f);

        board.matchFinder.FindAllMatches();

        if (otherGem != null)
        {
            if (!isMatched && !otherGem.isMatched)
            {
                otherGem.positionIndex = positionIndex;
                positionIndex = previousPosition;

                board.allGems[positionIndex.x, positionIndex.y] = this;
                board.allGems[otherGem.positionIndex.x, otherGem.positionIndex.y] = otherGem;

                yield return new WaitForSeconds(.5f);
                board.currentState = Board.BoardState.Move;
            }
            else
            {
                board.DestroyMatches();
            }
        }
    }
}
