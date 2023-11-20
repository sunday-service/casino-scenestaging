using Sandbox;

public sealed class CigaretteComponent : BaseComponent, BaseComponent.ExecuteInEditor
{
	[Property] public GameObject SmokingHand { get; set; }
	[Property, Range( 0, 0.69f )] public float AmountSmoked { get; set; } = 0.5f;

	private void RenderModel(SceneObject sceneObject, int pass)
	{
		sceneObject.Batchable = false;

		sceneObject.Attributes.Set( "D_STENCIL_PASS", pass );
		sceneObject.Attributes.Set( "BurnLevel", AmountSmoked );
		sceneObject.Attributes.Set( "Direction", Transform.Rotation.Up );
	}

	protected override void OnPreRender()
	{
		base.OnPreRender();

		if ( GameObject.GetComponent<ModelComponent>() is ModelComponent model )
		{
			RenderModel( model.SceneObject, 0 );
			//RenderModel( model.SceneObject, 1 );

			var smoke = model.GetComponent<ParticleSystem>( true, true );
			smoke.Transform.LocalPosition = smoke.Transform.LocalPosition.WithZ( 12 * (1 - AmountSmoked) );
		}
	}


	public override void Update()
	{
		base.Update();

		if ( Input.Down("Attack1"))
		{
			AmountSmoked += MathX.Clamp(0.001f, 0, 1);
		}
	
		if(Input.Pressed("Attack1"))
		{
			if ( !SmokingHand.IsValid() )
				return;
			
			var toPosition = new Vector3( 3, 14, 10 );
			var toRotation = new Vector3( -102, 55, 80 );
			var toTransfrom = new Transform( toPosition, Rotation.From( toRotation.x, toRotation.y, toRotation.z ) );

			SmokingHand.Transform.LerpTo( toTransfrom, 0.35f );
			
		}
	
		if(Input.Released("Attack1"))
		{
			if ( !SmokingHand.IsValid() )
				return;
			
			var fromPosition = new Vector3( 24, 18, -25 );
			var fromRotation = new Vector3( 48, 55, 80 );
			var fromTransform = new Transform( fromPosition, Rotation.From( fromRotation.x, fromRotation.y, fromRotation.z ) );

			SmokingHand.Transform.LerpTo( fromTransform, 0.35f );
			
		}
	
		if ( Input.Down( "Attack2" ) )
		{
			
			AmountSmoked -= MathX.Clamp( 0.01f, 0, 1 );

			AmountSmoked = AmountSmoked < 0 ? 0 : AmountSmoked;
		}
	}

}



