#pragma once

#include <Arduino.h>
#include <ArduinoJson.h>

#include "Mqtt/Message.h"
#include "Mqtt/Client.h"

#include "Device.h"

namespace Irrigation::Mqtt
{
    class Publisher
    {
    public:
        explicit Publisher(Client &client)
            : _client(client)
        {
        }

        Result publish(
            const String &topic,
            const JsonDocument &payload)
        {
            Message message = Message(topic);

            serializeJson(payload, message.payload);

            return _client.publish(message);
        }

    private:
        String prefix() const
        {
            return String("irrigation/") +
                   Device::id() +
                   "/";
        }

        Client &_client;
    };
}