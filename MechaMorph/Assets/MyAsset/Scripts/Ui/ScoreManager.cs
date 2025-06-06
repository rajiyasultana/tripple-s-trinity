using TMPro;
using TrippleTrinity.MechaMorph.MyAsset.Scripts.SaveManager;
using TrippleTrinity.MechaMorph.SaveManager;
using UnityEngine;

namespace TrippleTrinity.MechaMorph.MyAsset.Scripts.Ui
{
    public class ScoreManager : MonoBehaviour
    {
        private static ScoreManager _instance;
       
        private int _score;
        private const string HighScoreKey = "HighScore";
        private int _highscore;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI highScoreText;
        public static ScoreManager Instance
        {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("ScoreManager instance is null.");
            }
            return _instance;
        }
        }
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            LoadScore();
            UpdateScoreUI();
        }

        public void AddScore(int points)
        {
            if (points < 0)
            {
                Debug.LogWarning("Cannot add negative points.");
                return;
            }

            _score += points;

            // Check if new score is higher than previous high score
            if (_score > _highscore)
            {
                _highscore = _score;
                SaveScore(); //save highscore
            }

            UpdateScoreUI();
        }

        private void UpdateScoreUI()
        {
            if (scoreText != null)
            {
                scoreText.text = $"<color=#004DFF>Score: {_score:D2}</color>";
            }

            if (highScoreText != null)
            {
                highScoreText.text = $"Highest Score: {_highscore:D2}</color>";
            }
        }


        

        public int CurrentScore()
        {
            return _score;
        }

        public void LoadScore()
        {
            GameData data = SaveSystem.LoadGame();
            _score = 0;

            if (data != null)
            {
                _highscore = data.highScore; //Load high score from JSON
            }
            else
            {

                _highscore = 0;
            }

            UpdateScoreUI();
        }

        public void SaveScore()
        {
            if(_score > _highscore)
            {
                _highscore = _score;
            }

            GameData data = new GameData
            {
                highScore = _highscore,
                tokenCount = TokenUIManager.Instance.CurrentTokenCount()
            };

            SaveSystem.SaveGame(data);
            Debug.Log("Score Saved Successfully.");
        }

        public void ResetScore()
        {
            _score = 0;
        }
    }
}
