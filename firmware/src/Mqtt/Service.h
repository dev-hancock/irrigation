#pragma once

#include "Mqtt/Client.h"
#include "Mqtt/Consumer.h"
#include "Mqtt/Publisher.h"
#include "Mqtt/Options.h"

#include "WiFi/Client.h"
#include "Common/Api.h"

#include "Secrets.h"

namespace Irrigation::Mqtt
{
    class Service
    {
    public:
        Service(
            Router &router,
            WiFi::Client &wifi)
            : _client(wifi.socket()),
              _consumer(_client, router),
              _events(_client)
        {
        }

        Result begin()
        {
            Options options = Options::load();

            if (!options.isValid())
            {
                options.host = MQTT_HOST;
                options.port = MQTT_PORT;
                options.username = MQTT_USERNAME;
                options.password = MQTT_PASSWORD;

                if (!options.save())
                {
                    return Result::Failure("Failed to save MQTT options");
                }
            }

            Result result = _client.connect(options);

            if (result.isFailure())
            {
                return result;
            }

            Serial.println("MQTT initialized");

            return _consumer.begin();
        }

        void update()
        {
            if (!_client.isConnected())
            {
                Serial.println("MQTT disconnected, attempting to reconnect...");

                const Result result = _client.connect(Options::load());

                if (result.isFailure())
                {
                    Serial.println("Failed to reconnect to MQTT");
                    return;
                }
            }

            _consumer.update();
        }

        [[nodiscard]]
        IEvents &events()
        {
            return _events;
        }

    private:
        Client _client;
        Consumer _consumer;
        Publisher _events;
    };
}