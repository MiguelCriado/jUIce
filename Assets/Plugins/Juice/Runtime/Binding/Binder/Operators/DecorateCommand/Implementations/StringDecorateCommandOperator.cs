using UnityEngine;

namespace Juice
{
    public class StringDecorateCommandOperator : DecorateCommandOperator<string>
    {
        protected override ConstantBindingInfo<string> DecorationBindingInfo => decorationBindingInfo;

        [SerializeField] private ConstantBindingInfo<string> decorationBindingInfo = new ConstantBindingInfo<string>();
    }
}
