using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Muui.Tests
{
	public class ObservableCommandTests
	{
		[UnityTest]
		public IEnumerator Constructor_WithNoParameters_CanExecuteIsNotNull()
		{
			ObservableCommand command = new ObservableCommand();

			Assert.IsNotNull(command.CanExecute);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Constructor_WithNoParameters_CanExecuteValueIsTrue()
		{
			ObservableCommand command = new ObservableCommand();

			Assert.IsTrue(command.CanExecute.Value);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Constructor_WithParameter_CanExecuteIsNotNull()
		{
			ObservableVariable<bool> canExecute = new ObservableVariable<bool>();

			ObservableCommand command = new ObservableCommand(canExecute);

			Assert.IsNotNull(command.CanExecute);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Constructor_WithParameters_CanExecuteValueIsTrue()
		{
			ObservableVariable<bool> canExecute = new ObservableVariable<bool>(true);

			ObservableCommand command = new ObservableCommand(canExecute);

			Assert.IsTrue(command.CanExecute.Value);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Execute_WhenCanExecute_RaiseEvent()
		{
			ObservableCommand command = new ObservableCommand();
			int callbackCount = 0;
			command.OnRequestExecute += () => callbackCount++;

			command.Execute();

			Assert.IsTrue(callbackCount == 1);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Execute_WhenCanNotExecute_DoNotRaiseEvent()
		{
			ObservableVariable<bool> canExecute = new ObservableVariable<bool>();
			ObservableCommand command = new ObservableCommand(canExecute);
			canExecute.Value = false;
			int callbackCount = 0;
			command.OnRequestExecute += () => callbackCount++;

			command.Execute();

			Assert.IsTrue(callbackCount == 0);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Generics_Constructor_WithNoParameters_CanExecuteIsNotNull()
		{
			ObservableCommand<bool> command = new ObservableCommand<bool>();

			Assert.IsNotNull(command.CanExecute);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Generics_Constructor_WithNoParameters_CanExecuteValueIsTrue()
		{
			ObservableCommand<int> command = new ObservableCommand<int>();

			Assert.IsTrue(command.CanExecute.Value);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Generics_Constructor_WithParameter_CanExecuteIsNotNull()
		{
			ObservableVariable<bool> canExecute = new ObservableVariable<bool>();

			ObservableCommand<int> command = new ObservableCommand<int>(canExecute);

			Assert.IsNotNull(command.CanExecute);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Generics_Constructor_WithParameters_CanExecuteValueIsCopied()
		{
			ObservableVariable<bool> canExecute = new ObservableVariable<bool>(true);

			ObservableCommand<int> command = new ObservableCommand<int>(canExecute);

			Assert.IsTrue(command.CanExecute.Value);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Generics_Execute_WhenCanExecute_RaiseEvent()
		{
			ObservableCommand<int> command = new ObservableCommand<int>();
			int callbackCount = 0;
			command.OnRequestExecute += (int value) => callbackCount++;

			command.Execute(14);

			Assert.IsTrue(callbackCount == 1);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Generics_Execute_WhenCanNotExecute_DoNotRaiseEvent()
		{
			ObservableVariable<bool> canExecute = new ObservableVariable<bool>(true);
			ObservableCommand<int> command = new ObservableCommand<int>(canExecute);
			canExecute.Value = false;
			int callbackCount = 0;
			command.OnRequestExecute += (int value) => callbackCount++;

			command.Execute(14);

			Assert.IsTrue(callbackCount == 0);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Generics_Execute_WhenCanExecute_ParameterIsReceived()
		{
			ObservableVariable<bool> canExecute = new ObservableVariable<bool>(true);
			ObservableCommand<int> command = new ObservableCommand<int>(canExecute);
			int mirrorVariable = -1;
			command.OnRequestExecute += (int value) => mirrorVariable = value;

			command.Execute(14);

			Assert.IsTrue(mirrorVariable == 14);

			yield return null;
		}
	}
}
