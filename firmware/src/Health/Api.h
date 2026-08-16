#pragma once

#include <Arduino.h>

#include "Common/Api.h"
#include "Common/Result.h"

#include "Health/Handlers/Heartbeat.h"
#include "Health/Topics.h"

namespace Irrigation::Health
{
    class MessageHandler final : public IMessageHandler
    {
    public:
        MessageHandler(
            HeartbeatHandler &heartbeat)
            : _heartbeat(heartbeat)
        {
        }

        bool canHandle(const String &topic) override
        {
            return topic == Topics::Ping;
        }

        Result handle(const Message &message) override
        {
            if (message.topic == Topics::Ping)
            {
                return _heartbeat.handle();
            }

            return Result::Failure("Unknown command");
        }

    private:
        HeartbeatHandler &_heartbeat;
    };
}