using System;
using FPSMeleeDemo.FPS;
using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

namespace FPSMeleeDemo.Montage.Playables
{
	[Serializable]
	public class DamageActivatorPlayableAsset : PlayableAsset
	{
		// Factory method that generates a playable based on this asset
		public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
		{
			var playable = ScriptPlayable<DamageActivatorPlayableBehaviour>.Create(graph);
			var behaviour = playable.GetBehaviour();

			behaviour.Attacker = go.GetComponent<IAttacker>();

			return playable;
		}
	}


	public class DamageActivatorPlayableBehaviour : PlayableBehaviour
	{
		public IAttacker Attacker;

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
				if (Attacker is IWeaponInstanceContainer container)
				{
					var damageArea = container.WeaponInstance.GetComponentInChildren<DamageArea>();
					damageArea.End();
				}
			}
		}

		public override void OnBehaviourPlay(Playable playable, FrameData info)
		{
#if UNITY_EDITOR
			if (!Application.isPlaying) return;
#endif
			if (Attacker is IWeaponInstanceContainer container)
			{
				var damageArea = container.WeaponInstance.GetComponentInChildren<DamageArea>();
				damageArea.Begin();
			}
		}
	}
}
