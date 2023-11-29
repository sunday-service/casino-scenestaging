using Sandbox;

namespace Casino;

public class CigaretteMachine : BaseComponent, IInteractable
{
	[Property] int CigaretteCost { get; set; } = 20;
	public void Interact(GameObject player)
	{
		if(player.Components.Get<PlayerMoney>() is PlayerMoney money) 
		{
			NotificationFeed.Instance.PushNotification( $"Buying cigarettes for {CigaretteCost}", 0 );

			money.TakeMoney( CigaretteCost );
		}
		
	}
}
