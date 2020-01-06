import math
from typing import List

from gameElements import FleetCommand, GameState, Position


def GameUpdate(state : GameState) -> List[FleetCommand]:
    me = 1
    output = []

    myPlanets = list(filter(lambda x: x.OwnerId == me, state.Planets))
    enemies = list(filter(lambda x: x.OwnerId != me, state.Planets))

    if myPlanets and enemies:
        for planet in myPlanets:
            enemy = next(iter(sorted(enemies, key=lambda x:Distance(planet.Position, x.Position))))
            output.append(FleetCommand(planet.Id, enemy.Id, planet.NumberOfShips - 1))

    return output

def Distance(p1: Position, p2: Position):
    return math.sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y))
