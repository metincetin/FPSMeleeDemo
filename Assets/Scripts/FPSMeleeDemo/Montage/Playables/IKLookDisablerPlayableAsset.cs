using System;
using FPSMeleeDemo.Rigging;
using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

namespace FPSMeleeDemo.Montage.Playables
{
	[Serializable]
	public class IKLookDisablerPlayableAsset : PlayableAsset
	{
		// Factory method that generates a playable based on this asset
		
		public ExposedReference<IKTargetLook> IKLook;
		
		public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
		{
			var playable = ScriptPlayable<IKLookDisablerPlayableBehaviour>.Create(graph);
			var behaviour = playable.GetBehaviour();

			behaviour.IKLook = IKLook.Resolve(graph.GetResolver());

			return playable;
		}
	}
	
	public class IKLookDisablerPlayableBehaviour : PlayableBehaviour
	{
		public IKTargetLook IKLook;

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
				IKLook.enabled = true;
			}
		}

		public override void OnBehaviourPlay(Playable playable, FrameData info)
		{
#if UNITY_EDITOR
			if (!Application.isPlaying) return;
#endif
			IKLook.enabled = false;
		}
	}

}