#pragma once

#include <Arduino.h>
#include <vector>

namespace Irrigation::Device
{
    [[nodiscard]]
    inline String Id()
    {
        char buffer[13];

        snprintf(
            buffer,
            sizeof(buffer),
            "%012llX",
            ESP.getEfuseMac());

        return String(buffer);
    }
}