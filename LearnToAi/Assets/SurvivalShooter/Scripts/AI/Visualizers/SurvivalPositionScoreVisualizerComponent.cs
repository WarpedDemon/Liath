namespace Apex.UnitySurvivalShooter
{
    using System.Collections.Generic;
    using Apex.AI;
    using Apex.AI.Visualization;
    using UnityEngine;

    public sealed class SurvivalPositionScoreVisualizerComponent : ActionWithOptionsVisualizerComponent<MoveToBestPosition, Vector3>
    {
        protected override IList<Vector3> GetOptions(IAIContext context)
        {
            var ctx = (SurvivalContext)context;
            return ctx.sampledPositions;
        }

        protected override void DrawGUI(IList<ScoredOption<Vector3>> data)
        {
            var cam = Camera.main;
            if (cam == null)
            {
                return;
            }

            foreach (var scoredOption in data)
            {
                var score = scoredOption.score;

                var p = cam.WorldToScreenPoint(scoredOption.option);
                p.y = Screen.height - p.y;

                if (score < 0f)
                {
                    GUI.color = Color.red;
                }
                else if (score == 0f)
                {
                    GUI.color = Color.black;
                }
                else
                {
                    GUI.color = Color.green;
                }

                var content = new GUIContent(score.ToString("F0"));
                var size = new GUIStyle(GUI.skin.label).CalcSize(content);
                GUI.Label(new Rect(p.x, p.y, size.x, size.y), content);
            }
        }

        protected override void DrawGizmos(IList<ScoredOption<Vector3>> data)
        {
            float maxScore = 0f;
            float minScore = Mathf.Infinity;

            foreach (var scoredOption in data)
            {
                var value = scoredOption.score;
                if (value > maxScore)
                {
                    maxScore = value;
                }

                if (value < minScore)
                {
                    minScore = value;
                }
            }

            var diffScore = maxScore - minScore;

            foreach (var scoredOption in data)
            {
                var pos = scoredOption.option;
                var score = scoredOption.score;

                var normScore = score - minScore;

                Gizmos.color = GetColor(normScore, diffScore);
                Gizmos.DrawSphere(pos, 0.25f);
            }
        }

        private static Color GetColor(float score, float maxScore)
        {
            if (maxScore <= 0)
            {
                return Color.green;
            }

            if (score == maxScore)
            {
                return Color.cyan;
            }

            var quotient = score / maxScore;

            return new Color((1 - quotient), quotient, 0, 0.2f);
        }
    }
}