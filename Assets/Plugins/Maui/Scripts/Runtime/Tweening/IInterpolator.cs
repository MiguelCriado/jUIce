namespace Maui.Tweening
{
	public interface IInterpolator<T>
	{
		void SetEase(Ease ease);
		T Evaluate(T a, T b, float t);
	}
}