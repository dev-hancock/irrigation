#pragma once

#include <Arduino.h>
#include <ArduinoJson.h>

#include "Common/Api.h"

#include "Mqtt/Publisher.h"

namespace Irrigation::Mqtt
{
    class Events final : public IEvents
    {
    public:
        Events(Publisher &publisher)
            : _publisher(publisher)
        {
        }

        void publish(const String &topic, const JsonDocument &payload) override
        {
            _publisher.publish(topic, payload);
        }

    private:
        Publisher &_publisher;
    };
}