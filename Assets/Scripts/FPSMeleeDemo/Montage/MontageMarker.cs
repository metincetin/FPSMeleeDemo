using UnityEngine;
using UnityEngine.Timeline;

namespace FPSMeleeDemo.Montage
{
    public abstract class MontageMarker: Marker
	{
		public abstract void Notify(GameObject gameObject, MontagePlayer montagePlayer);
	}
}

