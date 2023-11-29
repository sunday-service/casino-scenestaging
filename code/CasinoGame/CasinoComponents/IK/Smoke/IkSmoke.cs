using Sandbox;

public sealed class IkSmoke : BaseComponent
{
	[Property] public GameObject Hand { get; set; }
	[Property] public GameObject Consumable { get; set; }

	protected override void OnUpdate()
	{
		if ( IsProxy )
			return;
		
		if ( Input.Down( "Attack1" ) )
		{
			if ( Consumable.Components.Get<CigaretteComponent>(FindMode.EverythingInSelfAndDescendants) is CigaretteComponent component )
			{
				component.AmountSmoked = component.AmountSmoked > 0.69 ? 0 : component.AmountSmoked + MathX.Clamp( 0.001f, 0, 1 );
			}
		}

		if ( Input.Pressed( "Attack1" ) )
		{
			if ( !Hand.IsValid() )
				return;

			var toPosition = new Vector3( 3.137f, 17.786f, 6.255f );
			var toRotation = new Vector3( -78.753f, 53.933f, 82.901f );
			var toTransfrom = new Transform( toPosition, Rotation.From( toRotation.x, toRotation.y, toRotation.z ) );

			Hand.Transform.LerpTo( toTransfrom, 0.35f );

		}

		if ( Input.Released( "Attack1" ) )
		{
			if ( !Hand.IsValid() )
				return;

			var fromPosition = new Vector3( 24, 18, -25 );
			var fromRotation = new Vector3( 48, 55, 80 );
			var fromTransform = new Transform( fromPosition, Rotation.From( fromRotation.x, fromRotation.y, fromRotation.z ) );

			Hand.Transform.LerpTo( fromTransform, 0.35f );

		}
	}
}
