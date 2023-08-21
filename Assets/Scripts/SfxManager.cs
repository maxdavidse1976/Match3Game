using UnityEngine;

namespace DSG.Match3
{
    public class SfxManager : MonoBehaviour
    {
        [SerializeField] AudioSource _gemSound;
        [SerializeField] AudioSource _explodeSound;
        [SerializeField] AudioSource _stoneSound;
        [SerializeField] AudioSource _roundOverSound;

        public static SfxManager instance;

        void Awake()
        {
            instance = this;
        }

        public void PlayGemSound()
        {
            _gemSound.Stop();
            _gemSound.pitch = Random.Range(.8f, 1.2f);
            _gemSound.Play();
        }

        public void PlayExplodeSound()
        {
            _explodeSound.Stop();
            _explodeSound.pitch = Random.Range(.8f, 1.2f);
            _explodeSound.Play();
        }

        public void PlayStoneSound()
        {
            _stoneSound.Stop();
            _stoneSound.pitch = Random.Range(.8f, 1.2f);
            _stoneSound.Play();
        }

        public void PlayRoundOverSound()
        {
            _roundOverSound.Play();
        }
    }
}
