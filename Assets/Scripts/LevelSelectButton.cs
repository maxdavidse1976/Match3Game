using UnityEngine;
using UnityEngine.SceneManagement;

namespace DSG.Match3
{

    public class LevelSelectButton : MonoBehaviour
    {
        [SerializeField] string _levelToLoad;
        [SerializeField] GameObject _star1;
        [SerializeField] GameObject _star2;
        [SerializeField] GameObject _star3;

        public void LoadLevel()
        {
            SceneManager.LoadScene(_levelToLoad);
        }
    }
}
