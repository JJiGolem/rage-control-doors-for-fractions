const fractionDoorEntered = {
    entered: false,
    fractionId: 0,
    accessRank: 0,
    locked: false,
    position: null
}

mp.events.add('render', () => {
    if (fractionDoorEntered.entered && fractionDoorEntered.position) {
        let pos = fractionDoorEntered.position;
        if (mp.game.gameplay.getDistanceBetweenCoords(localplayer.position.x, localplayer.position.y, localplayer.position.z, pos.x, pos.y, pos.z, true) < 3) {
            if (fractionDoorEntered.locked) {
                mp.game.ui.resetHudComponentValues(10);
                mp.game.ui.setHudComponentPosition(10, 0, 0);
                mp.game.ui.setTextComponentFormat('STRING');
                mp.game.ui.addTextComponentSubstringPlayerName("~h~Дверь ~r~закрыта");
                mp.game.ui.displayHelpTextFromStringLabel((0), false, true, -1);
            }
        }
    }
});

function doorStateChange(_model, _rightDoorPosition, _leftDoorPosition, _locked) {
    let modelHash = isNaN(_model) ? mp.game.joaat(_model) : parseInt(_model);
    mp.game.object.addDoorToSystem(modelHash, modelHash, _rightDoorPosition.x, _rightDoorPosition.y, _rightDoorPosition.z, false, false, false);
    mp.game.object.doorControl(modelHash, _rightDoorPosition.x, _rightDoorPosition.y, _rightDoorPosition.z, _locked, 0, 0, 0);

    if (_leftDoorPosition) {
        mp.game.object.addDoorToSystem(modelHash, modelHash, _leftDoorPosition.x, _leftDoorPosition.y, _leftDoorPosition.z, false, false, false);
        mp.game.object.doorControl(modelHash, _leftDoorPosition.x, _leftDoorPosition.y, _leftDoorPosition.z, _locked, 0, 0, 0);
    }

    fractionDoorEntered.position = _rightDoorPosition;
    fractionDoorEntered.locked = _locked;
}

mp.events.add("fractionDoors::enter_sync", (_fractionId, _accessRank, _model, _rightDoorPosition, _leftDoorPosition, _locked) => {
    fractionDoorEntered.entered = true;
    fractionDoorEntered.fractionId = _fractionId;
    fractionDoorEntered.accessRank = _accessRank;

    doorStateChange(_model, _rightDoorPosition, _leftDoorPosition, _locked);
})

mp.events.add("fractionDoors::sync", doorStateChange);

mp.events.add("fractionDoors::exit", () => {
    fractionDoorEntered.entered = false;
    fractionDoorEntered.fractionId = 0;
    fractionDoorEntered.accessRank = 0;
    fractionDoorEntered.position = null;
    fractionDoorEntered.locked = false;
})

mp.keys.bind(Keys.VK_E, false, () => {
    if (!loggedin || chatActive || editing || cuffed) return;
    if (fractionDoorEntered.entered) {
        let playerFractionId = localplayer.getVariable('fraction');
        let playerFractionRank = localplayer.getVariable('fractionRank');

        if (fractionDoorEntered.fractionId == playerFractionId
            && fractionDoorEntered.accessRank <= playerFractionRank) 
            {
                mp.events.callRemote("fractionDoor::control");
            }
    }
});