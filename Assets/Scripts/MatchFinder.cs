using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MatchFinder : MonoBehaviour
{

    private Board _board;
    public List<Gem> currentMatches = new List<Gem>();


    void Awake()
    {
        _board = FindObjectOfType<Board>();
    }

    public void FindAllMatches()
    {
        currentMatches.Clear();

        for (int x = 0; x < _board.width; x++)
        {
            for (int y = 0; y < _board.height; y++)
            {
                Gem currentGem = _board.allGems[x, y];
                if (currentGem != null)
                {
                    if (x > 0 && x < _board.width - 1)
                    {
                        Gem leftGem = _board.allGems[x - 1, y];
                        Gem rightGem = _board.allGems[x + 1, y];
                        if (leftGem != null && rightGem != null)
                        {
                            if (leftGem.type == currentGem.type && rightGem.type == currentGem.type)
                            {
                                Debug.Log("We found a match");
                                currentGem.isMatched = true;
                                leftGem.isMatched = true;
                                rightGem.isMatched = true;

                                currentMatches.Add(currentGem);
                                currentMatches.Add(leftGem);
                                currentMatches.Add(rightGem);
                            }
                        }
                    }
                    if (y > 0 && y < _board.height - 1)
                    {
                        Gem aboveGem = _board.allGems[x, y + 1];
                        Gem belowGem = _board.allGems[x, y - 1];
                        if (aboveGem != null && belowGem != null)
                        {
                            if (aboveGem.type == currentGem.type && belowGem.type == currentGem.type)
                            {
                                Debug.Log("We found a match");
                                currentGem.isMatched = true;
                                aboveGem.isMatched = true;
                                belowGem.isMatched = true;

                                currentMatches.Add(currentGem);
                                currentMatches.Add(aboveGem);
                                currentMatches.Add(belowGem);
                            }
                        }
                    }

                }
            }
        }
        if (currentMatches.Count > 0)
        {
            currentMatches = currentMatches.Distinct().ToList();
        }
        CheckForBombs();
    }

    public void CheckForBombs()
    {
        for(int i = 0; i < currentMatches.Count; i++)
        {
            Gem gem = currentMatches[i];
            
            int x = gem.positionIndex.x;
            int y = gem.positionIndex.y;

            if (gem.positionIndex.x > 0)
            {
                if (_board.allGems[x - 1, y] != null)
                {
                    if (_board.allGems[x - 1, y].type == Gem.GemType.bomb)
                    {
                        MarkBombArea(new Vector2Int(x - 1, y), _board.allGems[x - 1, y]);
                    }
                }
            }
            if (gem.positionIndex.x < _board.width - 1)
            {
                if (_board.allGems[x + 1, y] != null)
                {
                    if (_board.allGems[x + 1, y].type == Gem.GemType.bomb)
                    {
                        MarkBombArea(new Vector2Int(x + 1, y), _board.allGems[x + 1, y]);
                    }
                }
            }
            if (gem.positionIndex.y > 0)
            {
                if (_board.allGems[x, y - 1] != null)
                {
                    if (_board.allGems[x, y - 1].type == Gem.GemType.bomb)
                    {
                        MarkBombArea(new Vector2Int(x, y - 1), _board.allGems[x, y - 1]);
                    }
                }
            }
            if (gem.positionIndex.y < _board.height - 1)
            {
                if (_board.allGems[x, y + 1] != null)
                {
                    if (_board.allGems[x, y + 1].type == Gem.GemType.bomb)
                    {
                        MarkBombArea(new Vector2Int(x, y + 1), _board.allGems[x, y + 1]);
                    }
                }
            }
        }
    }

    public void MarkBombArea(Vector2Int bombPosition, Gem theBomb)
    {
        for(int x = bombPosition.x - theBomb.blastRadius; x <= bombPosition.x + theBomb.blastRadius; x++)
        {
            for(int y = bombPosition.y - theBomb.blastRadius; y <= bombPosition.y + theBomb.blastRadius; y++)
            {
                if (x >= 0 && x < _board.width && y >= 0 && y < _board.height)
                {
                    if (_board.allGems[x, y] != null)
                    {
                        _board.allGems[x, y].isMatched = true;
                        currentMatches.Add(_board.allGems[x, y]);
                    }
                }
            }
        }
        currentMatches = currentMatches.Distinct().ToList();
    }
}
