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

        Message() = default;

        explicit Message(const String &topic)
            : topic(topic)
        {
        }

        Message(
            const String &topic,
            const String &payload)
            : topic(topic),
              payload(payload)
        {
        }

        const unsigned long length() const
        {
            return payload.length();
        }
    };

    class IHandler
    {
    public:
        virtual const char *topic() const = 0;
        virtual Result handle(const JsonDocument &payload) = 0;
    };

    class Router
    {
    public:
        Router() = default;

        void add(IHandler &handler)
        {
            _handlers.push_back(&handler);
        }

        Result route(const String &topic, const JsonDocument &payload)
        {
            for (IHandler *handler : _handlers)
            {
                if (handler->topic() == topic.c_str())
                {
                    return handler->handle(payload);
                }
            }

            return Result::Failure("Route not found");
        }

    private:
        std::vector<IHandler *> _handlers;
    };

    class IEvents
    {
    public:
        virtual ~IEvents() = default;

        virtual void publish(
            const String &topic,
            const JsonDocument &payload) = 0;
    };
}
