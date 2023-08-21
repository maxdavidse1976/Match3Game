using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace DSG.Match3
{
    public class UIManager : MonoBehaviour
    {
        Board _theBoard;

        public TMP_Text timeText;
        public TMP_Text scoreText;

        public TMP_Text multiplierText;

        public TMP_Text winScore;
        public TMP_Text winText;

        public GameObject winStarsOne;
        public GameObject winStarsTwo;
        public GameObject winStarsThree;
        public GameObject roundOverScreen;
        public GameObject pauseScreen;

        [SerializeField] string _levelSelect;

        void Awake()
        {
            _theBoard = FindFirstObjectByType<Board>();
        }

        void Start()
        {
            winStarsOne.SetActive(false);
            winStarsTwo.SetActive(false);
            winStarsThree.SetActive(false);
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseUnPause();
            }
        }

        public void PauseUnPause()
        {
            if (!pauseScreen.activeInHierarchy)
            {
                pauseScreen.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                pauseScreen.SetActive(false);
                Time.timeScale = 1f;
            }
        }

        public void ShuffleBoard()
        {
            _theBoard.ShuffleBoard();
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void GoToLevelSelect()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(_levelSelect);
        }

        public void TryAgain()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}