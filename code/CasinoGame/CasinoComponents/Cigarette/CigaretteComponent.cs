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
			model.SceneObject.Attributes.Set( "Direction", Transform.Rotation.Up );

			var smoke = model.GetComponent<ParticleSystem>( true, true );
			smoke.Transform.LocalPosition = smoke.Transform.LocalPosition.WithZ(12 * (1-AmountSmoked));

			
		}
	}

	public override void Update()
	{
		base.Update();

		if(Input.Down("Attack1"))
		{
			AmountSmoked += MathX.Clamp(0.01f, 0, 1);
		}

		if ( Input.Down( "Attack2" ) )
		{
			
			AmountSmoked -= MathX.Clamp( 0.01f, 0, 1 );

			AmountSmoked = AmountSmoked < 0 ? 0 : AmountSmoked;
		}
	}

}



