#pragma once

#include <Arduino.h>
#include <vector>

namespace Irrigation::Valve
{
    enum class ValveStatus
    {
        Unknown,
        Open,
        Closed
    };

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

    using ValveId = String;

    using Duration = unsigned long;

    using Pin = uint8_t;

    struct Valve
    {
        ValveId id;
        Pin in1Pin;
        Pin in2Pin;
        Duration durationMs;
    };

    using Timestamp = unsigned long;

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