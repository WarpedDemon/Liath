namespace Apex.UnitySurvivalShooter
{
    using AI;
    using Apex.Serialization;

    public abstract class CustomScorer<T> : OptionScorerBase<T>
    {
        [ApexSerialization, FriendlyName("Score", "How much this scorer can score at maximum")]
        public float score = 0f;
    }
}