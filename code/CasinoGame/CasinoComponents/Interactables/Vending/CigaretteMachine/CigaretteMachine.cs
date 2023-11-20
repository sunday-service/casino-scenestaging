using Sandbox;

namespace Casino;

public class CigaretteMachine : BaseComponent, IInteractable
{
	[Property] uint CigaretteCost { get; set; } = 20;
	public void Interact(GameObject player)
	{
		if(player.GetComponent<PlayerMoney>() is PlayerMoney money) 
		{
			Log.Info( "Buying Cigarettes" );

			money.TakeMoney( CigaretteCost );
		}
		
	}
}
