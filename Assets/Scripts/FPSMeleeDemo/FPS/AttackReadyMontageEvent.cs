using FPSMeleeDemo.Montage;
using UnityEngine;
using UnityEngine.Playables;

namespace FPSMeleeDemo.FPS
{
    public class AttackReadyMontageEvent : MontageMarker, INotification
    {
        public PropertyName id => 0;

        public override void Notify(GameObject gameObject, MontagePlayer montagePlayer)
        {
			Debug.Log("Notify");
			montagePlayer.InvokeEvent(this, null);
        }
    }
}

