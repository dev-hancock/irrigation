#pragma once

#include <ArduinoMqttClient.h>

#include "Mqtt/Options.h"

#include "Common/Result.h"
#include "Common/Api.h"

namespace Irrigation::Mqtt
{
    class Client
    {
    public:
        Client(::Client &socket)
            : _client(socket)
        {
        }

        Result connect(const Options &options)
        {
            if (!options.isValid())
            {
                return Result::Failure(
                    "Invalid MQTT options");
            }

            if (!options.username.isEmpty())
            {
                _client.setUsernamePassword(
                    options.username.c_str(),
                    options.password.c_str());
            }

            if (!_client.connect(
                    options.host.c_str(),
                    options.port))
            {
                return Result::Failure(
                    "Failed to connect to MQTT broker");
            }

            Serial.print("Connected to MQTT broker: ");
            Serial.print(options.host);
            Serial.print(":");
            Serial.println(options.port);

            return Result::Success();
        }

        void disconnect()
        {
            _client.stop();

            Serial.println("Disconnected from MQTT broker");
        }

        [[nodiscard]]
        bool isConnected()
        {
            return _client.connected();
        }

        Result subscribe(const String &topic)
        {
            if (!_client.subscribe(topic))
            {
                return Result::Failure(
                    "Failed to subscribe");
            }

            Serial.print("Subscribed to MQTT topic: ");
            Serial.println(topic);

            return Result::Success();
        }

        [[nodiscard]]
        bool receive(Message &message)
        {
            if (!isConnected())
            {
                return false;
            }

            const int size = _client.parseMessage();

            if (size <= 0)
            {
                return false;
            }

            message.topic = _client.messageTopic();

            message.payload.clear();
            message.payload.reserve(size);

            while (_client.available())
            {
                message.payload +=
                    static_cast<char>(
                        _client.read());
            }

            Serial.print("Received MQTT message: ");
            Serial.print(message.topic);
            Serial.print(" (");
            Serial.print(size);
            Serial.print(" bytes): ");
            Serial.println(message.payload);

            return true;
        }

        Result publish(const Message &message, const bool retain = false)
        {
            if (!isConnected())
            {
                return Result::Failure(
                    "MQTT is not connected");
            }

            if (!_client.beginMessage(
                    message.topic,
                    message.payload.length(),
                    retain))
            {
                return Result::Failure(
                    "Failed to begin MQTT message");
            }

            const size_t written =
                _client.write(
                    reinterpret_cast<const uint8_t *>(
                        message.payload.c_str()),
                    message.payload.length());

            if (written != message.payload.length())
            {
                _client.endMessage();

                return Result::Failure(
                    "Failed to write MQTT message");
            }

            if (!_client.endMessage())
            {
                return Result::Failure(
                    "Failed to publish MQTT message");
            }

            Serial.print("Published MQTT message: ");
            Serial.print(message.topic);
            Serial.print(" (");
            Serial.print(written);
            Serial.print(" bytes): ");
            Serial.println(message.payload);

            return Result::Success();
        }

    private:
        MqttClient _client;
    };
}