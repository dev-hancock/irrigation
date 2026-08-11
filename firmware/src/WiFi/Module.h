#pragma once

#include <WiFi.h>

#include "WiFi/Client.h"
#include "WiFi/Options.h"

#include "Common/Api.h"

#include "Secrets.h"

namespace Irrigation::WiFi
{
    class Module
    {
    public:
        Result begin()
        {
            auto options = Options::load();

            if (!options.isValid())
            {
                options.ssid = WIFI_SSID;
                options.password = WIFI_PASSWORD;

                if (!options.save())
                {
                    return Result::Failure("Failed to save WiFi options");
                }
            }

            _options = options;

            Serial.println("WiFi module initialized");

            return _client.connect(_options);
        }

        void update()
        {
            if (_client.isConnected())
            {
                return;
            }

            if (millis() - _retry < 5000)
            {
                return;
            }

            Serial.println("WiFi disconnected, attempting to reconnect...");

            _client.connect(_options);

            _retry = millis();
        }

        [[nodiscard]]
        Client &client()
        {
            return _client;
        }

    private:
        Client _client;
        unsigned long _retry = 0;
        Options _options;
    };
}