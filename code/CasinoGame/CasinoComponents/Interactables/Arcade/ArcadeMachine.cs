using Sandbox;

namespace Casino;

public class ArcadeMachine : BaseComponent, IInteractable
{
	[Property] uint ArcadeCost { get; set; } = 20;
	public void Interact(GameObject player)
	{
		if(player.GetComponent<PlayerMoney>() is PlayerMoney money) 
		{
			money.TakeMoney( ArcadeCost );
		}
		
	}
}
