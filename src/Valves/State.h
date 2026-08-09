#pragma once

#include <Arduino.h>
#include <vector>

#include "Valves/Types.h"

namespace Irrigation::Valve
{
    class State
    {
    public:
        State() = default;

        explicit State(const std::vector<Valve> &valves)
        {
            _valves.reserve(valves.size());

            for (const Valve &valve : valves)
            {
                _valves.emplace_back(valve);
            }
        }

        ValveEntry *find(const ValveId &valveId)
        {
            for (ValveEntry &entry : _valves)
            {
                if (entry.valve.id == valveId)
                {
                    return &entry;
                }
            }

            return nullptr;
        }

    private:
        std::vector<ValveEntry> _valves;
    };
}