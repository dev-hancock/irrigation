#include <Arduino.h>

#include "App.h"

#include "Device/Definition.h"
#include "Valves/Definition.h"

namespace Irrigation
{
    constexpr uint32_t BaudRate = 115200;

    const Device::Definition device{
        .model = "irrigation-controller",
        .version = "1.0",
        .firmware = "0.1.0",
        .valves = {
            Valve::Definition{
                .id = 0,
                .in1Pin = 18,
                .in2Pin = 19,
                .durationMs = 20},
            Valve::Definition{
                .id = 1,
                .in1Pin = 21,
                .in2Pin = 22,
                .durationMs = 20},
            Valve::Definition{
                .id = 2,
                .in1Pin = 23,
                .in2Pin = 25,
                .durationMs = 20},
            Valve::Definition{
                .id = 3,
                .in1Pin = 26,
                .in2Pin = 27,
                .durationMs = 20},
        }};

    App app(device);
}

void setup()
{
    Serial.begin(Irrigation::BaudRate);

    const Irrigation::Result result = Irrigation::app.begin();

    if (result.isFailure())
    {
        Serial.println(result.message());
    }
}

void loop()
{
    Irrigation::app.update();
}