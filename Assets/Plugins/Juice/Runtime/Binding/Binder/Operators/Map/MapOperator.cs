using Juice.Collections;
using UnityEngine;

namespace Juice
{
	public abstract class MapOperator<TFrom, TTo> : ToOperator<TFrom, TTo>
	{
		[SerializeField] private SerializableDictionary<TFrom, TTo> mapper = new SerializableDictionary<TFrom, TTo>();
		[SerializeField] private ConstantBindingInfo<TTo> fallback = new ConstantBindingInfo<TTo>();

		private SerializableDictionary<TFrom, TTo> Mapper => mapper;
		private ConstantBindingInfo Fallback => fallback;

		private VariableBinding<TTo> fallbackBinding;

		protected override void Awake()
		{
			base.Awake();

			fallbackBinding = new VariableBinding<TTo>(Fallback, this);
		}

		protected override void OnEnable()
		{
			fallbackBinding.Bind();
			base.OnEnable();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			fallbackBinding.Unbind();
		}

		protected override TTo Convert(TFrom value)
		{
			if (Mapper.TryGetValue(value, out var result) == false)
			{
				result = fallbackBinding.Property.Value;
			}

			return result;
		}
	}
}
