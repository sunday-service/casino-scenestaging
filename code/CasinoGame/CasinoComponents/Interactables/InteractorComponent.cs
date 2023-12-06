using Sandbox;

namespace Casino;

public class InteractorComponent : Component
{
	[Property]
	public GameObject Player { get; set; }
	
	[Property]
	public float InteractRange { get; set; }
	
	[Property]
	public TagSet LayerMask { get; set; } = new();
	
	PhysicsTraceBuilder TraceRay( Vector3 from, Vector3 to ) => Scene.PhysicsWorld.Trace.Ray( from, to );

	protected override void OnUpdate()
	{
		if ( Player.Components.Get<CasinoPlayerController>() is CasinoPlayerController controller )
		{
			if ( Input.Pressed( "Use" ) )
			{
				var ray = TraceRay( controller.Eye.Transform.Position, controller.EyeAngles.ToRotation().Forward * InteractRange )
					.WithAnyTags( LayerMask )
					.Run();

				if ( ray.Hit && ray.Body.GameObject is GameObject interacted )
				{
					if ( interacted.Components.TryGet( out IInteractable interactableObj ) )
					{
						interactableObj.Interact(Player);
					}
				}
			}
		}
	}
}
