using Sandbox;

namespace Casino;

public partial class MoneyGiver : BaseComponent, IInteractable
{
	[Property] public int GiveAmount { get; set; } = 50;
	public void Interact( GameObject player )
	{
		if ( IsProxy )
			return;

		if ( player.Components.Get<PlayerMoney>() is PlayerMoney playerMoney )
		{
			NotificationFeed.Instance.PushNotification( $"Giving {GiveAmount} to {player.Name}", 0 );
			playerMoney.GiveMoney( GiveAmount );
		}
	}
}
