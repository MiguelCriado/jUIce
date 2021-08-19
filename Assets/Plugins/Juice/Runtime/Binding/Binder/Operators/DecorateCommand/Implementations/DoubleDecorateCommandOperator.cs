using UnityEngine;

namespace Juice
{
    public class DoubleDecorateCommandOperator : DecorateCommandOperator<double>
    {
        protected override ConstantBindingInfo<double> DecorationBindingInfo => decorationBindingInfo;

        [SerializeField] private ConstantBindingInfo<double> decorationBindingInfo = new ConstantBindingInfo<double>();
    }
}
