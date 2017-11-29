namespace Apex.UnitySurvivalShooter
{
    using Apex.AI;

    public sealed class EmptyAction : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            //This action does nothing.
            //This only serves the purpose of displaying an Idle action for this example.
            //Normally there is no reason to have actions that do nothing as a Qualifier with no action does exactly that.
        }
    }
}