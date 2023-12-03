using Sandbox;

public partial class PlayerMoney : BaseComponent
{
	[Property] public int StartingAmount { get; set; } = 1500;
	public int CurrentMoney { get; private set; }
	public Action OnMoneyGiven { get; set; }
	public Action OnMoneyTaken { get; set; }
	public Action OnMoneyException { get; set; }

	protected override void OnAwake()
	{
		base.OnAwake();

		CurrentMoney = StartingAmount;
	}

	public bool CanPurchase(int amount)
	{
		return CurrentMoney >= amount;
	}

	public bool GiveMoney(int amount)
	{
		CurrentMoney += amount;
		
		OnMoneyGiven?.Invoke();

		return true;
	}

	public bool TakeMoney(int amount)
	{
		var money = CurrentMoney - amount;

		if ( money < 0 )
		{
			OnMoneyException?.Invoke();
			return false;
		}

		CurrentMoney = money;

		OnMoneyTaken?.Invoke();

		return true;
	}

	
}
