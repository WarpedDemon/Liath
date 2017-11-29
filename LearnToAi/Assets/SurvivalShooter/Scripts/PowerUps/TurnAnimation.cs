namespace Apex.UnitySurvivalShooter
{
    using UnityEngine;

    public class TurnAnimation : MonoBehaviour
    {
        public Vector3 turn;

        private void FixedUpdate()
        {
            var v = turn * Time.deltaTime;
            this.transform.Rotate(v);
        }
    }
}