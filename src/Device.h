#pragma once

#include <Arduino.h>

namespace Irrigation
{
    class Device
    {
    public:
        [[nodiscard]]
        static String id()
        {
            char buffer[13];

            snprintf(
                buffer,
                sizeof(buffer),
                "%012llX",
                ESP.getEfuseMac());

            return String(buffer);
        }
    };
}