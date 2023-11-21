using Sandbox;

public sealed class IKDrink : BaseComponent
{
	[Property] public GameObject DrinkHand { get; set; }
	[Property] public GameObject Drink { get; set;}
	[Property, Range( 0, 1.0f )] public float AmountDrinked { get; set; } = 1.0f;

	public override void Update()
	{
		if ( Input.Down( "Attack2" ) )
		{
			AmountDrinked -= MathX.Clamp( 0.0025f, 0, 1 );
		}

		if ( Input.Pressed( "Attack2" ) )
		{
			if ( !DrinkHand.IsValid() )
				return;

			var toPosition = new Vector3( 7.071f, -16.123f, 4.725f );
			var toRotation = new Vector3( -58.689f, -67.962f, 104.251f );
			var toTransfrom = new Transform( toPosition, Rotation.From( toRotation.x, toRotation.y, toRotation.z ) );

			DrinkHand.Transform.LerpTo( toTransfrom, 0.35f );

		}

		if ( Input.Released( "Attack2" ) )
		{
			if ( !DrinkHand.IsValid() )
				return;

			var fromPosition = new Vector3( 3.267f, -13.667f, -2.8f );
			var fromRotation = new Vector3( -8.933f, -68.933f, 82.667f );
			var fromTransform = new Transform( fromPosition, Rotation.From( fromRotation.x, fromRotation.y, fromRotation.z ) );

			DrinkHand.Transform.LerpTo( fromTransform, 0.35f );

		}

		if ( Drink.GetComponent<LiquidComponent>( true, true ) is LiquidComponent component )
		{
			component.FillAmount = AmountDrinked;
		}
	}
}
