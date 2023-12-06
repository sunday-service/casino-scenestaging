using Sandbox;

namespace Casino;

public partial class Clock : Component, INetworkSerializable
{
	private static double GameTime { get; set; } = 0;

	public static Clock Instance { get; private set; }

	protected override void OnAwake()
	{
		base.OnAwake();

		Instance = new Clock();
	}

	protected override void OnFixedUpdate()
	{
		base.OnFixedUpdate();

		if ( IsProxy ) return;

		GameTime += 0.05f * Time.Delta;

		PrintTime();
	}

	[Broadcast]
	public void PrintTime()
	{
		Log.Info( $"Game Time: {GameTime}" );
	}

	public void Write( ref ByteStream stream )
	{
		stream.Write( GameTime );
	}

	public void Read( ByteStream stream )
	{
		GameTime = stream.Read<double>();
	}
}
