using UnityEngine;
using UnityEngine.SceneManagement;

namespace DSG.Match3
{
    public class LevelSelectMenu : MonoBehaviour
    {
        [SerializeField] string _mainMenu = "MainMenu";

        public void GoToMainMenu()
        {
            SceneManager.LoadScene(_mainMenu);
        }
    }
}
