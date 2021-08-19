using UnityEngine;

namespace Juice
{
    public class ColorDecorateCommandOperator : DecorateCommandOperator<Color>
    {
        protected override ConstantBindingInfo<Color> DecorationBindingInfo => decorationBindingInfo;

        [SerializeField] private ConstantBindingInfo<Color> decorationBindingInfo = new ConstantBindingInfo<Color>();
    }
}
