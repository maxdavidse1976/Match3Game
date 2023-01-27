using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    private Vector2Int _positionIndex;
    [HideInInspector] public Board board;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetupGem(Vector2Int position, Board theBoard)
    {
        _positionIndex = position;
        board = theBoard;
    }
}
