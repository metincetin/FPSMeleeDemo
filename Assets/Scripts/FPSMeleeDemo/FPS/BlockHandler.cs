using System;
using FPSMeleeDemo.Gameplay;
using UnityEngine;

namespace FPSMeleeDemo.FPS
{
    public class BlockHandler
	{
		public float BlockStartTime { get; private set; }
		public bool IsBlocking { get; private set; }

		private float _blockPower = MaxBlockPower;
		private const float MaxBlockPower = 5;

		public event Action<bool> BlockStateChanged;

		public void BeginBlock()
		{
			if (IsBlocking) return;
			IsBlocking = true;
			_blockPower = MaxBlockPower;

			BlockStateChanged?.Invoke(true);
		}

		public void EndBlock()
		{
			IsBlocking = false;
			BlockStateChanged?.Invoke(false);
		}

		public void DepleteDamage(DamageObject damage)
		{
			var diff = damage.Damage - _blockPower;

			if (diff > 0)
			{
				_blockPower -= damage.Damage;
			}
			else
			{
				_blockPower = 0;
			}

			damage.Damage = Mathf.Max(0, Mathf.Max(0, diff));
		}
	}
}

