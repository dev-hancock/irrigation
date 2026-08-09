#pragma once

#include <Arduino.h>
#include <ArduinoJson.h>
#include <vector>

#include "Common/Api.h"

#include "Valves/Types.h"
#include "Valves/State.h"
#include "Valves/Driver.h"
#include "Valves/Errors.h"

namespace Irrigation::Valve
{
    class OpenHandler final : public IHandler
    {
    public:
        OpenHandler(
            IEvents &events,
            State &state,
            Driver &driver)
            : _events(events),
              _state(state),
              _driver(driver)
        {
        }

        const char *topic() const override
        {
            return "valve/open";
        }

        Result handle(const JsonDocument &request) override
        {
            const ValveId id = request["id"] | "";

            if (id.isEmpty())
            {
                return Errors::NotFound();
            }

            ValveEntry *entry = _state.find(id);

            if (entry == nullptr)
            {
                return Errors::NotFound();
            }

            if (entry->status == ValveStatus::Open)
            {
                return Errors::InvalidState();
            }

            const Result result = _driver.open(entry->valve);

            if (result.isFailure())
            {
                return result;
            }

            const unsigned long timestamp = millis();

            entry->status = ValveStatus::Open;
            entry->updated = timestamp;

            JsonDocument event;

            event["id"] = id;
            event["timestamp"] = timestamp;

            _events.publish("valve/opened", event);

            return result;
        }

    private:
        IEvents &_events;
        State &_state;
        Driver &_driver;
    };
}