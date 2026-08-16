#pragma once

#include <Arduino.h>
#include <ArduinoJson.h>
#include <vector>

#include "Health/Api.h"
#include "Health/Handlers/Heartbeat.h"

#include "Common/Api.h"

namespace Irrigation::Health
{
    class Module
    {
    public:
        Module(
            const Device::Definition &device,
            Router &router,
            IEvents &events)
            : _heartbeat(device, events),
              _api(_heartbeat)
        {
            router.add(_api);
        }

        Result begin()
        {
            Serial.println("Health initialized");

            return Result::Success();
        }

    private:
        HeartbeatHandler _heartbeat;

        MessageHandler _api;
    };
}