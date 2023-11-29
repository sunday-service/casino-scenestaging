using Sandbox;

namespace Casino;

public class SlotMachine : BaseComponent, IInteractable, INetworkSerializable
{
	public enum ReelSymbol
	{
		CHERRY = 0,
		GRAPE = 1,
		LEMON = 2,
		SPADE = 3,
		SPIDER = 4,
		BELL = 5,
		ONEBAR = 6,
		TWOBAR = 7,
		THREEBAR = 8,
		SEVEN = 9
	}

	[Property] public int NumberOfReels {get; set;} = 3;
	[Property] public int Bet {get; set;} = 5;
	[Property] public int MaxReelTime {get; set;} = 5;

	public int NumberOfSymbols => Enum.GetValues(typeof(ReelSymbol)).Length;

	public int GetPayout( ReelSymbol symbol )
	{
		return symbol switch
		{
			ReelSymbol.CHERRY => 8,
			ReelSymbol.GRAPE => 16,
			ReelSymbol.LEMON => 32,
			ReelSymbol.SPADE => 64,
			ReelSymbol.SPIDER => 128,
			ReelSymbol.BELL => 256,
			ReelSymbol.ONEBAR => 512,
			ReelSymbol.TWOBAR => 1024,
			ReelSymbol.THREEBAR => 2048,
			ReelSymbol.SEVEN => 4096,

			_ => 0
		};
	}

	public int[] GetSlotSymbolSequence()
	{
		int[] result = new int[NumberOfReels];

		for(int i=0;i<NumberOfReels; i++)
		{
			result[i] = Game.Random.Next(0, Enum.GetValues(typeof(ReelSymbol)).Length);	
		}

		return result;
	}

	public bool Win( int[] results)
	{
		// Winner if all reels have the same symbol
		return results.Distinct().Count() == 1;
	}

	private async void ReelOneSpin()
	{
		await GameTask.RunInThreadAsync( ReelOneSpinTask );


		Log.Info( $"Reel One Spin Finished" );

		await GameTask.RunInThreadAsync( ReelTwoSpinTask );

		Log.Info( $"Reel Two` Spin Finished" );
	}

	private async Task ReelOneSpinTask()
	{
		await GameTask.Delay( 1000 );

		Log.Info( "spinning" );
	}

	private async Task ReelTwoSpinTask()
	{
		await GameTask.Delay( 1500 );
	}

	private async Task ReelThreeSpinTask()
	{
		await GameTask.Delay( 1500 );
	}

	public void Play(GameObject player) 
	{
		if ( player.Components.Get<PlayerMoney>( FindMode.InSelf ) is PlayerMoney playerMoney)
		{
			if(playerMoney.CurrentMoney < Bet)
			{
				NotificationFeed.Instance.PushNotification( "Not enough credits to play the slots", 0 );

				return;
			}

			var results = GetSlotSymbolSequence();

			foreach ( var result in results )
			{
				Log.Info( $"{(ReelSymbol)result}" );
			}

			Log.Info( $"{results.Distinct().Count()}" );

			if ( Win( results ) )
			{
				var payout = Bet * GetPayout( (ReelSymbol) results[0]);

				Log.Info( $"{player} wins {payout}" );

				NotificationFeed.Instance.PushNotification( $"Won {payout} credits", 0 );

				playerMoney.GiveMoney(payout );
			}
			else
			{
				Log.Info( $"{player} loses {Bet}" );

				NotificationFeed.Instance.PushNotification( $"Lost {Bet} credits", 0 );

				playerMoney.TakeMoney( Bet );
			}

			ReelOneSpin();
		}
	}

	public void Interact( GameObject player )
	{
		Log.Info( "Playing some slots" );

		Play(player);
	}

	public void Write( ref ByteStream stream )
	{
		
	}

	public void Read( ByteStream stream )
	{
		
	}
}
