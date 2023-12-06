using Sandbox;

namespace Casino;

public partial class Sit : Component, IInteractable
{
	public GameObject Player { get; private set; }
	private bool IsSitting { get; set; } = false;

	public void Interact( GameObject player ) 
	{
		Log.Info( "Trying to sit" );

		Player = player;
		IsSitting = !IsSitting;
		
		if ( player.Components.Get<SkinnedModelRenderer>( FindMode.InChildren ) is SkinnedModelRenderer model )
		{
			
			if ( IsSitting )
			{
				model.Set( "sit", 1 );
				model.GameObject.Parent.SetParent( GameObject, false);
			}
			else
			{
				model.Set( "sit", 0 );
			}
		}
	}
}
