using Sandbox;

namespace Casino;

public class SlotMachine : BaseComponent, IInteractable
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

	[Property, Group("Betting")] public int Bet { get; set; } = 5;

	private int NumberOfReels = 3;
	private int NumberOfSymbols => Enum.GetValues<ReelSymbol>().Length;

	private SlotMachineGame MiniGame = new SlotMachineGame();
	private GameObject Player { get; set; }

	Dictionary<ReelSymbol, int> Payout { get; set; } = new Dictionary<ReelSymbol, int>
	{
		{ ReelSymbol.CHERRY, 8 },
		{ ReelSymbol.GRAPE, 16 },
		{ ReelSymbol.LEMON, 32 },
		{ ReelSymbol.SPADE, 64 },
		{ ReelSymbol.SPIDER, 128 },
		{ ReelSymbol.BELL, 256 },
		{ ReelSymbol.ONEBAR, 512 },
		{ ReelSymbol.TWOBAR, 1024 },
		{ ReelSymbol.THREEBAR,2048 },
		{ ReelSymbol.SEVEN, 4096 },
	};

	protected override void OnAwake()
	{
		base.OnAwake();

		MiniGame.NumberOfReels = NumberOfReels;
		MiniGame.NumberOfSymbols = NumberOfSymbols;

		MiniGame.GameStart += OnGameStart;
		MiniGame.GameEnd += OnGameEnd;
		MiniGame.PlayerLost += OnPlayerLost;
		MiniGame.PlayerWon += OnPlayerWon;
	}

	public void Play( GameObject player )
	{
		NotificationFeed.Instance.PushNotification( "Playing slot machine", 0 );

		if ( player.Components.Get<PlayerMoney>( FindMode.InSelf ) is PlayerMoney playerMoney )
		{
			if ( !playerMoney.CanPurchase( Bet ) )
			{
				NotificationFeed.Instance.PushNotification( "Not enough credits to play the slots", 0 );

				return;
			}

			Player = player;

			MiniGame.Start();
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();

		MiniGame.GameLoop();
	}

	public void Interact( GameObject player )
	{
		Play( player );
	}

	public void OnGameStart()
	{
		if ( Components.Get<SkinnedModelRenderer>() is SkinnedModelRenderer model )
		{
			model.Set( "use", true );
		}
	}

	public void OnGameEnd()
	{
		Player = null;
	}

	public void OnPlayerLost( int[] symbols )
	{
		string symbolString = "";

		for ( int i = 0; i < NumberOfReels; i++ )
		{
			symbolString += $"{(ReelSymbol)symbols[i]} ";
		}

		NotificationFeed.Instance.PushNotification( $"Symbols: {symbolString}", 0 );
		NotificationFeed.Instance.PushNotification( $"Lost {Bet} credits", 0 );

		if ( Player.Components.Get<PlayerMoney>( FindMode.InSelf ) is PlayerMoney playerMoney )
		{
			playerMoney.TakeMoney( Bet );
		}
	}

	public void OnPlayerWon( int[] symbols )
	{
		string symbolString = "";

		for ( int i = 0; i < NumberOfReels; i++ )
		{
			symbolString += $"{(ReelSymbol)symbols[i]} ";
		}

		if ( Player.Components.Get<PlayerMoney>( FindMode.InSelf ) is PlayerMoney playerMoney )
		{
			var payout = Bet * Payout[(ReelSymbol) symbols[0]];

			NotificationFeed.Instance.PushNotification( $"Symbols: {symbolString}", 0 );
			NotificationFeed.Instance.PushNotification( $"Won {payout} credits", 0 );

			playerMoney.GiveMoney( payout );
		}
	}
}
