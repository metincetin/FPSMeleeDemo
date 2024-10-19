
using System.Collections;
using System.Collections.Generic;
using FPSMeleeDemo.Gameplay;
using UnityEngine;

namespace FPSMeleeDemo.BattleSystem
{
    public class BattleController : MonoBehaviour
	{
		public BattleInstance CurrentBattle { get; private set; }

		public void CreateBattle(BattleSettings settings)
		{
			CurrentBattle = new BattleInstance(settings.Characters);
		}
	}
}
