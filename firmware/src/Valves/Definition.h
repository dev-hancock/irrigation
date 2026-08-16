#pragma once

#include "Common/Types.h"

namespace Irrigation::Valve
{
    using ValveId = uint8_t;

    struct Definition
    {
        ValveId id;
        Pin in1Pin;
        Pin in2Pin;
        Duration durationMs;
    };
}