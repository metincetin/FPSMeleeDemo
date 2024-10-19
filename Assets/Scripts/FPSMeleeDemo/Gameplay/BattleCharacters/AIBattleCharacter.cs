using System.Collections;
using System.Collections.Generic;
using FPSMeleeDemo.BattleSystem;
using UnityEngine;

namespace FPSMeleeDemo.Gameplay.BattleCharacters
{
	public class AIBattleCharacter : MonoBehaviour, IBattleCharacter, IBattleObject
	{
		public Attributes Attributes { get; private set; } = new Attributes();
		public BattleInstance BattleInstance { get; set; }

		public void Update()
		{
			var target = BattleInstance.GetOther(this);

			if (target is not MonoBehaviour mb) return;

			var dir = mb.transform.position - transform.position;
			dir.y = 0;
			dir.Normalize();

			var rot = Quaternion.LookRotation(dir);

			transform.rotation = rot;
		}
	}
}
