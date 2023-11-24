using Sandbox;

namespace Casino;

public class SlotMachine : BaseComponent, IInteractable
{
	public enum ReelSymbol
	{
		CHERRY = 0,
		LEMON = 1,
		ORANGE = 2,
		PLUM = 3,
		BELL = 4, 
		BAR = 5,
		SEVEN = 6
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
			ReelSymbol.ORANGE => 16,
			ReelSymbol.PLUM => 32,
			ReelSymbol.BELL => 64,
			ReelSymbol.BAR => 128,
			ReelSymbol.SEVEN => 1024,

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

	public void Play(GameObject player) 
	{
		if ( player.GetComponent<PlayerMoney>( true, true ) is PlayerMoney playerMoney)
		{
			if(playerMoney.CurrentMoney < Bet)
			{
				Log.Info( $"{player.Name} doesn't have enough credits to play" );
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

				playerMoney.GiveMoney(payout );
			}
			else
			{
				Log.Info( $"{player} loses {Bet}" );

				playerMoney.TakeMoney( Bet );
			}
		}
	}

	public void Interact( GameObject player )
	{
		Log.Info( "Playing some slots" );

		Play(player);
	}
}
