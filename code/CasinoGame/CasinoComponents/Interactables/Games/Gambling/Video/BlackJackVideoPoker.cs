using Sandbox;

namespace Casino;

public class BlackJackVideoPoker : BaseComponent, IInteractable
{
	public void Interact(GameObject player)
	{
		Log.Info( "i am being interacted with" );
	}
}
