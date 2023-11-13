using Sandbox;

namespace Casino;

public class BlackJackMachine : BaseComponent, IInteractable
{
	public void Interact()
	{
		Log.Info( "i am being interacted with" );
	}
}
