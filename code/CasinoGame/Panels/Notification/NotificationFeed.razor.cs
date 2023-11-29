using Sandbox;

namespace Casino;

public partial class NotificationFeed : PanelComponent
{
	public static NotificationFeed Instance;
	[Property] public float FadeOutTime { get; set; } = 5.0f;

	public record Notification( string message, int type, RealTimeSince timeSinceAdded );
	public List<Notification> Notifications { get; set; } = new List<Notification>();

	public NotificationFeed()
	{
		Instance = this;
	}

	public void PushNotification(string  message, int type )
	{
		Notifications.Insert(0, new Notification( message, type, 0.0f ) );

		StateHasChanged();
	}

	public override void Update()
	{
		base.Update();

		if ( IsProxy )
			return;

		if(Notifications.RemoveAll(x => x.timeSinceAdded > FadeOutTime) > 0)
		{
			StateHasChanged();
		}
	}

	protected override int BuildHash() => System.HashCode.Combine( Notifications );
}
