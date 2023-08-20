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

        void Start()
        {
            _star1.SetActive(false);
            _star2.SetActive(false);
            _star3.SetActive(false);

            if (PlayerPrefs.HasKey(_levelToLoad + "_Star1"))
            {
                _star1.SetActive(true);
            }
            if (PlayerPrefs.HasKey(_levelToLoad + "_Star2"))
            {
                _star2.SetActive(true);
            }
            if (PlayerPrefs.HasKey(_levelToLoad + "_Star3"))
            {
                _star3.SetActive(true);
            }

        }
        public void LoadLevel()
        {
            SceneManager.LoadScene(_levelToLoad);
        }
    }
}
