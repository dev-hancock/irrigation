#pragma once

#include <Arduino.h>

#include "Common/Result.h"
#include "Valves/Types.h"

namespace Irrigation::Valve
{
    class Driver
    {
    public:
        void begin(const Valve &valve)
        {
            pinMode(valve.in1Pin, OUTPUT);
            pinMode(valve.in2Pin, OUTPUT);

            stop(valve);
        }

        Result open(const Valve &valve)
        {
            digitalWrite(valve.in1Pin, HIGH);
            digitalWrite(valve.in2Pin, LOW);

            delay(valve.durationMs);

            stop(valve);

            return Result::Success();
        }

        Result close(const Valve &valve)
        {
            digitalWrite(valve.in1Pin, LOW);
            digitalWrite(valve.in2Pin, HIGH);

            delay(valve.durationMs);

            stop(valve);

            return Result::Success();
        }

    private:
        void stop(const Valve &valve)
        {
            digitalWrite(valve.in1Pin, LOW);
            digitalWrite(valve.in2Pin, LOW);
        }
    };
}