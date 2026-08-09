#pragma once

#include <Arduino.h>

namespace Irrigation::Mqtt
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
}
