using UnityEngine;

namespace Juice
{
    public class IntDecorateCommandOperator : DecorateCommandOperator<int>
    {
        protected override ConstantBindingInfo<int> DecorationBindingInfo => decorationBindingInfo;

        [SerializeField] private ConstantBindingInfo<int> decorationBindingInfo = new ConstantBindingInfo<int>();
    }
}
