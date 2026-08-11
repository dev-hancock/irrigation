#include <Arduino.h>

#include "App.h"

namespace Irrigation
{
    constexpr uint32_t BaudRate = 115200;
    App app;
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