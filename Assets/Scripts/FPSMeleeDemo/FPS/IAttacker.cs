using FPSMeleeDemo.Data;
using UnityEngine;

namespace FPSMeleeDemo.FPS
{
	public interface IAttacker
	{
		public BlockHandler BlockHandler { get; }
		public void Attack(Vector2 direction);
		public void SetBlockState(bool value);
		public AttackAnimationHandler AnimationHandler {get;set;}
	}
}

