using System;
using FluentAssertions;
using NUnit.Framework;

namespace Juice.Tests
{
	public class BindingUtilsTests
	{
		[Test]
		public void CanBeBound_VariableWithReferenceType_ReturnTrue()
		{
			Type actualType = typeof(ObservableVariable<string>);
			Type targetType = typeof(IReadOnlyObservableVariable<string>);

			bool canBeBound = BindingUtils.CanBeBound(actualType, targetType);

			canBeBound.Should().BeTrue();
		}

		[Test]
		public void CanBeBound_VariableWithBuiltInValueType_ReturnTrue()
		{
			Type actualType = typeof(ObservableVariable<int>);
			Type targetType = typeof(IReadOnlyObservableVariable<int>);

			bool canBeBound = BindingUtils.CanBeBound(actualType, targetType);

			canBeBound.Should().BeTrue();
		}

		[Test]
		public void CanBeBound_RawTypeToObservableVariable_ReturnFalse()
		{
			Type actualType = typeof(int);
			Type targetType = typeof(IReadOnlyObservableVariable<int>);

			bool canBeBound = BindingUtils.CanBeBound(actualType, targetType);

			canBeBound.Should().BeFalse();
		}

		[Test]
		public void CanBeBound_CollectionWithReferenceType_ReturnTrue()
		{
			Type actualType = typeof(ObservableCollection<string>);
			Type targetType = typeof(IReadOnlyObservableCollection<string>);

			bool canBeBound = BindingUtils.CanBeBound(actualType, targetType);

			canBeBound.Should().BeTrue();
		}

		[Test]
		public void CanBeBound_CollectionWithBuiltInValueType_ReturnTrue()
		{
			Type actualType = typeof(ObservableCollection<int>);
			Type targetType = typeof(IReadOnlyObservableCollection<int>);

			bool canBeBound = BindingUtils.CanBeBound(actualType, targetType);

			canBeBound.Should().BeTrue();
		}

		[Test]
		public void CanBeBound_RawTypeToObservableCollection_ReturnFalse()
		{
			Type actualType = typeof(int);
			Type targetType = typeof(IReadOnlyObservableCollection<int>);

			bool canBeBound = BindingUtils.CanBeBound(actualType, targetType);

			canBeBound.Should().BeFalse();
		}

		[Test]
		public void CanBeBound_CommandWithReferenceType_ReturnTrue()
		{
			Type actualType = typeof(ObservableCommand<string>);
			Type targetType = typeof(IObservableCommand<string>);

			bool canBeBound = BindingUtils.CanBeBound(actualType, targetType);

			canBeBound.Should().BeTrue();
		}

		[Test]
		public void CanBeBound_CommandWithBuiltInValueType_ReturnTrue()
		{
			Type actualType = typeof(ObservableCommand<int>);
			Type targetType = typeof(IObservableCommand<int>);

			bool canBeBound = BindingUtils.CanBeBound(actualType, targetType);

			canBeBound.Should().BeTrue();
		}

		[Test]
		public void CanBeBound_CommandWithoutType_ReturnTrue()
		{
			Type actualType = typeof(ObservableCommand);
			Type targetType = typeof(IObservableCommand);

			bool canBeBound = BindingUtils.CanBeBound(actualType, targetType);

			canBeBound.Should().BeTrue();
		}

		[Test]
		public void CanBeBound_RawTypeToObservableCommand_ReturnFalse()
		{
			Type actualType = typeof(int);
			Type targetType = typeof(IObservableCommand<int>);

			bool canBeBound = BindingUtils.CanBeBound(actualType, targetType);

			canBeBound.Should().BeFalse();
		}

		[Test]
		public void CanBeBound_EventWithReferenceType_ReturnTrue()
		{
			Type actualType = typeof(ObservableEvent<string>);
			Type targetType = typeof(IObservableEvent<string>);

			bool canBeBound = BindingUtils.CanBeBound(actualType, targetType);

			canBeBound.Should().BeTrue();
		}

		[Test]
		public void CanBeBound_EventWithBuiltInValueType_ReturnTrue()
		{
			Type actualType = typeof(ObservableEvent<int>);
			Type targetType = typeof(IObservableEvent<int>);

			bool canBeBound = BindingUtils.CanBeBound(actualType, targetType);

			canBeBound.Should().BeTrue();
		}

