using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    [Serializable]
    public class PlayerDashData
    {
        [field: SerializeField][field: Range(1f, 3f)] public float speedModifier { get; private set; } = 2f;
        [field: SerializeField][field: Range(1f, 2f)] public float TimeToBeConsideredConsecutive { get; private set; } = 1f;
        [field: SerializeField][field: Range(1, 10)] public int consecutiveDashesLimitAmount { get; private set; } = 2;
        [field: SerializeField][field: Range(1f, 5f)] public float DashLimitReachedCooldown { get; private set; } = 1.75f;

    }
}
