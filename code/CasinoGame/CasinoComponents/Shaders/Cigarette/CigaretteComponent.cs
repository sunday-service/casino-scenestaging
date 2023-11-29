using Sandbox;

public sealed class CigaretteComponent : BaseComponent, BaseComponent.ExecuteInEditor
{
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

		if ( GameObject.Components.Get<ModelRenderer>() is ModelRenderer model )
		{
			RenderModel( model.SceneObject, 0 );
			//RenderModel( model.SceneObject, 1 );

			var smoke = model.Components.Get<ParticleSystem>( FindMode.EnabledInSelfAndDescendants );
			smoke.Transform.LocalPosition = smoke.Transform.LocalPosition.WithZ( 12 * (1 - AmountSmoked) );
		}
	}
}



