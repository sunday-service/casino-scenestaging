using Casino;
using Sandbox;

public sealed class ServiceBellComponent : Component, IInteractable
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
		Sound.Play( sound, pos );
		
	}
}
