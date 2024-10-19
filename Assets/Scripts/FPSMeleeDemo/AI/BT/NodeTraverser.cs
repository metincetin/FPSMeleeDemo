using System.Collections.Generic;

namespace FPSMeleeDemo.AI.BT
{
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
}