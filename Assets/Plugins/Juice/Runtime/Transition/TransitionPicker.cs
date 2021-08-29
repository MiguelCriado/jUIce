namespace Juice
{
	public abstract class TransitionPicker : ComponentTransition
	{
		public abstract ComponentTransition DefaultTransition { get; set; }
		public abstract ITransition SelectTransition(TransitionData transitionData);
	}
}