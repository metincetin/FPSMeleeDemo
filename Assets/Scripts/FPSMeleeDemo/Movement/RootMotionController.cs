using System;
using UnityEditor;
using UnityEngine;

namespace FPSMeleeDemo.Movement
{
	public class RootMotionController : MonoBehaviour
	{
		private Animator _animator;
		public Vector3 Velocity { get; private set; }
		public Vector3 AngularVelocity { get; private set; }

		private void Awake()
		{
			_animator = GetComponent<Animator>();
		}

		private void OnAnimatorMove()
		{
			Velocity = _animator.velocity;
			AngularVelocity = _animator.angularVelocity;
		}

#if UNITY_EDITOR
		[CustomEditor(typeof(RootMotionController))]
		public class RootMotionControllerEditor : Editor
		{
			public override void OnInspectorGUI()
			{
				base.OnInspectorGUI();

				if (Application.isPlaying)
				{
					EditorGUI.BeginDisabledGroup(true);

					var t = target as RootMotionController;
					EditorGUILayout.Vector3Field("Velocity", t.Velocity);
					EditorGUILayout.Vector3Field("Angular Velocity", t.AngularVelocity);

					EditorGUI.EndDisabledGroup();
				}
			}
		}
#endif
	}
}
