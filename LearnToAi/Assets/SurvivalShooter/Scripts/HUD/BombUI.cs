namespace Apex.UnitySurvivalShooter
{
    using UnityEngine;
    using UnityEngine.UI;

    public class BombUI : MonoBehaviour
    {
        private Text _text;

        private void Awake()
        {
            _text = GetComponentInChildren<Text>();
            SetBombs(0);
        }

        public void SetBombs(int currentBombs)
        {
            _text.text = currentBombs.ToString();
        }
    }
}