namespace Apex.UnitySurvivalShooter
{
    using UnityEngine;
    using UnityEngine.UI;

    public class BandAidUI : MonoBehaviour
    {
        private Text _text;

        private void Awake()
        {
            _text = GetComponentInChildren<Text>();
            SetBandAids(0);
        }

        public void SetBandAids(int currentBandAids)
        {
            _text.text = currentBandAids.ToString();
        }
    }
}