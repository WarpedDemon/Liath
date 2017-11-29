namespace Apex.UnitySurvivalShooter
{
    using UnityEngine;
    using UnityEngine.UI;

    public class AmmoUI : MonoBehaviour
    {
        private Text _text;                      

        private void Awake()
        {
            _text = GetComponentInChildren<Text>();
            SetAmmo(0);
        }

        public void SetAmmo(int currentAmmo)
        {
            _text.text = currentAmmo.ToString();
        }
    }
}