using System.Collections.Generic;

namespace Juice
{
	public interface IWindow : IView
	{
		bool HideOnForegroundLost { get; }
		bool IsPopup { get; }
		bool CloseOnShadowClick { get; }
		WindowPriority WindowPriority { get; }

		void SetLayer(WindowLayer layer);
		IViewModel GetNewViewModel();
		void SetPayload(Dictionary<string, object> payload);
		bool GetFromPayload<T>(string key, out T value);
		bool RemoveFromPayload(string key);
	}
}
