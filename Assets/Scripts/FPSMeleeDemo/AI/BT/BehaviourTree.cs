using System;
using System.Collections;

namespace FPSMeleeDemo.AI.BT
{
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
