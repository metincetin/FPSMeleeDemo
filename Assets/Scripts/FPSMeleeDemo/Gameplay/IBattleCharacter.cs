namespace FPSMeleeDemo.Gameplay
{

    public interface IBattleCharacter: IDamageReceiver
	{
		public Attributes Attributes { get; }
	}
}
