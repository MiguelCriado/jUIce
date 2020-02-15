using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Muui.Tests
{
	public class SignalBusTests : MonoBehaviour
	{
		private class TestSignal : ISignal
		{
			public int IntValue;
			public bool BoolValue;
		}

		[UnityTest]
		public IEnumerable Default_AccessingDefaultInstance_DefaultInstanceIsCreated()
		{
			Assert.IsNotNull(SignalBus.Default);

			yield return null;
		}

		[UnityTest]
		public IEnumerable Default_AccessingDefaultInstanceMultipleTimes_InstanceIsTheSame()
		{
			SignalBus busA = SignalBus.Default;
			SignalBus busB = SignalBus.Default;

			Assert.IsTrue(busA.Equals(busB));

			yield return null;
		}

		[UnityTest]
		public IEnumerable Subscribe_WhenSignalFired_ExecutesCallback()
		{
			SignalBus bus = new SignalBus();
			int callbackCount = 0;
			bus.Subscribe((TestSignal signal) => { callbackCount++; });

			bus.Fire(new TestSignal());

			Assert.IsTrue(callbackCount == 1);

			yield return null;
		}

		[UnityTest]
		public IEnumerable Subscribe_WhenSignalFired_ExecutesSeveralCallbacks()
		{
			SignalBus bus = new SignalBus();
			int firstCallbackCount = 0;
			int secondCallbackCount = 0;
			bus.Subscribe((TestSignal signal) => { firstCallbackCount++; });
			bus.Subscribe((TestSignal signal) => { secondCallbackCount++; });

			bus.Fire(new TestSignal());

			Assert.IsTrue(firstCallbackCount == 1 && secondCallbackCount == 1);

			yield return null;
		}

		[UnityTest]
		public IEnumerable Unsubscribe_WhenSignalFiredAfterUnsubscribe_CallbackIsNotExecuted()
		{
			SignalBus bus = new SignalBus();
			int callbackCount = 0;

			void OnSignalFired(TestSignal signal)
			{
				callbackCount++;
			}

			bus.Subscribe<TestSignal>(OnSignalFired);
			bus.Unsubscribe<TestSignal>(OnSignalFired);

			bus.Fire(new TestSignal());

			Assert.IsTrue(callbackCount == 0);

			yield return null;
		}

		[UnityTest]
		public IEnumerable Fire_WhenFiringSignalWithNoSubscription_DoNotThrowException()
		{
			SignalBus bus = new SignalBus();

			Assert.DoesNotThrow(() => bus.Fire(new TestSignal()));

			yield return null;
		}
	}
}
