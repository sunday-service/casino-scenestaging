using Sandbox;

namespace Casino;

public class InteractorComponent : BaseComponent
{
	[Property]
	public GameObject source { get; set; }
	
	[Property]
	public float interactRange { get; set; }
	
	[Property]
	public TagSet layerMask { get; set; } = new();
	
	PhysicsTraceBuilder TraceRay( Vector3 from, Vector3 to ) => Scene.PhysicsWorld.Trace.Ray( from, to );

	public override void OnEnabled()
	{
		
	}

	public override void FixedUpdate()
	{
		if ( Input.Down( "Use" ) )
		{
			var ray = TraceRay( source.Transform.Position, source.Transform.Rotation.Forward * interactRange )
				.WithAnyTags( layerMask )
				.Run();
			if ( ray.Hit && ray.Body.GameObject is GameObject interacted )
			{
				if ( interacted.TryGetComponent( out IInteractable interactableObj ) )
				{
					interactableObj.Interact();
				}
			}
		}
	}
}
