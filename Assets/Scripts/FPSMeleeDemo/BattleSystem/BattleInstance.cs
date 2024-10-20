using System;
using FPSMeleeDemo.Gameplay;
using UnityEngine;

namespace FPSMeleeDemo.BattleSystem
{

    public class BattleInstance
	{
		IBattleCharacter[] _characters;

		public BattleInstance(params IBattleCharacter[] characters)
		{
			_characters = characters;
			AssignDependencies();
		}

        public IBattleCharacter GetOther(IBattleCharacter battleCharacter)
        {
			foreach(var b in _characters)
			{
				if (b != battleCharacter) return b;
			}
			return null;
        }

        private void AssignDependencies()
		{
			foreach(var c in _characters)
			{
				var objects = (c as MonoBehaviour).GetComponentsInChildren<IBattleObject>();
				foreach(var o in objects)
				{
					o.BattleInstance = this;
				}
			}
		}
	}
}

