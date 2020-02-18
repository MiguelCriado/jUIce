using System.Threading.Tasks;
using UnityEngine;

namespace Muui
{
#pragma warning disable 0649
	public abstract class BasePresenter<T> : MonoBehaviour where T : IScreenController
	{
		[SerializeField] private T screenPrefab;

		protected UIFrame uiFrame;

		public virtual void Initialize(UIFrame uiFrame)
		{
			this.uiFrame = uiFrame;
			uiFrame.RegisterScreen(screenPrefab);
		}

		public virtual void Terminate()
		{
			uiFrame.DisposeScreen(screenPrefab);
		}

		protected virtual Task ShowScreen()
		{
			return uiFrame.ShowScreen<T>();
		}

		protected virtual Task HideScreen()
		{
			return uiFrame.HideScreen<T>();
		}
	}

	public abstract class BasePresenter<T1, T2> : BasePresenter<T1>
		where T1 : BaseScreenController<T2>
		where T2 : IScreenProperties
	{
		protected virtual Task ShowScreen(T2 properties)
		{
			return uiFrame.ShowScreen<T1, T2>(properties);
		}
	}
#pragma warning restore 0649
}
