#pragma once

#include <Arduino.h>

#include "Device/Id.h"
#include "Device/Config.h"

namespace Irrigation::Device
{
    struct Info
    {
        String id;
        String firmware;
        String model;
        String version;

        [[nodiscard]]
        static Info get()
        {
            return Info{
                .id = Device::id(),
                .firmware = Definition::Firmware,
                .model = Definition::Model,
                .version = Definition::Version};
        }
    };
}