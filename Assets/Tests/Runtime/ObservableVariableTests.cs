using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Muui.Tests
{
	public class ObservableVariableTests
	{
		private int intMirrorVariable;
		private int callbackCount;

		[SetUp]
		public void Setup()
		{
			intMirrorVariable = 0;
			callbackCount = 0;
		}

		[TearDown]
		public void TearDown()
		{

		}

		[UnityTest]
		public IEnumerator Value_WhenSetAValue_KeepsThatValue()
		{
			ObservableVariable<int> variable = new ObservableVariable<int>();

			variable.Value = 14;

			Assert.IsTrue(variable.Value == 14);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Value_WhenSetTheSameValue_DontRaiseCallback()
		{
			ObservableVariable<int> variable = new ObservableVariable<int>();

			variable.Value = 14;

			Assert.IsTrue(variable.Value == 14);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Subscribe_WhenValueChange_CallCallback()
		{
			ObservableVariable<int> variable = new ObservableVariable<int>();
			variable.Changed += CopyIntVariable;

			variable.Value = 14;

			Assert.IsTrue(intMirrorVariable == 14);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Subscribe_WhenSubscription_NoCallbackIsRaised()
		{
			ObservableVariable<int> variable = new ObservableVariable<int>();

			variable.Changed += IncrementCallbackCount;

			Assert.IsTrue(callbackCount == 0);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Subscribe_WhenValueChange_OnlyOneCallCallbackIsRaised()
		{
			ObservableVariable<int> variable = new ObservableVariable<int>();
			variable.Changed += IncrementCallbackCount;

			variable.Value = 42;

			Assert.IsTrue(callbackCount == 1);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Unsubscribe_WhenValueChange_DontCallCallback()
		{
			ObservableVariable<int> variable = new ObservableVariable<int>();
			variable.Changed += CopyIntVariable;
			variable.Value = 14;

			variable.Changed -= CopyIntVariable;
			variable.Value = 42;

			Assert.IsTrue(intMirrorVariable == 14);

			yield return null;
		}

		private void CopyIntVariable(int newValue)
		{
			intMirrorVariable = newValue;
		}

		private void IncrementCallbackCount(int newValue)
		{
			callbackCount++;
		}
	}
}
