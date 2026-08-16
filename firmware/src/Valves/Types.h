#pragma once

#include <Arduino.h>
#include <vector>

#include "Common/Types.h"

#include "Valves/Definition.h"

namespace Irrigation::Valve
{
    enum class ValveStatus
    {
        Unknown,
        Open,
        Closed
    };

    using Valve = Definition;

    inline const char *toString(ValveStatus status)
    {
        switch (status)
        {
        case ValveStatus::Unknown:
            return "unknown";
        case ValveStatus::Open:
            return "open";
        case ValveStatus::Closed:
            return "closed";
        default:
            return "invalid";
        }
    }

    struct ValveEntry
    {
        explicit ValveEntry(const Valve &valve)
            : valve(valve)
        {
        }

        Valve valve;
        ValveStatus status{ValveStatus::Unknown};
        Timestamp updated{0};
    };
}