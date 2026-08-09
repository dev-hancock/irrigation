#pragma once

#include <WiFiClient.h>

#include "WiFi/Options.h"

namespace Irrigation::WiFi
{
    class Client
    {
    public:
        explicit Client()
            : _client()
        {
        }

        Result connect(const Options &options)
        {
            ::WiFi.begin(options.ssid.c_str(), options.password.c_str());

            const unsigned long started = millis();

            while (::WiFi.status() != WL_CONNECTED)
            {
                if (millis() - started >= 10000)
                {
                    return Result::Failure("Failed to connect to WiFi");
                }

                delay(100);
            }

            Serial.print("Connected to WiFi network: ");
            Serial.println(options.ssid);

            return Result::Success();
        }

        void disconnect()
        {
            ::WiFi.disconnect();
        }

        [[nodiscard]]
        bool isConnected() const
        {
            return ::WiFi.status() == WL_CONNECTED;
        }

        [[nodiscard]]
        ::Client &socket()
        {
            return _client;
        }

    private:
        WiFiClient _client;
    };
}