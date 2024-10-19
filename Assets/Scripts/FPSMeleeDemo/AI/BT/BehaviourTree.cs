using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSMeleeDemo.AI.BT
{
	public enum NodeStatus
	{
		Running,
		Fail,
		Success,
	}

	public abstract class BTDecorator
	{
		public bool Invert { get; }
		protected abstract bool ShouldRun { get; }
		public bool Test => Invert ? !ShouldRun : ShouldRun;
	}

	public abstract class BTNode
	{
		public BTDecorator[] Decorators { get; set; }

		public virtual void Enter() { }
		public virtual void Exit() { }
		
		public BTNode(params BTDecorator[] decorators)
		{
			Decorators = decorators;
		}

		public bool GetDecoratorResult()
		{
			if (Decorators == null) return true;
			foreach (var d in Decorators)
			{
				if (!d.Test) return false;
			}
			return true;
		}

		public NodeStatus Run()
		{
			if (!GetDecoratorResult()) return NodeStatus.Fail;


			return OnRun();
		}
		protected abstract NodeStatus OnRun();
	}

	public class BTLog : BTAction
	{
		public string Message = "Hello, world";

		public override void Enter()
		{
			Debug.Log("BTLog Enter");
		}

		public override void Exit()
		{
			Debug.Log("BTLog Exit");
		}

		protected override NodeStatus OnRun()
		{
			Debug.Log(Message);
			return NodeStatus.Success;
		}
	}

	public class BTWait : BTAction
	{
		public float Duration;
		private float _startTime;

		public override void Enter()
		{
			_startTime = Time.time;
		}

		protected override NodeStatus OnRun()
		{
			if (Time.time - _startTime > Duration) return NodeStatus.Success;
			return NodeStatus.Running;
		}
	}
	
	public class BTParallel : BTComposite
	{
		public BTParallel(params BTNode[] nodes) : base(nodes){}
		protected override NodeStatus OnRun()
		{
			foreach (BTNode node in _traverser.Iterate())
			{
				node.Run();
			}
			return NodeStatus.Running;
		}
	}

	public class BTSequence : BTComposite
	{
		public BTSequence(params BTNode[] nodes): base(nodes){}

		protected override NodeStatus OnRun()
		{
			var result = Current.Run();
			if (result == NodeStatus.Success)
			{
				if (_traverser.HasNext) Traverse();
				else return NodeStatus.Success;
			}

			return result;
		}
	}
	public class BTSelector : BTComposite
	{
		public BTSelector (params BTNode[] nodes): base(nodes){}

		protected override NodeStatus OnRun()
		{
			var result = Current.Run();
			if (result != NodeStatus.Running && !HasNext) return result;

			if (result == NodeStatus.Fail)
			{
				Traverse();
			}

			return result;
		}
	}

	public abstract class BTComposite : BTNode, INodeTraverser
	{
		protected NodeTraverser _traverser;

		public BTComposite(params BTNode[] nodes)
		{
			_traverser = new NodeTraverser(nodes);
		}

		public override void Enter()
		{
			_traverser.Begin();
		}


		public BTNode Current => _traverser.Current;

		public bool HasNext => _traverser.HasNext;

		public void Restart()
		{
			_traverser.Restart();
		}

		public void Traverse()
		{
			_traverser.Traverse();
		}
	}

	public abstract class BTAction : BTNode
	{
		protected BTAction(params BTDecorator[] decorators) : base(decorators){}
	}

	public interface INodeTraverser
	{
		public BTNode Current { get; }

		public bool HasNext { get; }
		public void Traverse();
		public void Restart();
	}

	public class NodeTraverser : INodeTraverser
	{
		private BTNode[] _nodes;

		private int _index;

		public NodeTraverser(params BTNode[] nodes)
		{
			_nodes = nodes;
			_index = 0;
		}

		public void Begin()
		{
			Current.Enter();
		}

		public BTNode Current => _nodes[_index];

		public bool HasNext => _index < _nodes.Length - 1;

		public void Restart()
		{
			Current.Exit();
			_index = 0;
			Current.Enter();
		}

		public void Traverse()
		{
			var old = Current;
			_index = (_index + 1) % _nodes.Length;

			if (old != null && old != Current)
			{
				old.Exit();
			}

			Current.Enter();
		}

        public void End()
        {
			Current.Exit();
        }

        public IEnumerable<BTNode> Iterate()
        {
	        return _nodes;
        }
	}


	// minimalistic behaviour tree implementation
	public class BehaviourTree : INodeTraverser
	{
		private NodeTraverser _traverser;

		public BTNode Current => _traverser.Current;

		public bool HasNext => _traverser.HasNext;

		public bool HasBegun { get; private set; }

		public BehaviourTree(params BTNode[] nodes)
		{
			_traverser = new NodeTraverser(nodes);
		}

		public void Begin()
		{
			_traverser.Begin();

			HasBegun = true;
		}
		public void End()
		{
			_traverser.End();

			HasBegun = false;
		}

		public void Restart()
		{
			_traverser.Restart();
			_traverser.Begin();
		}

		public void Update()
		{
			if (!HasBegun) return;
			var result = _traverser.Current.Run();
			switch (result)
			{
				case NodeStatus.Fail:
					Restart();
					break;
				case NodeStatus.Success:
					Traverse();
					break;
			}
		}

		public void Traverse()
		{
			_traverser.Traverse();
		}
	}
}
