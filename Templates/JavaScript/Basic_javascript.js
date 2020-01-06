process.stdin.setEncoding('utf8');

var content = '';
console.log('ready!');
process.stdin.on('data', function(buf) {
    content += buf.toString();
    if (content !== null && content.indexOf('\n') > -1) {
        var gamestate = JSON.parse(content);
        if (gamestate !== null) {
            var fleetCommands = gameUpdate(gamestate);
            if (fleetCommands !== null) {
                var output = JSON.stringify(fleetCommands);
                if (output !== null) {
                    console.log(output);
                }
            }
        }
        content = '';
    }
});




function gameUpdate(gameState) {

    var output = [];
    for (var i = 0, len = gameState.planets.length; i < len; i++) {
        var planet = gameState.planets[i];
        if (planet.ownerID === 1) {
            var enemy = getClosestEnemy(gameState.planets[i], gameState.planets);
            if (enemy !== null) {
                var fleetCommand = {
                    SourcePlanetId: planet.id,
                    DestinationPlanetId: enemy.id,
                    NumberOfUnits: planet.numberOfShips - 1
                };
                output.push(fleetCommand);
            }
        }
    }
    return output;
}

function getClosestEnemy(myPlanet, planets) {

    var minDist = Number.MAX_SAFE_INTEGER;
    var output = null;
    for (var i = 0, len = planets.length; i < len; i++) {
        if (planets[i].ownerID !== 1) {
            var dist = distance(myPlanet.position, planets[i].position);
            if (dist > 0 && dist < minDist) {
                minDist = dist;
                output = planets[i];
            }
        }
    }
    return output;
}


function distance(p1, p2) {
    return Math.sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y));
}
