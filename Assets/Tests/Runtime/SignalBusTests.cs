using System.Collections;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Maui.Tests
{
	public class SignalBusTests
	{
		private class TestSignal : ISignal
		{

		}

		[Test]
		public void Default_AccessingDefaultInstance_DefaultInstanceIsCreated()
		{
			SignalBus.Default.Should().NotBeNull();
		}

		[Test]
		public void Default_AccessingDefaultInstanceMultipleTimes_InstanceIsTheSame()
		{
			SignalBus busA = SignalBus.Default;
			SignalBus busB = SignalBus.Default;

			busA.Should().BeSameAs(busB);
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
