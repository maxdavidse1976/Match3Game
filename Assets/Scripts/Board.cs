using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private GameObject _bgTilePrefab;

    [SerializeField] private Gem[] _gems;

    private Gem[,] _allGems;

    void Start()
    {
        _allGems = new Gem[_width, _height];
        Setup();
    }

    private void Setup()
    {
        for(int x = 0; x < _width; x++)
        {
            for(int y = 0; y < _height; y++)
            {
                Vector2 position = new Vector2(x, y);
                GameObject bgTile = Instantiate(_bgTilePrefab, position, Quaternion.identity);
                // Make sure all our tiles are under the board GameObject
                bgTile.transform.parent = transform;
                bgTile.name = $"BG Tile - {x},{y}";

                int gemToUse = Random.Range(0, _gems.Length);
                SpawnGem(new Vector2Int(x, y), _gems[gemToUse]);
            }
        }
    }

    private void SpawnGem(Vector2Int position, Gem gemToSpawn)
    {
        Gem gem = Instantiate(gemToSpawn, new Vector3(position.x, position.y, 0), Quaternion.identity);
        gem.transform.parent = transform;
        gem.name = $"Gem {position.x}, {position.y}";
        _allGems[position.x, position.y] = gem;

        gem.SetupGem(position, this);
    }
}
