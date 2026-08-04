#pragma once

#include <Arduino.h>
#include <ArduinoJson.h>
#include <vector>

#include "Valves/State.h"
#include "Valves/Driver.h"
#include "Valves/Handlers/Open.h"
#include "Valves/Handlers/Close.h"

#include "Common/Api.h"

namespace Irrigation::Valve
{
    class Module
    {
    public:
        Module(
            State &state,
            Router &router,
            Events &events)
            : _state(state),
              _router(router),
              _driver(),
              _open(events, _state, _driver),
              _close(events, _state, _driver)
        {
        }

        void begin()
        {
            const std::vector<Valve> valves = {
                Valve{
                    .id = "1",
                    .in1Pin = 5,
                    .in2Pin = 4,
                    .durationMs = 20},
                Valve{
                    .id = "2",
                    .in1Pin = 18,
                    .in2Pin = 19,
                    .durationMs = 20}};

            _state = State(valves);

            for (const Valve init : valves)
            {
                _driver.begin(init);
            }

            _router.add(_open);
            _router.add(_close);
        }

    private:
        State &_state;
        Router &_router;

        Driver _driver;

        OpenHandler _open;
        CloseHandler _close;
    };
}