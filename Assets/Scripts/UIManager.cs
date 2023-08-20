using UnityEngine;
using TMPro;

namespace DSG.Match3
{
    public class UIManager : MonoBehaviour
    {
        public TMP_Text timeText;
        public TMP_Text scoreText;

        public TMP_Text multiplierText;

        public TMP_Text winScore;
        public TMP_Text winText;

        public GameObject winStarsOne;
        public GameObject winStarsTwo;
        public GameObject winStarsThree;
        public GameObject roundOverScreen;

        // Start is called before the first frame update
        void Start()
        {
            winStarsOne.SetActive(false);
            winStarsTwo.SetActive(false);
            winStarsThree.SetActive(false);
        }
    }
}