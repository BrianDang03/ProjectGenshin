using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    [Serializable]
    public class PlayerWalkData
    {
        [field: SerializeField][field: Range(0, 1f)] public float speedModifier { get; private set; } = 0.225f;
    }
}
