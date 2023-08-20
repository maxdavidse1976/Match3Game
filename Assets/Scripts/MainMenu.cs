using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DSG.Match3
{
    public class MainMenu : MonoBehaviour
    {
        public string levelToLoad;

        public void StartGame()
        {
            SceneManager.LoadScene(levelToLoad);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}