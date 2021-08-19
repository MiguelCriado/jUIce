using System.Threading.Tasks;
using UnityEngine;

namespace Juice
{
	public class TransitionPicker : ITransition
	{
		public string SelectedTransition { get; }

		private ComponentTransition transition;
		
		public TransitionPicker(string selectedTransition)
		{
			SelectedTransition = selectedTransition;
		}
		
		public void Prepare(RectTransform target)
		{
			var options = target.GetComponent<TransitionPickerOptions>();

			if (options)
			{
				if (options.TryGetTransition(SelectedTransition, out transition))
				{
					transition.Prepare(target);
				}
				else 
				{
					Debug.LogError($"Selected transition \"{SelectedTransition}\" not found in {nameof(TransitionPickerOptions)} component", target);
				}
			}
			else
			{
				Debug.LogError($"{target.name} must have a {nameof(TransitionPickerOptions)} component attached", target);
			}
		}

		public async Task Animate(RectTransform target)
		{
			if (transition)
			{
				await transition.Animate(target);
			}
		}

		public void Cleanup(RectTransform target)
		{
			if (transition)
			{
				transition.Cleanup(target);
			}
		}
	}
}