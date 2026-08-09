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
            Router &router,
            IEvents &events)
            : _state(state),
              _driver(),
              _open(events, _state, _driver),
              _close(events, _state, _driver),
              _reset(events, _state, _driver),
              _api(_open, _close, _reset)
        {
            router.add(_api);
        }

        Result begin()
        {
            const std::vector<Valve> valves = {
                Valve{
                    .id = "1",
                    .in1Pin = 5,
                    .in2Pin = 4,
                    .durationMs = 200},
                Valve{
                    .id = "2",
                    .in1Pin = 18,
                    .in2Pin = 19,
                    .durationMs = 200}};

            _state = State(valves);

            for (const Valve init : valves)
            {
                _driver.begin(init);
            }

            const Result result = _reset.handle();

            if (result.isFailure())
            {
                return result;
            }

            Serial.println("Valves module initialized");

            return Result::Success();
        }

    private:
        State &_state;

        Driver _driver;

        unsigned long _timestamp = 0;

        OpenHandler _open;
        CloseHandler _close;
        ResetHandler _reset;

        MessageHandler _api;
    };
}