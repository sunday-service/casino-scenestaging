﻿using Sandbox;
using Sandbox.Physics;

[Title( "Spring Joint" )]
[Category( "Physics" )]
[Icon( "waves", "red", "white" )]
public sealed class SpringJoint : Joint
{
	private float frequency;
	private float damping;
	private float maxLength;
	private float minLength;

	/// <summary>
	/// The stiffness of the spring
	/// </summary>
	[Property]
	public float Frequency
	{
		get => frequency;
		set
		{
			frequency = value;

			if ( springJoint.IsValid() )
			{
				var springLinear = springJoint.SpringLinear;
				springLinear.Frequency = value;
				springJoint.SpringLinear = springLinear;
			}
		}
	}

	/// <summary>
	/// The damping ratio of the spring, usually between 0 and 1
	/// </summary>
	[Property]
	public float Damping
	{
		get => damping;
		set
		{
			damping = value;

			if ( springJoint.IsValid() )
			{
				var springLinear = springJoint.SpringLinear;
				springLinear.Damping = value;
				springJoint.SpringLinear = springLinear;
			}
		}
	}

	/// <summary>
	/// Maximum length it should be allowed to go
	/// </summary>
	[Property]
	public float MaxLength
	{
		get => maxLength;
		set
		{
			maxLength = value;

			if ( springJoint.IsValid() )
			{
				springJoint.MaxLength = value;
			}
		}
	}

	/// <summary>
	/// Minimum length it should be allowed to go
	/// </summary>
	[Property]
	public float MinLength
	{
		get => minLength;
		set
		{
			minLength = value;

			if ( springJoint.IsValid() )
			{
				springJoint.MinLength = value;
			}
		}
	}

	private Sandbox.Physics.SpringJoint springJoint;

	protected override PhysicsJoint CreateJoint( PhysicsBody body1, PhysicsBody body2 )
	{
		springJoint = PhysicsJoint.CreateSpring( body1, body2, MinLength, MaxLength );
		springJoint.SpringLinear = new PhysicsSpring( Frequency, Damping );
		return springJoint;
	}
}
