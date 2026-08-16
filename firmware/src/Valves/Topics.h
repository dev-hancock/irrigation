#pragma once

#include <Arduino.h>

namespace Irrigation::Valve
{
    class Topics
    {
    public:
        static constexpr const char *OpenValve = "valve/open";

        static constexpr const char *CloseValve = "valve/close";

        static constexpr const char *ResetValve = "valve/reset";

        static String ValveState(const ValveId id)
        {
            return "valve/" + String(id) + "/state";
        }
    };
}
