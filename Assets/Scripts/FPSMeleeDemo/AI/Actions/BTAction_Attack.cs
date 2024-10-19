using FPSMeleeDemo.AI.BT;
using FPSMeleeDemo.FPS;
using UnityEngine;

namespace FPSMeleeDemo.AI.Actions
{
    public class BTAction_Attack : BTAction
    {
        private float _lastAttack;
        public float AttackRate = 1.4f;

        protected override NodeStatus OnRun()
        {
            if (Time.time - _lastAttack < AttackRate) return NodeStatus.Running;
            
            _lastAttack = Time.time;
            
            Attacker.Attack(Vector2.right);
            
			return NodeStatus.Running;
        }

        public IAttacker Attacker { get; set; }
        public Transform Target { get; set; }
    }
}


