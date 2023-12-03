using Sandbox;

namespace Casino;

public partial class SlotMachineGame
{
	public enum GameState
	{
		IDLE,
		GAMESTART,
		GAMEUPDATE,
		GAMEEND,
		GAMEERROR,
		PLAYERWON,
		PLAYERLOST
	}

	
	public Action GameIdle;
	public Action GameStart;
	public Action GameUpdate;
	public Action GameEnd;
	public Action GameError;

	public Action<int[]> PlayerWon;
	public Action<int[]> PlayerLost;


	public GameState CurrentGameState = GameState.IDLE;

	public int NumberOfSymbols { get; set; } = 10;
	public int NumberOfReels { get; set; } = 3;

	public SlotMachineGame()
	{
		GameIdle += OnGameIdle;
		GameStart += OnGameStart;
		GameUpdate += OnGameUpdate;
		GameEnd += OnGameEnd;
	}

	public void Start()
	{
		ChangeState( GameState.GAMESTART );
	}

	public void GameLoop()
	{
		switch ( CurrentGameState )
		{
			case GameState.IDLE:
				GameIdle?.Invoke();
				break;
			case GameState.GAMESTART:
				GameStart?.Invoke();
				break;
			case GameState.GAMEUPDATE:
				GameUpdate?.Invoke();
				break;
			case GameState.GAMEEND:
				GameEnd?.Invoke();
				break;
		}
	}

	public void ChangeState( GameState state )
	{
		CurrentGameState = state;
	}

	public void OnGameIdle()
	{

	}


	public void OnGameStart()
	{
		var symbols = GetSlotSymbolSequence();

		if ( Win(symbols) )
		{
			PlayerWon?.Invoke( symbols );
		}
		else
		{
			PlayerLost?.Invoke(symbols);
		}

		ChangeState( GameState.GAMEEND );
	}

	public void OnGameUpdate() 
	{ 
	
	}

	public void OnGameEnd()
	{
		ChangeState( GameState.IDLE );
	}

	private int[] GetSlotSymbolSequence()
	{
		int[] result = new int[NumberOfReels];

		for ( int i = 0; i < NumberOfReels; i++ )
		{
			result[i] = Game.Random.Next( 0, NumberOfSymbols);
		}

		return result;
	}

	private bool Win( int[] result)
	{
		// Winner if all reels have the same symbol
		return result.Distinct().Count() == 1;
	}

	
}
