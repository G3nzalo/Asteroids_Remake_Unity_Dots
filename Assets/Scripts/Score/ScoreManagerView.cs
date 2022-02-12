using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Score.View
{
    public class ScoreManagerView : MonoBehaviour
    {
        public static ScoreManagerView instance;

        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] Button btn1;

        public int CurentScore { get; set; }
        public void SetScoreText(int _currentScore) => scoreText.text = "Score: " + _currentScore.ToString();

        private void Awake()
        {
            Initialize();
        }

        void Start() => setupBtn();

        void setupBtn() => btn1.onClick.AddListener(delegate { btnClicked(); });

        void btnClicked() => Application.Quit();

        private void Initialize()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }

    }
}