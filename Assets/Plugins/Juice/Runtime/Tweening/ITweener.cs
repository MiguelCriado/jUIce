namespace Juice.Tweening
{
	public delegate void TweenerIdChangedHandler(Tweener tweener, object lastId, object newId);
	public delegate void TweenerLifecycleEventHandler(Tweener tweener);
	
	public interface ITweener
	{
		event TweenerIdChangedHandler IdChanged;
		event TweenerLifecycleEventHandler Completed;
		event TweenerLifecycleEventHandler Killed;
		
		object Id { get; set; }
		bool IsPlaying { get; }
	}
}