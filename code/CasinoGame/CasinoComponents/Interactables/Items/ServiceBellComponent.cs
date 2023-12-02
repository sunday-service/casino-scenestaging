using Casino;
using Sandbox;

public sealed class ServiceBellComponent : BaseComponent, IInteractable
{
	protected override void OnUpdate()
	{

	}

	public void Interact( GameObject player )
	{
		PlaySound( "servicebell", Transform.Position );
		Log.Info( "HELLO" );
	}

	[Broadcast]
	private void PlaySound(string sound, Vector3 pos)
	{
		Sound.FromWorld( sound, pos );
	}
}
