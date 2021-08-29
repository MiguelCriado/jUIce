namespace Juice
{
    public class WindowTransitionData : TransitionData
    {
        public IWindow TargetWindow { get; }

        public WindowTransitionData(IWindow targetWindow)
        {
            TargetWindow = targetWindow;
        }
    }
}