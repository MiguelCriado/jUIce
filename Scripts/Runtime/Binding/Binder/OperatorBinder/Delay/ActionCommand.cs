using System;

namespace Juice
{
	public class ActionCommand
	{
		public float DueTime { get; }
		
		private Action action;

		public ActionCommand(Action action, float dueTime)
		{
			this.action = action;
			DueTime = dueTime;
		}

		public void Execute()
		{
			action?.Invoke();
		}
	}
}