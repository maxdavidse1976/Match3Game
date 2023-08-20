using UnityEngine;

namespace DSG.Match3
{
    public class BoardLayout : MonoBehaviour
    {
        [SerializeField] LayoutRow[] _allRows;

        public Gem[,] GetLayout()
        {
            Gem[,] theLayout = new Gem[_allRows[0].gemsInRow.Length, _allRows.Length];

            for(int y = 0; y < _allRows.Length; y++)
            {
                for (int x = 0; x < _allRows[y].gemsInRow.Length; x++)
                {
                    if (x < theLayout.GetLength(0))
                    {
                        if (_allRows[y].gemsInRow[x] != null)
                        {
                            theLayout[x, _allRows.Length - 1 - y] = _allRows[y].gemsInRow[x];
                        }
                    }
                }
            }
            return theLayout;
        }

    }

    [System.Serializable]
    public class LayoutRow
    {
        public Gem[] gemsInRow;
    }
}
