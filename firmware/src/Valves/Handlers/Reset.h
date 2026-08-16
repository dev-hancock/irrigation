#pragma once

#include <Arduino.h>
#include <ArduinoJson.h>
#include <vector>

#include "Valves/Types.h"
#include "Valves/State.h"
#include "Valves/Driver.h"
#include "Valves/Errors.h"
#include "Valves/Topics.h"

namespace Irrigation::Valve
{
    class ResetHandler
    {
    public:
        ResetHandler(
            IEvents &events,
            State &state,
            Driver &driver)
            : _events(events),
              _state(state),
              _driver(driver)
        {
        }

        Result handle()
        {
            for (ValveEntry &entry : _state)
            {
                entry.status = ValveStatus::Unknown;
                entry.updated = 0;

                Result result = Result::Success();

                result = _driver.close(entry.valve);

                if (result.isFailure())
                {
                    return result;
                }

                const Timestamp timestamp = millis();

                entry.status = ValveStatus::Closed;
                entry.updated = timestamp;

                ValveId id = entry.valve.id;

                JsonDocument event;

                event["id"] = id;
                event["status"] = toString(ValveStatus::Closed);

                const String topic = Topics::ValveState(id);

                _events.publish(topic, event, true);

                if (result.isFailure())
                {
                    return result;
                }
            }

            return Result::Success();
        }

    private:
        IEvents &_events;
        State &_state;
        Driver &_driver;
    };
}