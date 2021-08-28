using System;

namespace Juice
{
    public abstract class EnumToBoolOperator<T> : MapOperator<T, bool> where T : Enum
    {
    }
}
