namespace Juice.Pooling
{
	public interface IPoolable
	{
		void OnSpawn();
		void OnRecycle();
	}
}
