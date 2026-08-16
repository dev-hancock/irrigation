#pragma once

#include "Common/Api.h"

#include "Device/Definition.h"

namespace Irrigation::Device
{
    class Module
    {
    public:
        Module(
            const Device::Definition &device,
            IEvents &events)
            : _events(events),
              _device(device)
        {
        }

        Result begin()
        {
            JsonDocument payload;

            payload["firmware"] = _device.firmware;
            payload["model"] = _device.model;
            payload["version"] = _device.version;

            Result result = _events.publish("device", payload, true);

            if (result.isFailure())
            {
                return result;
            }

            Serial.println("Device initialized");

            return Result::Success();
        }

    private:
        IEvents &_events;
        const Device::Definition &_device;
    };
}