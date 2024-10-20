using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSMeleeDemo.Gameplay.Tests
{
    public class GenericDamageReceiver : MonoBehaviour, IDamageReceiver
    {
        public event Action<DamageObject> DamageReceived;

        public void ApplyDamage(DamageObject damage)
        {
        }
    }
}
