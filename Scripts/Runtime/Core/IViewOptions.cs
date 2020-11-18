namespace Juice
{
	public interface IViewOptions
	{
		Transition InTransition { get; }
		Transition OutTransition { get; }
	}
}