#pragma once

#include <Arduino.h>
#include <ArduinoJson.h>
#include <vector>

#include "Valves/State.h"
#include "Valves/Driver.h"

#include "Valves/Api.h"

#include "Valves/Handlers/Open.h"
#include "Valves/Handlers/Close.h"
#include "Valves/Handlers/Reset.h"

#include "Common/Api.h"

namespace Irrigation::Valve
{
    class Module
    {
    public:
        Module(
            State &state,
            const std::vector<Valve> &valves,
            Router &router,
            IEvents &events)
            : _state(state),
              _driver(),
              _open(events, _state, _driver),
              _close(events, _state, _driver),
              _reset(events, _state, _driver),
              _api(_open, _close, _reset),
              _valves(valves)
        {
            router.add(_api);
        }

        Result begin()
        {
            _state = State(_valves);

            for (const Valve init : _valves)
            {
                _driver.begin(init);
            }

            const Result result = _reset.handle();

            if (result.isFailure())
            {
                return result;
            }

            Serial.println("Valves initialized");

            return Result::Success();
        }

    private:
        State &_state;
        const std::vector<Valve> &_valves;

        Driver _driver;

        OpenHandler _open;
        CloseHandler _close;
        ResetHandler _reset;

        MessageHandler _api;
    };
}