﻿using Sandbox;
using Sandbox.Network;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

public abstract partial class BaseComponent
{
	public bool IsProxy => GameObject.IsProxy;

	public bool rpcFromNetwork;

	public void __rpc_Broadcast( Action resume, string methodName, params object[] argumentList )
	{
		if ( !rpcFromNetwork && GameObject.IsNetworked && SceneNetworkSystem.Instance is not null )
		{
			var msg = new ObjectMessageMsg();
			msg.Guid = GameObject.Id;
			msg.Component = GetType().Name;
			msg.MessageName = methodName;
			msg.ArgumentData = SceneNetworkSystem.Instance.Packer.Serialize( argumentList );

			SceneNetworkSystem.Instance.Broadcast( msg );
		}

		rpcFromNetwork = false;

		// we want to call this
		resume();
	}

}
