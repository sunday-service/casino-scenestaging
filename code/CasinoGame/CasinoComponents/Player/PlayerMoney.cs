using Sandbox;

public partial class PlayerMoney : BaseComponent
{
	[Property] public uint StartingAmount { get; set; } = 1500;
	public uint CurrentMoney { get; private set; }
	public Action OnMoneyGiven { get; set; }
	public Action OnMoneyTaken { get; set; }
	public Action OnMoneyException { get; set; }

	public override void OnAwake()
	{
		base.OnAwake();

		CurrentMoney = StartingAmount;
	}

	public bool GiveMoney(uint amount)
	{
		CurrentMoney += amount;
		
		OnMoneyGiven?.Invoke();

		return true;
	}

	public bool TakeMoney(uint amount)
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
