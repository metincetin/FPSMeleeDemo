using FPSMeleeDemo.BattleSystem;
using UnityEngine;

namespace FPSMeleeDemo.AI
{
    public abstract class AIBehaviour : ScriptableObject
	{
		public AIController Owner { get; private set; }
		public BattleInstance BattleInstance { get; set; }

		public void Assign(AIController owner)
		{
			if (Owner) Debug.LogError("AIBehaviour is already assigned", this);
			Owner = owner;
		}

		public abstract void Begin();
		public abstract void End();

		public abstract void Update();
	}
}

