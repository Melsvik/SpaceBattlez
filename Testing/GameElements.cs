using System.Collections.Generic;

namespace BattleBotTester
{
	public class GameElements
	{
		public class Bot
		{
			public int Id { get; set; }
			public string Name { get; set; }
		}

		public class Position
		{
			public int X { get; set; }
			public int Y { get; set; }
		}

		public class Planet
		{
			public int Id { get; set; }
			public int OwnerId { get; set; }
			public Position Position { get; set; }
			public int NumberOfShips { get; set; }
			public int GrowthRate { get; set; }
		}

		public class Fleet
		{
			public int Id { get; set; }
			public int OwnerId { get; set; }
			public int NumberOfShips { get; set; }
			public int TicksToDestination { get; set; }
			public int DestinationPlanetId { get; set; }
			public int SourcePlanetId { get; set; }
			public Position Position { get; set; }
		}

		public class GameState
		{
			public List<Bot> Bots { get; set; }
			public List<Planet> Planets { get; set; }
			public List<Fleet> Fleets { get; set; }
		}

		public class FleetCommand
		{
			public int SourcePlanetId { get; set; }
			public int DestinationPlanetId { get; set; }
			public int NumberOfUnits { get; set; }

			public FleetCommand(int sourcePlanetId, int destinationPlanetId, int numberOfUnits)
			{
				SourcePlanetId = sourcePlanetId;
				DestinationPlanetId = destinationPlanetId;
				NumberOfUnits = numberOfUnits;
			}
		}
	}
}