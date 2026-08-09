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
            IEvents &events)
            : _state(state),
              _driver(),
              _open(events, _state, _driver),
              _close(events, _state, _driver)
        {
            router.add(_open);
            router.add(_close);
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

            return Result::Success();
        }

        void update()
        {
            if (millis() - _timestamp < 1000)
            {
                return;
            }

            _timestamp = millis();

            ValveEntry *entry = _state.find("1");

            if (entry == nullptr)
            {
                return;
            }

            if (entry->status == ValveStatus::Open)
            {
                const Result result = _driver.close(entry->valve);

                if (result.isFailure())
                {
                    return;
                }

                entry->status = ValveStatus::Closed;
                entry->updated = millis();

                return;
            }

            if (entry->status == ValveStatus::Closed)
            {
                const Result result = _driver.open(entry->valve);

                if (result.isFailure())
                {
                    return;
                }

                entry->status = ValveStatus::Open;
                entry->updated = millis();

                return;
            }
        }

    private:
        State &_state;

        Driver _driver;

        unsigned long _timestamp = 0;

        OpenHandler _open;
        CloseHandler _close;
    };
}