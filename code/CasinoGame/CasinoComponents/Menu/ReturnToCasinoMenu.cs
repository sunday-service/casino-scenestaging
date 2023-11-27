using Sandbox;

public sealed class ReturnToCasinoMenu : BaseComponent
{
	public override void Update()
	{
		if ( Input.EscapePressed )
		{
			GameManager.ActiveScene.LoadFromFile( "scenes/tests/menu.scene" );
		}
	}
}
