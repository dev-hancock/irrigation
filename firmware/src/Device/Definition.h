#pragma once

#include <Arduino.h>
#include <vector>

#include "Valves/Definition.h"

namespace Irrigation::Device
{
    struct Definition
    {
        const char *model;
        const char *version;
        const char *firmware;

        std::vector<Valve::Definition> valves;
    };
}