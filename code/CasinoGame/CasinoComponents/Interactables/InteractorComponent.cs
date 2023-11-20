using Sandbox;

namespace Casino;

public class InteractorComponent : BaseComponent
{
	[Property]
	public GameObject Player { get; set; }
	
	[Property]
	public float InteractRange { get; set; }
	
	[Property]
	public TagSet LayerMask { get; set; } = new();
	
	PhysicsTraceBuilder TraceRay( Vector3 from, Vector3 to ) => Scene.PhysicsWorld.Trace.Ray( from, to );

	public override void Update()
	{
		if ( Player.GetComponent<CasinoPlayerController>() is CasinoPlayerController controller )
		{
			if ( Input.Pressed( "Use" ) )
			{
				var ray = TraceRay( controller.Eye.Transform.Position, controller.EyeAngles.ToRotation().Forward * InteractRange )
					.WithAnyTags( LayerMask )
					.Run();

				if ( ray.Hit && ray.Body.GameObject is GameObject interacted )
				{
					if ( interacted.TryGetComponent( out IInteractable interactableObj ) )
					{
						interactableObj.Interact(Player);
					}
				}
			}
		}
	}
}
