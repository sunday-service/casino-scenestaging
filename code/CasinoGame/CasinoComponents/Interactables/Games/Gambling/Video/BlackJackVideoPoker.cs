using Sandbox;

namespace Casino;

public class BlackJackVideoPoker : Component, IInteractable
{
	public void Interact(GameObject player)
	{
		Log.Info( "i am being interacted with" );
	}
}
