#include <Arduino.h>
#include <Application.h>

namespace
{
    constexpr int BAUD_RATE = 115200;
    App app;
}

void setup()
{
    Serial.begin(BAUD_RATE);
    app.begin();
}

void loop()
{
    app.update();
}