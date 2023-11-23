using Sandbox;

namespace Casino;

public class SlotMachine : BaseComponent, IInteractable
{
	public void Interact( GameObject player )
	{
		Log.Info( "Playing some slots heh heh" );
	}
}
