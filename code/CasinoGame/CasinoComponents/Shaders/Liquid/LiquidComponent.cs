﻿using Sandbox;

public sealed class LiquidComponent : BaseComponent, BaseComponent.ExecuteInEditor
{
	[Property, Range(0, 1)] public float FillAmount { get; set; } = 0.5f;
	[Property, Range( 0, 0.5f )] public float FoamThickness { get; set; } = 0.05f;

	[Property] public Color FillColorFoam { get; set; } = new Color( 0, 0.6f, 0.7f );
	[Property] public Color FillColorUpper { get; set; } = new Color( 0.0f, 0.5f, 0.5f );
	[Property] public Color FillColorLower { get; set; } = new Color( 0.0f, 0.0f, 1.0f);

	[Property, Range(0, 8, 0.1f)] public float RimLightStrengthPower { get; set; } = 2;

	[Property, Range(0, 0.1f)] public float MaxWobble { get; set; } = 0.004f;

	[Property, Range( 0, 64, 0.25f )] public float WobbleFrequency { get; set; } = 4f;
	[Property, Range( 0, 8, 0.1f )] public float WobbleAmplitude { get; set; } = 0.05f;

	float BobTime { get; set; } = 0.75f;

	float WobbleAmountAddX;
	float WobbleAmountAddY;

	Vector3 LastPosition;
	Vector3 LastRotation;

	protected override void OnPreRender()
	{
		base.OnPreRender();

		if ( GameObject.GetComponent<ModelComponent>().SceneObject is SceneObject model )
		{
			BobTime += Time.Delta;

			WobbleAmountAddX = MathX.Lerp( WobbleAmountAddX, 0, Time.Delta * 1 );
			WobbleAmountAddY = MathX.Lerp( WobbleAmountAddY, 0, Time.Delta * 1 );
			
			var pulse = 2f * (float) Math.PI * 1f;

			var wobbleAmountX = WobbleAmountAddX * (float) Math.Sin( pulse * BobTime );
			var wobbleAmountY = WobbleAmountAddY * (float) Math.Sin( pulse * BobTime );

			model.Batchable = false;

			model.Attributes.Set( "Direction", Transform.Rotation.Up );

			model.Attributes.Set( "FillAmount", FillAmount );

			model.Attributes.Set( "FoamThickness", FoamThickness );
			model.Attributes.Set( "FillColorFoam", FillColorFoam );
			model.Attributes.Set( "FillColorUpper", FillColorUpper );
			model.Attributes.Set( "FillColorLower", FillColorLower );

			model.Attributes.Set( "RimLightStrengthPower", RimLightStrengthPower );

			model.Attributes.Set( "WobbleX", wobbleAmountX );
			model.Attributes.Set( "WobbleY", wobbleAmountY );

			model.Attributes.Set( "FillWobbleFrequency", WobbleFrequency );
			model.Attributes.Set( "FillWobbleAmplitude", WobbleAmplitude );

			var velocity = (LastPosition - Transform.Position) / Time.Delta;
			var angularVelocity = Transform.Rotation.Angles().AsVector3() - LastRotation;

			WobbleAmountAddX += MathX.Clamp( (velocity.y + (angularVelocity.x * 1)) * MaxWobble, -MaxWobble, MaxWobble );
			WobbleAmountAddY += MathX.Clamp( (velocity.x + (angularVelocity.y * 1)) * MaxWobble, -MaxWobble, MaxWobble );

			LastPosition = Transform.Position;
			LastRotation = Transform.Rotation.Angles().AsVector3();
		}
	}
		
}


