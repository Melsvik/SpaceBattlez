using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceBattlez.CSharp
{
	public class Bot : IBot
	{
		public List<GameElements.FleetCommand> GameUpdate(GameElements.GameState state)
		{
			const int me = 1;
			var output = new List<GameElements.FleetCommand>();

			foreach (var myPlanet in state.Planets.Where(x => x.OwnerId == me))
			{
				var enemy = state.Planets.Where(x => x.OwnerId != me).OrderBy(x => Distance(myPlanet.Position, x.Position)).FirstOrDefault();

				if (enemy != null)
				{
					output.Add(new GameElements.FleetCommand(myPlanet.Id, enemy.Id, myPlanet.NumberOfShips - 1));
				}
			}

			return output;
		}

		private double Distance(GameElements.Position p1, GameElements.Position p2)
		{
			return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
		}
	}
}