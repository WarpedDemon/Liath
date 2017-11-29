namespace Apex.UnitySurvivalShooter
{
    using UnityEngine;

    /// <summary>
    /// A facade for updating the HUD
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class HUDState : MonoBehaviour
    {
        private static AmmoUI _ammo;
        private static BandAidUI _bandaids;
        private static BombUI _bombs;
        private static HealthUI _health;
        private static ScoreUI _score;

        public static void UpdateAmmo(int currentAmmo)
        {
            _ammo.SetAmmo(currentAmmo);
        }

        public static void UpdateBandAids(int currentBandAids)
        {
            _bandaids.SetBandAids(currentBandAids);
        }

        public static void UpdateBombs(int currentBombs)
        {
            _bombs.SetBombs(currentBombs);
        }

        public static void UpdateHealth(int currentHealth)
        {
            _health.SetHealth(currentHealth);
        }

        public static void UpdateScore(int score)
        {
            _score.AddScore(score);
        }

        private void Awake()
        {
            _ammo = GetComponentInChildren<AmmoUI>();
            _bandaids = GetComponentInChildren<BandAidUI>();
            _bombs = GetComponentInChildren<BombUI>();
            _health = GetComponentInChildren<HealthUI>();
            _score = GetComponentInChildren<ScoreUI>();
        }
    }
}
