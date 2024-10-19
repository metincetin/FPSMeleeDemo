namespace FPSMeleeDemo.AI.BT
{
	public interface INodeTraverser
	{
		public BTNode Current { get; }

		public bool HasNext { get; }
		public void Traverse();
		public void Restart();
	}
}