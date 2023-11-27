using Sandbox;

public sealed class IkDrink : BaseComponent
{
	[Property] public GameObject Hand { get; set; }
	[Property] public GameObject Consumable { get; set;}

	public override void Update()
	{
		if ( Input.Down( "Attack2" ) )
		{
			if ( Consumable.GetComponent<LiquidComponent>( true, true ) is LiquidComponent component )
			{
				component.FillAmount = component.FillAmount < 0 ? 0.8f : component.FillAmount - MathX.Clamp( 0.001f, 0, 1 );
			}
		}

		if ( Input.Pressed( "Attack2" ) )
		{
			if ( !Hand.IsValid() )
				return;

			var toPosition = new Vector3( 7.071f, -16.123f, 4.725f );
			var toRotation = new Vector3( -58.689f, -67.962f, 104.251f );
			var toTransfrom = new Transform( toPosition, Rotation.From( toRotation.x, toRotation.y, toRotation.z ) );

			Hand.Transform.LerpTo( toTransfrom, 0.35f );
		}

		if ( Input.Released( "Attack2" ) )
		{
			if ( !Hand.IsValid() )
				return;

			var fromPosition = new Vector3( 11.347f, -10.747f, -3.858f );
			var fromRotation = new Vector3( -8.933f, -68.933f, 82.667f );
			var fromTransform = new Transform( fromPosition, Rotation.From( fromRotation.x, fromRotation.y, fromRotation.z ) );

			Hand.Transform.LerpTo( fromTransform, 0.35f );
		}
	}
}
