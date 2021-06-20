using System;
using Juice.Tweening;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

namespace Juice
{
	public static class AotHelper
	{
		private static readonly bool AlwaysFalse = DateTime.UtcNow.Year < 0;

		public static void Ensure(Action action)
		{
			if (AlwaysFalse)
			{
				try
				{
					action();
				}
				catch (Exception e)
				{
					throw new InvalidOperationException("", e);
				}
			}
		}

		[Preserve]
		public static void EnsureBasicTypes()
		{
			EnsureType<bool>();
			EnsureType<byte>();
			EnsureType<sbyte>();
			EnsureType<char>();
			EnsureType<decimal>();
			EnsureType<double>();
			EnsureType<float>();
			EnsureType<int>();
			EnsureType<uint>();
			EnsureType<long>();
			EnsureType<ulong>();
			EnsureType<short>();
			EnsureType<ushort>();

			EnsureType<DateTime>();
			EnsureType<TimeSpan>();
			EnsureType<Guid>();

			EnsureType<Vector2>();
			EnsureType<Vector2Int>();
			EnsureType<Vector3>();
			EnsureType<Vector3Int>();
			EnsureType<Vector4>();
			EnsureType<Quaternion>();
			EnsureType<Color>();
			EnsureType<Color32>();
			EnsureType<RangeInt>();
			EnsureType<Rect>();
			EnsureType<RectInt>();
			EnsureType<Bounds>();
			EnsureType<BoundsInt>();
			EnsureType<KeyCode>();
			EnsureType<LayerMask>();

			EnsureType<Image.Type>();
			EnsureType<Image.FillMethod>();
			EnsureType<Image.OriginHorizontal>();
			EnsureType<Image.OriginVertical>();
			EnsureType<Image.Origin90>();
			EnsureType<Image.Origin180>();
			EnsureType<Image.Origin360>();

			EnsureType<Ease>();

			throw new InvalidOperationException("This method is used for AOT code generation only. Do not call it at runtime.");
		}

		public static void EnsureType<T>() where T : struct
		{
			EnsureVariable<T>();
			EnsureCollection<T>();
			EnsureCommand<T>();
			EnsureEvent<T>();
		}

		public static void EnsureVariable<T>() where T : struct
		{
			Ensure(() => new VariableBoxer<object, T>(new ObservableVariable<T>()));
		}

		public static void EnsureCollection<T>() where T : struct
		{
			Ensure(() => new CollectionBoxer<object, T>(new ObservableCollection<T>()));
		}

		public static void EnsureCommand<T>() where T : struct
		{
			Ensure(() => new CommandBoxer<object, T>(new ObservableCommand<T>()));
		}

		public static void EnsureEvent<T>() where T : struct
		{
			Ensure(() => new EventBoxer<object, T>(new ObservableEvent<T>()));
		}
	}
}
