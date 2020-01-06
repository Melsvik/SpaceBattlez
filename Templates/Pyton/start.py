import json
import sys

from bot import GameUpdate
from gameElements import GameState

print("Ready!")
sys.stdout.flush()

while True:
    input = sys.stdin.readline()
    state = json.loads(input)

    fleets = GameUpdate(GameState(state["bots"], state["planets"], state["fleets"]))
    
    output = []
    for fleet in fleets:
        output.append(fleet.toJSON())
    
    print('[' + ', '.join(output) + ']')
    sys.stdout.flush()
