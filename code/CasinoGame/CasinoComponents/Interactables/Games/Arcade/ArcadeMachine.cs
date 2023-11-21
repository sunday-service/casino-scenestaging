using Sandbox;

namespace Casino;

public class ArcadeMachine : BaseComponent, IInteractable
{
	[Property] uint ArcadeCost { get; set; } = 20;
	public void Interact(GameObject player)
	{
		if(player.GetComponent<PlayerMoney>() is PlayerMoney money) 
		{
			Log.Info( "Putting money into the arcade machine..." );
			money.TakeMoney( ArcadeCost );
		}
		
	}
}
