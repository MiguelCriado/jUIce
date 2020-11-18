namespace Juice
{
	public class WindowOptions : IViewOptions
	{
		public Transition InTransition { get; set; }
		public Transition OutTransition { get; set; }
		public WindowPriority? Priority { get; set; }
	}
}