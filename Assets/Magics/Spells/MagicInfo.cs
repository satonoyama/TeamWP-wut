using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicalFX
{
    public class MagicInfo : MonoBehaviour
    {
        [SerializeField] float delay;
        [SerializeField] bool burstfire;
        [Header("Status")]
        [SerializeField] float firePower;
        [SerializeField] float manaCost;
        [Header("Other Effects")]
        [SerializeField] float otherEffectDelay;

        public float Delay { get {return delay;} }
        public float Power { get { return firePower; } }
        public float ManaCost { get { return manaCost; } }
        public float OtherEffectDelay { get {return otherEffectDelay;} }
    }
}
