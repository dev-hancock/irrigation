#pragma once

#include <Arduino.h>
#include <ArduinoJson.h>

#include "Common/Api.h"

#include "Mqtt/Client.h"

#include "Device/Id.h"

namespace Irrigation::Mqtt
{
    class Publisher final : public IEvents
    {
    public:
        explicit Publisher(Client &client)
            : _client(client)
        {
        }

        Result publish(
            const String &topic,
            const JsonDocument &payload,
            const bool retain = false) override
        {
            Message message{
                .topic = prefix() + topic,
                .payload = ""};

            serializeJson(payload, message.payload);

            return _client.publish(message, retain);
        }

    private:
        [[nodiscard]]
        String prefix() const
        {
            return "irrigation/" + Device::Id() + "/event/";
        }

        Client &_client;
    };
}