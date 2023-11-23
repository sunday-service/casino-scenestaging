using Sandbox;
using System.Collections.Generic;

namespace Casino;

public class BlackJackTable : BaseComponent, IInteractable
{

	public enum CardSuit
	{
		SPADES = 0,
		HEARTS = 1,
		CLUBS = 2,
		DIAMONDS = 3,
	}

	public enum CardRank
	{
		ACE = 0,
		KING = 1,
		QUEEN = 2,
		JACK = 3,
		TEN = 4,
		NINE = 5,
		EIGHT = 6,
		SEVEN = 7,
		SIX = 8,
		FIVE = 9,
		FOUR = 10,
		THREE = 11,
		TWO = 12,
		ONE = 13
	};

	public struct Card
	{
		public Card( CardRank rank, CardSuit suit )
		{
			Rank = rank;
			Suit = suit;
		}

		public CardRank Rank { get; set; }
		public CardSuit Suit { get; set; }
		public bool IsVisble { get; set; } = false;
	}

	public struct FrenchDeck
	{
		public List<Card> Cards;

		public FrenchDeck()
		{
			Cards = new List<Card>();

			foreach(CardRank rank in Enum.GetValues(typeof(CardRank)))
			{	
				foreach(CardSuit suit in Enum.GetValues(typeof(CardSuit)))
				{
					Cards.Add( new Card( rank, suit ) );
				}
			}
		}

		public void Shuffle()
		{
			// Fisher - Yates Shuffle Algorithm from
			// https://code-maze.com/csharp-randomize-list/

			for ( int i = Cards.Count - 1; i > 0; i-- )
			{
				var k = Game.Random.Next( i + 1 );
				var value = Cards[k];
				Cards[k] = Cards[i];
				Cards[i] = value;
			}
		}

		public Card DrawCard(bool isVisible = false)
		{
			var card = Cards.First();
			Cards.Remove( card );

			card.IsVisble = isVisible;

			return card;
		}

		public void LogDeck()
		{
			foreach(var card in Cards)
			{
				Log.Info( $"{card.Rank} {card.Suit}" );
			}

			Log.Info( "" );
		}
	}

	public struct BlackJackPlayer
	{
		public BlackJackPlayer(string name)
		{
			Name = name;
			PlayerCards = new List<Card>();
		}

		public int GetHandValue()
		{
			var value = 0;

			foreach ( var card in PlayerCards )
			{
				value += GetCardValue( card);
			}

			return value;
		}

		private int GetCardValue( Card card)
		{
			return card.Rank switch
			{
				CardRank.ACE => PlayerHand + 11 > 21 ? 1 : 11,
				CardRank.KING => 10,
				CardRank.QUEEN => 10,
				CardRank.JACK => 10,
				CardRank.TEN => 10,
				CardRank.NINE => 9,
				CardRank.EIGHT => 8,
				CardRank.SEVEN => 7,
				CardRank.SIX => 6,
				CardRank.FIVE => 5,
				CardRank.FOUR => 4,
				CardRank.THREE => 3,
				CardRank.TWO => 2,
				CardRank.ONE => 1,

				_ => 0
			};
		}

		public string Name;
		public List<Card> PlayerCards;
		public int PlayerHand => GetHandValue();
	}

	[Property] public GameObject DealerHiddenCard { get; set; }
	[Property] public GameObject DealerFacingCard { get; set; }
	[Property] public GameObject PlayerHiddenCard { get; set; }
	[Property] public GameObject PlayerFacingCard { get; set; }

	public FrenchDeck CardDeck { get; set; } = new FrenchDeck();
	public List<BlackJackPlayer> BlackJackPlayers { get; set; } = new List<BlackJackPlayer>();

	public void Interact( GameObject player )
	{
		if(PlayerFacingCard.GetComponent<ModelComponent>() is ModelComponent component)
		{
			component.MaterialOverride = component.Model.GetMaterials( 1 ).ToList()[0];
		}

		Log.Info( "Interacting with Black Jack Table" );

		DealCards();
	}

	public void ResetGame()
	{
		CardDeck = new FrenchDeck();

		BlackJackPlayers = new List<BlackJackPlayer>()
		{
			new BlackJackPlayer("Dealer"),
			new BlackJackPlayer("Player")
		};
	}

	public void DealCards()
	{
		ResetGame();
		
		CardDeck.LogDeck();
		CardDeck.Shuffle();
		CardDeck.LogDeck();

		foreach(var player in BlackJackPlayers)
		{
			player.PlayerCards.Add( CardDeck.DrawCard() );
		}

		foreach ( var player in BlackJackPlayers )
		{
			player.PlayerCards.Add( CardDeck.DrawCard(true) );
		}

		CardDeck.LogDeck();

		foreach(var player in BlackJackPlayers )
		{
			foreach(var card in player.PlayerCards)
			{
				Log.Info( $"{player.Name} Card: {card.Rank} {card.Suit} {card.IsVisble}" );
			}

			Log.Info( "" );
			Log.Info( $"{player.Name} Hand Value: {player.PlayerHand}" );
			Log.Info( "" );
		}
	}

	


	
}
