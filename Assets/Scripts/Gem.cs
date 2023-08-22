using System.Collections;
using UnityEngine;

namespace DSG.Match3
{ 
    public partial class Gem : MonoBehaviour
    {
        Vector2 _firstTouchPosition;
        Vector2 _finalTouchPosition;
        Gem _otherGem;
        Vector2Int _previousPosition;

        bool _mousePressed;
        float _swipeAngle = 0;

        public GameObject destroyEffect;
        public GemType type;

        public bool isMatched;
        public int blastRadius = 2;
        public int scoreValue = 10;

        [HideInInspector] public Vector2Int positionIndex;
        [HideInInspector] public Board board;


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

            if (_mousePressed && Input.GetMouseButtonUp(0))
            {
                _mousePressed = false;
                if (board.currentState == Board.BoardState.Move)
                {
                    _finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    CalculateAngle();
                }
            }
        }

        public void SetupGem(Vector2Int position, Board theBoard)
        {
            positionIndex = position;
            board = theBoard;
        }

        void OnMouseDown()
        {
            if (board.currentState == Board.BoardState.Move && board.roundManager.roundTime > 0)
            {
                _firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _mousePressed = true;
            }
        }
    
        void CalculateAngle()
        {
            _swipeAngle = Mathf.Atan2(_finalTouchPosition.y - _firstTouchPosition.y, _finalTouchPosition.x - _firstTouchPosition.x);
            _swipeAngle = _swipeAngle * 180 / Mathf.PI;

            if (Vector3.Distance(_firstTouchPosition, _finalTouchPosition) > 0.5f)
            {
                MovePieces();
            }
        }

        void MovePieces()
        {
            _previousPosition = positionIndex;
            // If angle between 45 and -45 move the piece to the right
            if (_swipeAngle < 45 && _swipeAngle > -45 && positionIndex.x < board.width - 1)
            {
                _otherGem = board.allGems[positionIndex.x + 1, positionIndex.y];
                _otherGem.positionIndex.x--;
                positionIndex.x++;
            }
            // If angle between 135 and 45 move the piece up
            else if (_swipeAngle > 45 && _swipeAngle <= 135 && positionIndex.x < board.height - 1)
            {
                _otherGem = board.allGems[positionIndex.x, positionIndex.y + 1];
                _otherGem.positionIndex.y--;
                positionIndex.y++;
            }
            else if (_swipeAngle < -45 && _swipeAngle >= -135 && positionIndex.y > 0)
            {
                _otherGem = board.allGems[positionIndex.x, positionIndex.y - 1];
                _otherGem.positionIndex.y++;
                positionIndex.y--;
            }
            else if (_swipeAngle > 135 || _swipeAngle < -135 && positionIndex.x > 0)
            {
                _otherGem = board.allGems[positionIndex.x - 1, positionIndex.y];
                _otherGem.positionIndex.x++;
                positionIndex.x--;
            }
            board.allGems[positionIndex.x, positionIndex.y] = this;
            board.allGems[_otherGem.positionIndex.x, _otherGem.positionIndex.y] = _otherGem;

            StartCoroutine(CheckMoveCoroutine());
        }

        public IEnumerator CheckMoveCoroutine()
        {
            board.currentState = Board.BoardState.Wait;

            yield return new WaitForSeconds(.1f);

            board.matchFinder.FindAllMatches();

            if (_otherGem != null)
            {
                if (!isMatched && !_otherGem.isMatched)
                {
                    _otherGem.positionIndex = positionIndex;
                    positionIndex = _previousPosition;

                    board.allGems[positionIndex.x, positionIndex.y] = this;
                    board.allGems[_otherGem.positionIndex.x, _otherGem.positionIndex.y] = _otherGem;

                    yield return new WaitForSeconds(.1f);
                    board.currentState = Board.BoardState.Move;
                }
                else
                {
                    board.DestroyMatches();
                }
            }
        }
    }
}