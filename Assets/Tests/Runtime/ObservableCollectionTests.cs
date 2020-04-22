using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Muui.Tests
{
	public class ObservableCollectionTests
	{
		[UnityTest]
		public IEnumerator Constructor_CreatingAnEmptyCollection_SetsThatCollectionEmpty()
		{
			ObservableCollection<int> collection = new ObservableCollection<int>();

			Assert.IsEmpty(collection);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Constructor_CreatingCollectionWithListArgument_NewCollectionHasTheSameSize()
		{
			List<int> list = new List<int>() {1, 2, 3};

			ObservableCollection<int> collection = new ObservableCollection<int>(list);

			Assert.IsTrue(collection.Count == 3);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Constructor_CreatingCollectionWithListArgument_NewCollectionIsCopied()
		{
			List<int> list = new List<int>() {1, 2, 3};

			ObservableCollection<int> collection = new ObservableCollection<int>(list);

			Assert.IsTrue(collection[0] == 1 && collection[1] == 2 && collection[2] == 3);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Add_AddingAnElement_CollectionCountIncreases()
		{
			ObservableCollection<int> collection = new ObservableCollection<int>();

			collection.Add(1);

			Assert.IsTrue(collection.Count == 1);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Add_AddingAnElement_FirstElementIsAddedElement()
		{
			ObservableCollection<int> collection = new ObservableCollection<int>();

			collection.Add(14);

			Assert.IsTrue(collection[0] == 14);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Move_MoveAnItem_ItemIsInCorrectIndex()
		{
			ObservableCollection<int> collection = new ObservableCollection<int>() {0, 1, 2};

			collection.Move(0, 2);

			Assert.IsTrue(collection[2] == 0);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Move_MoveAnItem_ItemIsNotInOldIndex()
		{
			ObservableCollection<int> collection = new ObservableCollection<int>() {0, 1, 2};

			collection.Move(0, 2);

			Assert.IsFalse(collection[0] == 0);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Move_MoveAnItem_CountIsPreserved()
		{
			ObservableCollection<int> collection = new ObservableCollection<int>() {0, 1, 2};

			collection.Move(0, 2);

			Assert.IsTrue(collection.Count == 3);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Remove_RemovingAnItem_CountIsDecreased()
		{
			ObservableCollection<int> collection = new ObservableCollection<int>() {0, 1, 2};

			collection.RemoveAt(0);

			Assert.IsTrue(collection.Count == 2);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Remove_RemovingAnItem_ItemIsNotInItsIndex()
		{
			ObservableCollection<int> collection = new ObservableCollection<int>() {0, 1, 2};

			collection.RemoveAt(1);

			Assert.IsFalse(collection[1] == 1);

			yield return null;
		}

		[UnityTest]
		public IEnumerator Set_SettingAnItem_ValueIsChanged()
		{
			ObservableCollection<int> collection = new ObservableCollection<int>() {0, 1, 2};

			collection[1] = 14;

			Assert.IsTrue(collection[1] == 14);

			yield return null;
		}

		[UnityTest]
		public IEnumerator OnAdd_AddingAnElement_OnAddIsRaised()
		{
			ObservableCollection<int> collection = new ObservableCollection<int>();
			bool isCallbackRaised = false;
			collection.OnAdd += (index, value) => isCallbackRaised = true;

			collection.Add(14);

			Assert.IsTrue(isCallbackRaised);

			yield return null;
		}

		[UnityTest]
		public IEnumerator OnAdd_AddingAnElement_OnAddIsRaisedJustOnce()
		{
			ObservableCollection<int> collection = new ObservableCollection<int>();
			int callbackCount = 0;
			collection.OnAdd += (index, value) => callbackCount++;

			collection.Add(14);

			Assert.IsTrue(callbackCount == 1);

			yield return null;
		}

		[UnityTest]
		public IEnumerator OnAdd_AddingAnElement_OnAddNotifiesCorrectIndex()
		{
			ObservableCollection<int> collection = new ObservableCollection<int>();
			int callbackIndex = -1;
			collection.OnAdd += (index, value) => callbackIndex = index;

			collection.Add(14);

			Assert.IsTrue(callbackIndex == 0);

			yield return null;
		}

		[UnityTest]
		public IEnumerator OnAdd_AddingAnElement_OnAddNotifiesCorrectValue()
		{
			ObservableCollection<int> collection = new ObservableCollection<int>();
			int callbackValue = -1;
			collection.OnAdd += (index, value) => callbackValue = value;

			collection.Add(14);

			Assert.IsTrue(callbackValue == 14);

			yield return null;
		}

		[UnityTest]
		public IEnumerator OnAdd_InsertAnElement_OnAddIsRaised()
		{
			ObservableCollection<int> collection = new ObservableCollection<int>() {0, 1, 2};
			int callbackCount = 0;
			collection.OnAdd += (index, value) => callbackCount++;

			collection.Insert(1, 14);

			Assert.IsTrue(callbackCount == 1);

			yield return null;
		}

		[UnityTest]
		public IEnumerator OnCountChange_AddAnItem_OnCountChangeIsRaised()
		{
			ObservableCollection<int> collection = new ObservableCollection<int>() {0, 1, 2};
			int callbackCount = 0;
			collection.OnCountChange += (count, newCount) => callbackCount++;

			collection.Add(3);

			Assert.IsTrue(callbackCount == 1);

			yield return null;
		}

		[UnityTest]
		public IEnumerator OnCountChange_RemoveAnItem_OnCountChangeIsRaised()
		{
			ObservableCollection<int> collection = new ObservableCollection<int>() {0, 1, 2};
			int callbackCount = 0;
			collection.OnCountChange += (count, newCount) => callbackCount++;

			collection.Remove(0);

			Assert.IsTrue(callbackCount == 1);

			yield return null;
		}

		[UnityTest]
		public IEnumerator OnRemove_RemoveAnItem_OnRemoveIsRaised()
		{
			ObservableCollection<int> collection = new ObservableCollection<int>() {0, 1, 2};
			int callbackCount = 0;
			collection.OnRemove += (index, value) => callbackCount++;

			collection.Remove(0);

			Assert.IsTrue(callbackCount == 1);

			yield return null;
		}

		[UnityTest]
		public IEnumerator OnMove_MoveAnItem_OnMoveIsRaised()
		{
			ObservableCollection<int> collection = new ObservableCollection<int>() {0, 1, 2};
			int callbackCount = 0;
			collection.OnMove += (index, newIndex, value) => callbackCount++;

			collection.Move(0, 2);

			Assert.IsTrue(callbackCount == 1);

			yield return null;
		}

		[UnityTest]
		public IEnumerator OnReplace_SetAValue_OnReplaceIsRaised()
		{
			ObservableCollection<int> collection = new ObservableCollection<int>() {0, 1, 2};
			int callbackCount = 0;
			collection.OnReplace += (index, value, newValue) => callbackCount++;

			collection[0] = 2;

			Assert.IsTrue(callbackCount == 1);

			yield return null;
		}

		[UnityTest]
		public IEnumerator OnReset_ClearCollection_OnResetIsRaised()
		{
			ObservableCollection<int> collection = new ObservableCollection<int>() {0, 1, 2};
			int callbackCount = 0;
			collection.OnReset += () => callbackCount++;

			collection.Clear();

			Assert.IsTrue(callbackCount == 1);

			yield return null;
		}
	}
}
