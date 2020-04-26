using NUnit.Framework;

namespace Muui.Tests
{
	public class ObservableCommandTests
	{
		#region [Non Generic]

		[Test]
		public void Constructor_WithNoParameters_CanExecuteIsNotNull()
		{
			ObservableCommand command = new ObservableCommand();

			Assert.IsNotNull(command.CanExecute);
		}

		[Test]
		public void Constructor_WithNoParameters_CanExecuteValueIsTrue()
		{
			ObservableCommand command = new ObservableCommand();

			Assert.IsTrue(command.CanExecute.Value);
		}

		[Test]
		public void Constructor_WithCanExecuteParameter_CanExecuteIsNotNull()
		{
			ObservableVariable<bool> canExecute = new ObservableVariable<bool>();

			ObservableCommand command = new ObservableCommand(canExecute);

			Assert.IsNotNull(command.CanExecute);
		}

		[Test]
		public void Constructor_WithCanExecuteParameter_CanExecuteValueIsTrue()
		{
			ObservableVariable<bool> canExecute = new ObservableVariable<bool>(true);

			ObservableCommand command = new ObservableCommand(canExecute);

			Assert.IsTrue(command.CanExecute.Value);
		}

		[Test]
		public void Constructor_WithCallbackParameter_OnRequestExecuteEventIsNotNull()
		{
			int callbackCount = 0;
			ObservableCommand command = new ObservableCommand(() => callbackCount++);

			command.Execute();

			Assert.IsTrue(callbackCount == 1);
		}

		[Test]
		public void Execute_WhenCanExecute_RaiseEvent()
		{
			ObservableCommand command = new ObservableCommand();
			int callbackCount = 0;
			command.ExecuteRequested += () => callbackCount++;

			command.Execute();

			Assert.IsTrue(callbackCount == 1);
		}

		[Test]
		public void Execute_WhenCanNotExecute_DoNotRaiseEvent()
		{
			ObservableVariable<bool> canExecute = new ObservableVariable<bool>();
			ObservableCommand command = new ObservableCommand(canExecute);
			canExecute.Value = false;
			int callbackCount = 0;
			command.ExecuteRequested += () => callbackCount++;

			command.Execute();

			Assert.IsTrue(callbackCount == 0);
		}

		#endregion

		#region [Generic]

				[Test]
		public void Generic_Constructor_WithNoParameters_CanExecuteIsNotNull()
		{
			ObservableCommand<bool> command = new ObservableCommand<bool>();

			Assert.IsNotNull(command.CanExecute);
		}

		[Test]
		public void Generic_Constructor_WithNoParameters_CanExecuteValueIsTrue()
		{
			ObservableCommand<int> command = new ObservableCommand<int>();

			Assert.IsTrue(command.CanExecute.Value);
		}

		[Test]
		public void Generic_Constructor_WithCanExecuteParameter_CanExecuteIsNotNull()
		{
			ObservableVariable<bool> canExecute = new ObservableVariable<bool>();

			ObservableCommand<int> command = new ObservableCommand<int>(canExecute);

			Assert.IsNotNull(command.CanExecute);
		}

		[Test]
		public void Generic_Constructor_WithCanExecuteParameter_CanExecuteValueIsTrue()
		{
			ObservableVariable<bool> canExecute = new ObservableVariable<bool>(true);

			ObservableCommand<int> command = new ObservableCommand<int>(canExecute);

			Assert.IsTrue(command.CanExecute.Value);
		}

		[Test]
		public void Generic_Constructor_WithCallbackParameter_OnRequestExecuteEventIsNotNull()
		{
			int callbackCount = 0;
			ObservableCommand<int> command = new ObservableCommand<int>((value) => callbackCount++);

			command.Execute(14);

			Assert.IsTrue(callbackCount == 1);
		}

		[Test]
		public void Generic_Execute_WhenCanExecute_RaiseEvent()
		{
			ObservableCommand<int> command = new ObservableCommand<int>();
			int callbackCount = 0;
			command.ExecuteRequested += (int value) => callbackCount++;

			command.Execute(14);

			Assert.IsTrue(callbackCount == 1);
		}

		[Test]
		public void Generic_Execute_WhenCanNotExecute_DoNotRaiseEvent()
		{
			ObservableVariable<bool> canExecute = new ObservableVariable<bool>(true);
			ObservableCommand<int> command = new ObservableCommand<int>(canExecute);
			canExecute.Value = false;
			int callbackCount = 0;
			command.ExecuteRequested += (int value) => callbackCount++;

			command.Execute(14);

			Assert.IsTrue(callbackCount == 0);
		}

		[Test]
		public void Generic_Execute_WhenCanExecute_ParameterIsReceived()
		{
			ObservableVariable<bool> canExecute = new ObservableVariable<bool>(true);
			ObservableCommand<int> command = new ObservableCommand<int>(canExecute);
			int mirrorVariable = -1;
			command.ExecuteRequested += (int value) => mirrorVariable = value;

			command.Execute(14);

			Assert.IsTrue(mirrorVariable == 14);
		}

		#endregion
	}
}
