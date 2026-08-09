#pragma once

#include "Common/Api.h"

#include "Device/Types.h"

namespace Irrigation::Device
{
    class Module
    {
    public:
        Module(IEvents &events)
            : _events(events)
        {
        }

        Result begin()
        {
            const Info info = Info::get();

            JsonDocument payload;

            payload["id"] = info.id;
            payload["firmware"] = info.firmware;
            payload["model"] = info.model;

            Result result = _events.publish("device", payload, true);

            if (result.isFailure())
            {
                return result;
            }

            Serial.println("Device module initialized");

            return Result::Success();
        }

    private:
        IEvents &_events;
    };
}