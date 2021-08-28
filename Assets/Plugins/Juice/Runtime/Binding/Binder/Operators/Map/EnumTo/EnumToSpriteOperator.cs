using System;
using Juice.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Juice
{
    public class EnumToSpriteOperator<T> : MapOperator<T, Sprite> where T : Enum
    {
    }
}
