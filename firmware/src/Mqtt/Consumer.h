#pragma once

#include <Arduino.h>
#include <ArduinoJson.h>

#include "Mqtt/Client.h"

#include "Common/Api.h"
#include "Common/Result.h"

#include "Device/Id.h"

namespace Irrigation::Mqtt
{
    class Consumer
    {
    public:
        Consumer(
            Client &client,
            Router &router)
            : _client(client),
              _router(router)
        {
        }

        Result begin()
        {
            return _client.subscribe(prefix() + "#");
        }

        void update()
        {
            Message message;

            if (!_client.receive(message))
            {
                return;
            }

            const Result result = consume(message);

            if (result.isFailure())
            {
                Serial.print("Failed to consume MQTT message: ");
                Serial.println(result.message());
            }
        }

    private:
        Result consume(const Message &message)
        {
            const String base = prefix();

            if (!message.topic.startsWith(base))
            {
                return Result::Failure(
                    "Unexpected MQTT topic");
            }

            Message routed = message;

            routed.topic =
                message.topic.substring(
                    base.length());

            if (routed.topic.isEmpty())
            {
                return Result::Failure(
                    "Missing MQTT route");
            }

            return _router.route(routed);
        }

        [[nodiscard]]
        String prefix() const
        {
            return "irrigation/" + Device::id() + "/command/";
        }

        Client &_client;
        Router &_router;
    };
}