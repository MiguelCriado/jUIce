using System;
using UnityEngine;

namespace Juice
{
	[ExecuteAlways]
	public class TransitionFeeder : MonoBehaviour
	{
		[SerializeField] private Widget target = default;
		[SerializeField] private ComponentTransition showTransition = default;
		[SerializeField] private ComponentTransition hideTransition = default;

		private void OnValidate()
		{
			EnsureTargetIsSet();
			FeedTransitions();
		}

		private void Awake()
		{
			EnsureTargetIsSet();
			FeedTransitions();
		}

#if UNITY_EDITOR
		private void Update()
		{
			if (Application.isPlaying == false && !target)
			{
				EnsureTargetIsSet();
				FeedTransitions();
			}
		}
#endif

		private void EnsureTargetIsSet()
		{
			if (!target)
			{
				target = GetComponent<Widget>();
			}
		}
		
		private void FeedTransitions()
		{
			if (target)
			{
				target.ShowTransition = showTransition;
				target.HideTransition = hideTransition;
			}
			else if (Application.isPlaying)
			{
				Debug.LogError($"A {nameof(Widget)} is required to feed the transitions.");
			}
		}
	}
}
