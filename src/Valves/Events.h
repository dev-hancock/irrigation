#pragma once

#include "Valves/Types.h"

namespace Irrigation::Valve
{
    namespace Event
    {
        struct Opened
        {
            ValveId id;
            unsigned long timestamp;
        };

        struct Closed
        {
            ValveId id;
            unsigned long timestamp;
        };
    }
}