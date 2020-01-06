using System.Collections.Generic;

namespace SpaceBattlez.CSharp
{
	public interface IBot
	{
		List<GameElements.FleetCommand> GameUpdate(GameElements.GameState state);
	}
}