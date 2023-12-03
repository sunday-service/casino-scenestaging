using Sandbox;

namespace Casino;

public partial class MoneyTaker : BaseComponent, IInteractable
{
	[Property] public int TakeAmount { get; set; } = 5;
	public void Interact( GameObject player ) 
	{
		if ( IsProxy )
			return;

		if(player.Components.Get<PlayerMoney>() is PlayerMoney playerMoney )
		{
			NotificationFeed.Instance.PushNotification( $"Taking {TakeAmount} from {player.Name}", 0 );
			playerMoney.TakeMoney( TakeAmount );
		}
	}
}
