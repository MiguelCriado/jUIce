using System;

namespace Maui.Tweening
{
	public interface ITweener
	{
		event Action Completed;
		bool IsPlaying { get; }
	}
}