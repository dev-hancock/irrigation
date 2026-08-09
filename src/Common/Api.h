#pragma once

#include <Arduino.h>
#include <ArduinoJson.h>

#include "Common/Result.h"

namespace Irrigation
{
    struct Message
    {
        String topic;
        String payload;
    };

    class IMessageHandler
    {
    public:
        // virtual const char *topic() const = 0;
        virtual bool canHandle(const String &topic) = 0;
        virtual Result handle(const Message &message) = 0;
    };

    class Router
    {
    public:
        Router() = default;

        void add(IMessageHandler &handler)
        {
            _handlers.push_back(&handler);
        }

        Result route(const Message &message)
        {
            Serial.print("Routing message to topic: ");
            Serial.println(message.topic);

            for (IMessageHandler *handler : _handlers)
            {
                if (handler->canHandle(message.topic))
                {
                    Result result = handler->handle(message);

                    return result;
                }
            }

            return Result::Failure("Route not found");
        }

    private:
        std::vector<IMessageHandler *> _handlers;
    };

    class IEvents
    {
    public:
        virtual ~IEvents() = default;

        virtual Result publish(
            const String &topic,
            const JsonDocument &payload,
            const bool retain = false) = 0;
    };
}
