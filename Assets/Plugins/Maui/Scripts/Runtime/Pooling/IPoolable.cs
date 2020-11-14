namespace Maui.Pooling
{
	public interface IPoolable
	{
		void OnSpawn();
		void OnRecycle();
	}
}
