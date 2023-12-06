using Sandbox;

namespace Casino;

public class ArcadeMachine : Component, IInteractable
{
	[Property] int ArcadeCost { get; set; } = 20;
	public void Interact(GameObject player)
	{
		if(player.Components.Get<PlayerMoney>() is PlayerMoney money) 
		{
			NotificationFeed.Instance.PushNotification( $"Spending {ArcadeCost} on arcade machine", 0 );
			money.TakeMoney( ArcadeCost );
		}
		
	}
}
