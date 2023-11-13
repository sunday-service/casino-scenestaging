using Sandbox;

public sealed class CigaretteComponent : BaseComponent, BaseComponent.ExecuteInEditor
{
	[Property, Range( 0, 0.69f )] public float AmountSmoked { get; set; } = 0.5f;
	
	protected override void OnPreRender()
	{
		base.OnPreRender();

		if ( GameObject.GetComponent<ModelComponent>() is ModelComponent model )
		{
			model.SceneObject.Batchable = false;

			model.SceneObject.Attributes.Set( "BurnLevel", AmountSmoked );

			var smoke = model.GetComponent<ParticleSystem>( true, true );
			smoke.Transform.LocalPosition = smoke.Transform.LocalPosition.WithZ(12 * (1-AmountSmoked));

			
		}
	}

}



