using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    [CreateAssetMenu(fileName = "Player", menuName = "Custom/Characters/Player")]
    public class PlayerSO : ScriptableObject
    {
        //Initializes the data  
        [field: SerializeField] public PlayerGroundedData groundedData { get; private set; }
    }
}
