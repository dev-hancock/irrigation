#pragma once

#include <Arduino.h>
#include <ArduinoJson.h>
#include <vector>

#include "Device/Id.h"

#include "Health/Topics.h"

#include "Common/Api.h"

namespace Irrigation::Health
{
    class HeartbeatHandler
    {
    public:
        HeartbeatHandler(
            const Device::Definition &device,
            IEvents &events)
            : _events(events),
              _device(device)
        {
        }

        Result handle()
        {
            JsonDocument payload;

            payload["id"] = Device::Id();

            Result result = _events.publish(Topics::Pong, payload, true);

            if (result.isFailure())
            {
                return result;
            }

            Serial.println("Health initialized");

            return Result::Success();
        }

    private:
        IEvents &_events;
        const Device::Definition &_device;
    };
}
