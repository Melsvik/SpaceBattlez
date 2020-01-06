import json


class FleetCommand:
    def __init__(self, sourcePlanetId, destinationPlanetId, numberOfUnits):
        self.SourcePlanetId = sourcePlanetId
        self.DestinationPlanetId = destinationPlanetId
        self.NumberOfUnits = numberOfUnits

    def toJSON(self):
        return json.dumps(self.__dict__)


class Bot:
    def __init__(self, id, name):
        self.Id = id
        self.Name = name
    
class Position:
    def __init__(self, x, y):
        self.X = x
        self.Y = y

class Planet:
    def __init__(self, id, ownerId, position, numberOfShips, growthRate):
        self.Id = id
        self.OwnerId = ownerId
        self.Position = Position(position["x"], position["y"])
        self.NumberOfShips = numberOfShips
        self.GrowthRate = growthRate

class Fleet:
    def __init__(self, id, ownerId, sourcePlanetId, destinationPlanetId, numberOfShips, ticksToDestination, position):
        self.Id = id
        self.OwnerId = ownerId
        self.NumberOfShips = numberOfShips
        self.TicksToDestination = ticksToDestination
        self.DestinationPlanetId = destinationPlanetId
        self.SourcePlanetId = sourcePlanetId
        self.Position = Position(position["x"], position["y"])

class GameState:
    def __init__(self, bots, planets, fleets):
        self.Bots=[]
        for bot in bots:    
            self.Bots.append(Bot(   bot["id"], 
                                    bot["name"]))

        self.Planets=[]
        for planet in planets:    
            self.Planets.append(Planet( planet["id"], 
                                        planet["ownerID"], 
                                        planet["position"], 
                                        planet["numberOfShips"],
                                        planet["growthRate"]))
        
        self.Fleets=[]
        for fleet in fleets:    
            self.Fleets.append(Fleet(   fleet["id"], 
                                        fleet["ownerID"], 
                                        fleet["sourcePlanetID"], 
                                        fleet["destinationPlanetID"], 
                                        fleet["numberOfShips"], 
                                        fleet["ticksToDestination"], 
                                        fleet["position"]))
