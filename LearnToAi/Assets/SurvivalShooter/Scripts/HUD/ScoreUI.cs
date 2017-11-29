namespace Apex.UnitySurvivalShooter
{
    using UnityEngine;
    using UnityEngine.UI;

    public class ScoreUI : MonoBehaviour
    {
        private Text _text;
        private int _score;

        private void Awake()
        {
            _text = GetComponent<Text>();
            _score = 0;
            AddScore(0);
        }

        public void AddScore(int score)
        {
            _score += score;
            _text.text = "Score: " + _score;
        }
    }
}