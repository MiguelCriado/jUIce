using System;
using UnityEngine;

namespace Juice
{
    public abstract class EnumToColorOperator<T> : MapOperator<T, Color> where T : Enum
    {
    }
}
