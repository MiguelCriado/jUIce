using UnityEngine;

namespace Juice
{
    public class Vector2DecorateCommandOperator : DecorateCommandOperator<Vector2>
    {
        protected override ConstantBindingInfo<Vector2> DecorationBindingInfo => decorationBindingInfo;

        [SerializeField] private ConstantBindingInfo<Vector2> decorationBindingInfo = new ConstantBindingInfo<Vector2>();

    }
}
