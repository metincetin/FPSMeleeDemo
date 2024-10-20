using System;
using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

namespace FPSMeleeDemo.Montage.Playables
{
	[Serializable]
	public class AudioPlayerPlayableAsset : PlayableAsset
	{
		// Factory method that generates a playable based on this asset
		
		public ExposedReference<AudioSource> AudioSource;
		public AudioClip[] Clips;
		public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
		{
			var playable = ScriptPlayable<AudioPlayerPlayableBehaviour>.Create(graph);
			var behaviour = playable.GetBehaviour();

			behaviour.AudioSource = AudioSource.Resolve(graph.GetResolver());
			behaviour.Clips = Clips;

			return playable;
		}
	}
	
	public class AudioPlayerPlayableBehaviour : PlayableBehaviour
	{
		public AudioSource AudioSource;
		public AudioClip[] Clips;

		public override void OnBehaviourPlay(Playable playable, FrameData info)
		{
			var clip = Clips[Random.Range(0, Clips.Length)];
			AudioSource.PlayOneShot(clip);
		}
	}

}