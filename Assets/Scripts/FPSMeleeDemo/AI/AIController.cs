using System.Collections;
using System.Collections.Generic;
using FPSMeleeDemo.BattleSystem;
using UnityEngine;

namespace FPSMeleeDemo.AI
{
	public class AIController : MonoBehaviour, IBattleObject
	{
		private bool _isBehaviourInitialized = false;

		[SerializeField]
		private AIBehaviour _aiBehaviour;
		public AIBehaviour AIBehaviour
		{
			get => _aiBehaviour;
			private set
			{
				_isBehaviourInitialized = false;

				if (_aiBehaviour) _aiBehaviour.End();
				_aiBehaviour = value;

				if (!_aiBehaviour) return;

				InitializeBehaviour();
			}
		}

		public BattleInstance BattleInstance { get; set; }

		public void SetBehaviourFromTemplate(AIBehaviour template)
		{
			var inst = Instantiate(template);
			AIBehaviour = inst;
		}

		private void InitializeBehaviour()
		{
			_aiBehaviour.Assign(this);
			_aiBehaviour.BattleInstance = BattleInstance;
			_aiBehaviour.Begin();

			_isBehaviourInitialized = true;
		}

		private void Start()
		{
			if (_aiBehaviour && !_isBehaviourInitialized)
			{
				InitializeBehaviour();
			}
		}

		private void Update()
		{
			if (AIBehaviour)
			{
				AIBehaviour.Update();
			}
		}
	}
}
