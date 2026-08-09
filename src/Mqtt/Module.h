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
    class Module
    {
    public:
        Module(
            Router &router,
            WiFi::Client &wifi)
            : _client(wifi.socket()),
              _consumer(_client, router),
              _publisher(_client)
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

            return _consumer.begin();
        }

        void update()
        {
            if (!_client.isConnected())
            {
                const Result result = _client.connect(Options::load());

                if (result.isFailure())
                {
                    return;
                }
            }

            _consumer.update();
        }

        [[nodiscard]]
        Publisher &publisher()
        {
            return _publisher;
        }

    private:
        Client _client;
        Consumer _consumer;
        Publisher _publisher;
    };
}