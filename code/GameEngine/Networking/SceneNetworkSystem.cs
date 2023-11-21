﻿using Sandbox;
using Sandbox.Network;
using Sandbox.Utility;
using System.Runtime;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

/// <summary>
/// This is created and referenced by the network system, as a way to route.
/// </summary>
public class SceneNetworkSystem : GameNetworkSystem
{
	internal static SceneNetworkSystem Instance { get; private set; }

	public SceneNetworkSystem()
	{
		Instance = this;
	}

	/// <summary>
	/// A client has joined and wants a snapshot of the world
	/// </summary>
	public override void GetSnapshot( NetworkChannel source, ref SnapshotMsg msg )
	{
		ThreadSafe.AssertIsMainThread();

		msg.Time = Time.Now;

		var o = new GameObject.SerializeOptions();
		o.SceneForNetwork = true;

		msg.SceneData = GameManager.ActiveScene.Serialize( o ).ToJsonString();
		msg.NetworkObjects = GameManager.ActiveScene.SerializeNetworkObjects();
	}

	public override void Dispose()
	{
		base.Dispose();

		if ( Instance == this )
			Instance = null;
	}

	/// <summary>
	/// We have recieved a snapshot of the world
	/// </summary>
	public override async Task SetSnapshotAsync( SnapshotMsg msg )
	{
		ThreadSafe.AssertIsMainThread();

		// TODO - really shouldn't allow set and get of ActiveScene
		// we should have a switchscene function, that disables everything
		// in the old scene.

		GameManager.ActiveScene?.Clear();
		GameManager.ActiveScene?.Destroy();
		GameManager.ActiveScene?.ProcessDeletes();

		GameManager.ActiveScene = new Scene();

		Time.Now = msg.Time;

		if ( !string.IsNullOrWhiteSpace( msg.SceneData ) )
		{
			var sceneData = JsonObject.Parse( msg.SceneData ).AsObject();
			GameManager.ActiveScene.Deserialize( sceneData );
		}

		foreach ( var nwo in msg.NetworkObjects )
		{
			OnObjectCreate( nwo, null );
		}

		GameManager.IsPlaying = true;
	}

	public override void OnJoined( NetworkChannel client )
	{
		Log.Info( $"Client {client.Name} ({client.Id}) has joined the game!" );
	}

	public override void OnLeave( NetworkChannel client )
	{
		Log.Info( $"Client {client.Name} ({client.Id}) has left the game!" );

		GameManager.ActiveScene.DestroyNetworkObjects( x => x.Owner == client.Id );
	}

	public override IDisposable Push()
	{
		if ( GameManager.ActiveScene is null )
			return null;

		return GameManager.ActiveScene.Push();
	}

	protected override void OnObjectCreate( in ObjectCreateMsg message, NetworkChannel source )
	{
		// TODO: Does source have the authority to create?

		var go = new GameObject();

		// TODO: Does this server allow this client to be creating objects from json?
		go.Deserialize( JsonObject.Parse( message.JsonData ).AsObject() );
		go.NetworkSpawnRemote( message );

		//go.Receive( message.Update );
	}

	protected override void OnObjectUpdate( in ObjectUpdateMsg message, NetworkChannel source )
	{
		var obj = GameManager.ActiveScene.Directory.FindByGuid( message.Guid );
		if ( obj is null )
		{
			Log.Warning( $"ObjectUpdate: Unknown object {message.Guid}" );
			return;
		}

		// TODO: Does source have the authority to update?

		obj.Receive( message );
	}

	protected override void OnObjectDestroy( in ObjectDestroyMsg message, NetworkChannel source )
	{
		var obj = GameManager.ActiveScene.Directory.FindByGuid( message.Guid );
		if ( obj is null )
		{
			Log.Warning( $"ObjectDestroy: Unknown object {message.Guid}" );
			return;
		}

		// TODO: Does source have the authoruty to destroy?

		if ( obj.Net is not null )
		{
			obj.Net.OnNetworkDestroy();
		}
		else
		{
			obj.Destroy();
		}
	}

	protected override void OnObjectMessage( in ObjectMessageMsg message, NetworkChannel source )
	{
		// TODO - flag for "global rpc", for calling statics?
		// DispatchRpc( null, .. )

		var obj = GameManager.ActiveScene.Directory.FindByGuid( message.Guid );
		if ( obj is null )
		{
			Log.Warning( $"OnObjectMessage: Unknown object {message.Guid}" );
			return;
		}

		// todo - support rpcs on GameObject? Not user created obviously, but makes sense to pass things through here?
		//		- like changing ownership etc?
		//		- just set target to the obj?
		// DispatchRpc( obj, .. )

		var componentName = message.Component;
		var target = obj.Components.FirstOrDefault( x => x.GetType().Name == componentName );

		DispatchRpc( source, target, message.MessageName, message.ArgumentData );
	}

	private void DispatchRpc( NetworkChannel source, object target, string messageName, byte[] argumentData )
	{
		var typeDesc = TypeLibrary.GetType( target.GetType() );
		if ( typeDesc == null )
		{
			throw new System.Exception( $"Tried to call RPC on unknown type {target.GetType()}" );
		}

		// TODO: support calling overloaded methods?
		var method = typeDesc.GetMethod( messageName );
		if ( method == null )
		{
			throw new System.Exception( $"Unknown RPC '{messageName}' on {typeDesc.Name}" );
		}

		// todo make sure has an RPC attribute
		// todo make sure of permissions, make sure source can call this

		if ( target is BaseComponent bc ) bc.rpcFromNetwork = true;

		var arguments = Packer.Deserialize( argumentData ) as object[];

		method.Invoke( target, arguments );
	}
}