		[Test]
		public void CanBeBound_EventWithoutType_ReturnTrue()
		{
			Type actualType = typeof(ObservableEvent);
			Type targetType = typeof(IObservableEvent);

			bool canBeBound = BindingUtils.CanBeBound(actualType, targetType);

			canBeBound.Should().BeTrue();
		}

		[Test]
		public void CanBeBound_RawTypeToObservableEvent_ReturnFalse()
		{
			Type actualType = typeof(int);
			Type targetType = typeof(IObservableEvent<int>);

			bool canBeBound = BindingUtils.CanBeBound(actualType, targetType);

			canBeBound.Should().BeFalse();
		}

		[Test]
		public void NeedsToBeBoxed_VariableWithBuiltInValueTypeToObject_ReturnTrue()
		{
			Type actualType = typeof(ObservableVariable<int>);
			Type targetType = typeof(IReadOnlyObservableVariable<object>);

			bool needsToBeBoxed = BindingUtils.NeedsToBeBoxed(actualType, targetType);

			needsToBeBoxed.Should().BeTrue();
		}

		[Test]
		public void NeedsToBeBoxed_VariableWithReferenceTypeToObject_ReturnFalse()
		{
			Type actualType = typeof(ObservableVariable<string>);
			Type targetType = typeof(IReadOnlyObservableVariable<object>);

			bool needsToBeBoxed = BindingUtils.NeedsToBeBoxed(actualType, targetType);

			needsToBeBoxed.Should().BeFalse();
		}

		[Test]
		public void NeedsToBeBoxed_CollectionWithBuiltInValueTypeToObject_ReturnTrue()
		{
			Type actualType = typeof(ObservableCollection<int>);
			Type targetType = typeof(IReadOnlyObservableCollection<object>);

			bool needsToBeBoxed = BindingUtils.NeedsToBeBoxed(actualType, targetType);

			needsToBeBoxed.Should().BeTrue();
		}

		[Test]
		public void NeedsToBeBoxed_CollectionWithReferenceTypeToObject_ReturnFalse()
		{
			Type actualType = typeof(ObservableCollection<string>);
			Type targetType = typeof(IReadOnlyObservableCollection<object>);

			bool needsToBeBoxed = BindingUtils.NeedsToBeBoxed(actualType, targetType);

			needsToBeBoxed.Should().BeFalse();
		}

		[Test]
		public void NeedsToBeBoxed_CommandWithBuiltInValueTypeToObject_ReturnTrue()
		{
			Type actualType = typeof(ObservableCommand<int>);
			Type targetType = typeof(IObservableCommand<object>);

			bool needsToBeBoxed = BindingUtils.NeedsToBeBoxed(actualType, targetType);

			needsToBeBoxed.Should().BeTrue();
		}

		[Test]
		public void NeedsToBeBoxed_CommandWithReferenceTypeToObject_ReturnFalse()
		{
			Type actualType = typeof(ObservableCommand<string>);
			Type targetType = typeof(IObservableCommand<object>);

			bool needsToBeBoxed = BindingUtils.NeedsToBeBoxed(actualType, targetType);

			needsToBeBoxed.Should().BeFalse();
		}

		[Test]
		public void NeedsToBeBoxed_EventWithBuiltInValueTypeToObject_ReturnTrue()
		{
			Type actualType = typeof(ObservableEvent<int>);
			Type targetType = typeof(IObservableEvent<object>);

			bool needsToBeBoxed = BindingUtils.NeedsToBeBoxed(actualType, targetType);

			needsToBeBoxed.Should().BeTrue();
		}

		[Test]
		public void NeedsToBeBoxed_EventWithReferenceTypeToObject_ReturnFalse()
		{
			Type actualType = typeof(ObservableEvent<string>);
			Type targetType = typeof(IObservableEvent<object>);

			bool needsToBeBoxed = BindingUtils.NeedsToBeBoxed(actualType, targetType);

			needsToBeBoxed.Should().BeFalse();
		}
	}
}
