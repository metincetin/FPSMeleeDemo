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
		private const float MaxBlockPower = 35;

		public float RemainingBlockRate => _blockPower / MaxBlockPower;

		public event Action<bool> BlockStateChanged;

		public void AddBlockPower(float power)
		{
			_blockPower = Mathf.Min(MaxBlockPower, _blockPower + power);
		}

		public void BeginBlock()
		{
			if (IsBlocking) return;
			IsBlocking = true;
			_blockPower = MaxBlockPower;

			BlockStartTime = Time.time;

			BlockStateChanged?.Invoke(true);
		}

		public void EndBlock()
		{
			IsBlocking = false;
			BlockStateChanged?.Invoke(false);
		}

		public void DepleteDamage(DamageObject damage)
		{
			var diff =  _blockPower - damage.Damage;

			if (diff > 0)
			{
				_blockPower -= damage.Damage;
				damage.Damage = 0;
			}
			else
			{
				damage.Damage -= _blockPower;
				_blockPower = 0;
			}
		}
	}
}

