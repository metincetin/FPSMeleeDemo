using System;
using FPSMeleeDemo.FPS;
using FPSMeleeDemo.Rigging;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace FPSMeleeDemo.Montage.Playables
{
	[Serializable]
	public class WeaponTrailActivatorPlayableAsset : PlayableAsset
	{
		// Factory method that generates a playable based on this asset
		
		public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
		{
			var playable = ScriptPlayable<WeaponTrailActivatorPlayableBehaviour>.Create(graph);
			var behaviour = playable.GetBehaviour();

			behaviour.Attacker = go.GetComponent<Attacker>();

			return playable;
		}
	}
	
	public class WeaponTrailActivatorPlayableBehaviour : PlayableBehaviour
	{
		public Attacker Attacker;

		public override void OnBehaviourPause(Playable playable, FrameData info)
		{
#if UNITY_EDITOR
			if (!Application.isPlaying) return;
#endif
			var duration = playable.GetDuration();
			var count = playable.GetTime() + info.deltaTime;

			if ((info.effectivePlayState == PlayState.Paused && count > duration) ||
				playable.GetGraph().GetRootPlayable(0).IsDone())
			{
				var trail = Attacker.WeaponInstance.Graphics.GetComponentInChildren<StickWeaponTrailEffect>();
				if (trail)
				{
					trail.emitting = false;
				}
			}
		}

		public override void OnBehaviourPlay(Playable playable, FrameData info)
		{
#if UNITY_EDITOR
			if (!Application.isPlaying) return;
#endif
				var trail = Attacker.WeaponInstance.Graphics.GetComponentInChildren<StickWeaponTrailEffect>();
				if (trail)
				{
					trail.emitting = false;
				}
		}
	}

}