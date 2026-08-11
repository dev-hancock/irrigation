#pragma once

#include <Arduino.h>

namespace Irrigation::Device
{
    [[nodiscard]]
    inline String id()
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