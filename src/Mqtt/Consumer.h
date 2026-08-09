#pragma once

#include <Arduino.h>
#include <ArduinoJson.h>

#include "Mqtt/Client.h"

#include "Common/Api.h"
#include "Common/Result.h"

#include "Device.h"

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

            consume(message);
        }

    private:
        void consume(const Message &message)
        {
            const String topic = message.topic;

            if (!topic.startsWith(prefix()))
            {
                return;
            }

            JsonDocument payload;

            const DeserializationError error =
                deserializeJson(payload, message.payload);

            if (error)
            {
                return;
            }

            String route = topic.substring(prefix().length());

            if (route.isEmpty())
            {
                return;
            }

            _router.route(route, payload);
        }

        [[nodiscard]]
        String prefix() const
        {
            return String("irrigation/") +
                   Device::id() +
                   "/";
        }

        Client &_client;
        Router &_router;
    };
}