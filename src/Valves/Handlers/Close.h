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
    class CloseHandler final : public IHandler
    {
    public:
        CloseHandler(
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
            return "valve/close";
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

            if (entry->status == ValveStatus::Closed)
            {
                return Errors::InvalidState();
            }

            const Result result = _driver.close(entry->valve);

            if (result.isFailure())
            {
                return result;
            }

            const unsigned long timestamp = millis();

            entry->status = ValveStatus::Closed;
            entry->updated = timestamp;

            JsonDocument event;

            event["id"] = id;
            event["timestamp"] = timestamp;

            _events.publish("valve/closed", event);

            return result;
        }

    private:
        IEvents &_events;
        State &_state;
        Driver &_driver;
    };
}