using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSG.Match3
{
    public partial class Board : MonoBehaviour
    {
        [SerializeField] GameObject _bgTilePrefab;
        [SerializeField] Gem[] _gems;

        UIManager _uiManager;
        float _bonusMultiplier;
        BoardLayout _boardLayout;
        Gem[,] _layoutStore;

        public int width;
        public int height;
        public Gem[,] allGems;

        public float gemSpeed;
        public BoardState currentState = BoardState.Move;

        public Gem bomb;
        public float bombChance = 2f;

        public float bonusAmount = .5f;

        [HideInInspector] public MatchFinder matchFinder;
        [HideInInspector] public RoundManager roundManager;


        void Awake()
        {
            matchFinder = FindObjectOfType<MatchFinder>();
            roundManager = FindObjectOfType<RoundManager>();
            _uiManager = FindObjectOfType<UIManager>();
            _boardLayout = GetComponent<BoardLayout>();
        }

        void Start()
        {
            allGems = new Gem[width, height];
            _layoutStore = new Gem[width, height];
            Setup();
        }

        void Update()
        {
            //matchFinder.FindAllMatches();

            if (Input.GetKeyDown(KeyCode.S)) 
            {
                ShuffleBoard();
            }
        }

        void Setup()
        {
            if (_boardLayout != null)
            {
                _layoutStore = _boardLayout.GetLayout();
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector2 position = new Vector2(x, y);
                    GameObject bgTile = Instantiate(_bgTilePrefab, position, Quaternion.identity);
                    // Make sure all our tiles are under the board GameObject
                    bgTile.transform.parent = transform;
                    bgTile.name = $"BG Tile - {x},{y}";

                    if (_layoutStore[x, y] != null)
                    {
                        SpawnGem(new Vector2Int(x, y), _layoutStore[x, y]);
                    }
                    else
                    {
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
        }

        void SpawnGem(Vector2Int position, Gem gemToSpawn)
        {
            if (Random.Range(0f, 100f) < bombChance)
            {
                gemToSpawn = bomb;
            }

            Gem gem = Instantiate(gemToSpawn, new Vector3(position.x, position.y + height, 0), Quaternion.identity);
            gem.transform.parent = transform;
            gem.name = $"Gem {position.x}, {position.y}";
            allGems[position.x, position.y] = gem;

            gem.SetupGem(position, this);
        }

        bool MatchesAt(Vector2Int posToCheck, Gem gemToCheck)
        {
            if (posToCheck.x > 1)
            {
                if (allGems[posToCheck.x - 1, posToCheck.y].type == gemToCheck.type && allGems[posToCheck.x - 2, posToCheck.y].type == gemToCheck.type)
                {
                    return true;
                }
            }

            if (posToCheck.y > 1)
            {
                if (allGems[posToCheck.x, posToCheck.y - 1].type == gemToCheck.type && allGems[posToCheck.x, posToCheck.y - 2].type == gemToCheck.type)
                {
                    return true;
                }
            }

            return false;
        }

        void DestroyMatchedGemAt(Vector2Int position)
        {
            if (allGems[position.x, position.y] != null)
            {
                if (allGems[position.x, position.y].isMatched)
                {
                    if (allGems[position.x, position.y].type == Gem.GemType.bomb)
                    {
                        SfxManager.instance.PlayExplodeSound();
                    }
                    else if (allGems[position.x, position.y].type == Gem.GemType.stone)
                    {
                        SfxManager.instance.PlayStoneSound();
                    }
                    else
                    {
                        SfxManager.instance.PlayGemSound();
                    }
                    Instantiate(allGems[position.x, position.y].destroyEffect, new Vector2(position.x, position.y), Quaternion.identity);
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
                    ScoreCheck(matchFinder.currentMatches[i]);
                    DestroyMatchedGemAt(matchFinder.currentMatches[i].positionIndex);
                }
            }
            StartCoroutine(DescreaseRowCoroutine());
        }

        IEnumerator DescreaseRowCoroutine()
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

        IEnumerator FillBoardCoroutine()
        {
            yield return new WaitForSeconds(.1f);
            RefillBoard();

            yield return new WaitForSeconds(.1f);
            matchFinder.FindAllMatches();
            if (matchFinder.currentMatches.Count > 0)
            {
                _bonusMultiplier++;
                yield return new WaitForSeconds(.1f);
                DestroyMatches();
                _uiManager.multiplierText.text = _bonusMultiplier.ToString();
            }
            else
            {
                yield return new WaitForSeconds(.1f);
                currentState = BoardState.Move;
                _bonusMultiplier = 0;
                _uiManager.multiplierText.text = _bonusMultiplier.ToString();
            }

        }

        void RefillBoard()
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

        void CheckMisplacedGems()
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

        public void ShuffleBoard()
        {
            if (currentState != BoardState.Wait)
            {
                currentState = BoardState.Wait;
                List<Gem> gemsFromBoard = new List<Gem>();
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        gemsFromBoard.Add(allGems[x, y]);
                        allGems[x, y] = null;
                    }
                }

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        int gemToUse = Random.Range(0, gemsFromBoard.Count);
                        int iterations = 0;
                        while(MatchesAt(new Vector2Int(x, y), gemsFromBoard[gemToUse]) && iterations < 100 && gemsFromBoard.Count > 1)
                        {
                            gemToUse = Random.Range(0, gemsFromBoard.Count);
                            iterations++;
                        }
                        gemsFromBoard[gemToUse].SetupGem(new Vector2Int(x, y), this);
                        allGems[x, y] = gemsFromBoard[gemToUse];
                        gemsFromBoard.RemoveAt(gemToUse);
                    }
                }
                StartCoroutine(FillBoardCoroutine());
            }
        }

        public void ScoreCheck(Gem gemToCheck)
        {
            roundManager.currentScore += gemToCheck.scoreValue;

            if (_bonusMultiplier > 0)
            {
                float bonusToAdd = gemToCheck.scoreValue * _bonusMultiplier * bonusAmount;
                roundManager.currentScore += Mathf.RoundToInt(bonusToAdd);
            }
        }
    }
}
