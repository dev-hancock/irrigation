#pragma once

#include <Arduino.h>
#include <ArduinoJson.h>
#include <vector>

#include "Common/Result.h"

namespace Irrigation
{
    class IMessageHandler
    {
    public:
        virtual const char *topic() const = 0;
        virtual Result handle(const JsonDocument &payload) = 0;
    };

    class Router
    {
    public:
        void add(IMessageHandler &handler)
        {
            _handlers.push_back(&handler);
        }

        [[nodiscard]]
        bool route(
            const String &topic,
            const JsonDocument &payload) const
        {
            for (IMessageHandler *handler : _handlers)
            {
                if (String(handler->topic()) == topic)
                {
                    handler->handle(payload);
                    return true;
                }
            }

            return false;
        }

    private:
        std::vector<IMessageHandler *> _handlers;
    };
}