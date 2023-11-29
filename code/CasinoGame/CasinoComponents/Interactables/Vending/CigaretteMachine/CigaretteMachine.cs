using Sandbox;

namespace Casino;

public class CigaretteMachine : BaseComponent, IInteractable
{
	[Property] int CigaretteCost { get; set; } = 20;
	public void Interact(GameObject player)
	{
		if(player.Components.Get<PlayerMoney>() is PlayerMoney money) 
		{
			Log.Info( "Buying Cigarettes" );

			money.TakeMoney( CigaretteCost );
		}
		
	}
}
