using UnityEngine;

namespace Juice
{
    public class Vector3DecorateCommandOperator : DecorateCommandOperator<Vector3>
    {
        protected override ConstantBindingInfo<Vector3> DecorationBindingInfo => decorationBindingInfo;

        [SerializeField] private ConstantBindingInfo<Vector3> decorationBindingInfo = new ConstantBindingInfo<Vector3>();
    }
}
