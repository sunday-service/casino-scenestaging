using Sandbox;

public sealed class ReturnToCasinoMenu : BaseComponent
{
	protected override void OnUpdate()
	{
		if ( Input.EscapePressed )
		{
			GameManager.ActiveScene.LoadFromFile( "scenes/tests/menu.scene" );
		}
	}
}
