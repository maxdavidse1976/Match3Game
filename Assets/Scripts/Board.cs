using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Board : MonoBehaviour
{
    public int width;
    public int height;
    [SerializeField] private GameObject _bgTilePrefab;

    [SerializeField] private Gem[] _gems;

    public Gem[,] allGems;

    public float gemSpeed;

    [HideInInspector]
    public MatchFinder matchFinder;

    public BoardState currentState = BoardState.Move;

    private void Awake()
    {
        matchFinder = FindObjectOfType<MatchFinder>();
    }
    void Start()
    {
        allGems = new Gem[width, height];
        Setup();
    }

    private void Update()
    {
        //matchFinder.FindAllMatches();
    }

    private void Setup()
    {
        Debug.Log("Getting here!");
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 position = new Vector2(x, y);
                GameObject bgTile = Instantiate(_bgTilePrefab, position, Quaternion.identity);
                // Make sure all our tiles are under the board GameObject
                bgTile.transform.parent = transform;
                bgTile.name = $"BG Tile - {x},{y}";

                int gemToUse = Random.Range(0, _gems.Length);

                int iterations = 0;
                while (MatchesAt(new Vector2Int(x, y), _gems[gemToUse]) && iterations < 100)
                {
                    gemToUse = Random.Range(0, _gems.Length);
                    iterations++;
                }
                SpawnGem(new Vector2Int(x, y), _gems[gemToUse]);
            }
        }
    }

    private void SpawnGem(Vector2Int position, Gem gemToSpawn)
    {
        Gem gem = Instantiate(gemToSpawn, new Vector3(position.x, position.y + height, 0), Quaternion.identity);
        gem.transform.parent = transform;
        gem.name = $"Gem {position.x}, {position.y}";
        allGems[position.x, position.y] = gem;

        gem.SetupGem(position, this);
    }

    private bool MatchesAt(Vector2Int positionToCheck, Gem gemToCheck)
    {
        if (positionToCheck.x > 1)
        {
            if (allGems[positionToCheck.x - 1, positionToCheck.y].type == gemToCheck.type
                && allGems[positionToCheck.x - 2, positionToCheck.y].type == gemToCheck.type)
            {
                return true;
            }
        }

        if (positionToCheck.y > 1)
        {
            if (allGems[positionToCheck.x, positionToCheck.y - 1].type == gemToCheck.type
                && allGems[positionToCheck.x, positionToCheck.y - 2].type == gemToCheck.type)
            {
                return true;
            }
        }
        return false;
    }

    private void DestroyMatchedGemAt(Vector2Int position)
    {
        if (allGems[position.x, position.y] != null)
        {
            if (allGems[position.x, position.y].isMatched)
            {
                Destroy(allGems[position.x, position.y].gameObject);
                allGems[position.x, position.y] = null;
            }
        }
    }

    public void DestroyMatches()
    {
        for (int i = 0; i < matchFinder.currentMatches.Count; i++)
        {
            if (matchFinder.currentMatches[i] != null)
            {
                DestroyMatchedGemAt(matchFinder.currentMatches[i].positionIndex);
            }
        }
        StartCoroutine(DescreaseRowCoroutine());
    }

    private IEnumerator DescreaseRowCoroutine()
    {
        yield return new WaitForSeconds(.2f);

        int nullCounter = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allGems[x, y] == null)
                {
                    nullCounter++;
                }
                else if (nullCounter > 0)
                {
                    allGems[x, y].positionIndex.y -= nullCounter;
                    allGems[x, y - nullCounter] = allGems[x, y];
                    allGems[x, y] = null;
                }
            }
            nullCounter = 0;
        }

        StartCoroutine(FillBoardCoroutine());
    }

    private IEnumerator FillBoardCoroutine()
    {
        yield return new WaitForSeconds(.5f);
        RefillBoard();

        yield return new WaitForSeconds(.5f);
        matchFinder.FindAllMatches();
        if (matchFinder.currentMatches.Count > 0)
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
        else
        {
            yield return new WaitForSeconds(.5f);
            currentState = BoardState.Move;
        }

    }

    private void RefillBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allGems[x, y] == null)
                {
                    int gemToUse = Random.Range(0, _gems.Length);

                    SpawnGem(new Vector2Int(x, y), _gems[gemToUse]);
                }
            }
        }
        CheckMisplacedGems();
    }

    private void CheckMisplacedGems()
    {
        List<Gem> foundGems = new List<Gem>();
        foundGems.AddRange(FindObjectsOfType<Gem>());
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (foundGems.Contains(allGems[x, y]))
                {
                    foundGems.Remove(allGems[x, y]);
                }
            }
        }

        foreach(Gem gem in foundGems)
        {
            Destroy(gem.gameObject);
        }
    }
}
