#pragma once

#include <Arduino.h>

#include "Common/Api.h"
#include "Common/Result.h"

#include "Valves/Handlers/Open.h"
#include "Valves/Handlers/Close.h"
#include "Valves/Handlers/Reset.h"
#include "Valves/Topics.h"

namespace Irrigation::Valve
{
    class MessageHandler final : public IMessageHandler
    {
    public:
        MessageHandler(
            OpenHandler &open,
            CloseHandler &close,
            ResetHandler &reset)
            : _open(open),
              _close(close),
              _reset(reset)
        {
        }

        bool canHandle(const String &topic) override
        {
            return topic == Topics::OpenValve ||
                   topic == Topics::CloseValve ||
                   topic == Topics::ResetValve;
        }

        Result handle(const Message &message) override
        {
            if (message.topic == Topics::ResetValve)
            {
                return _reset.handle();
            }

            JsonDocument request;

            if (deserializeJson(request, message.payload))
            {
                return Result::Failure("Invalid JSON");
            }

            if (message.topic == Topics::OpenValve)
            {
                return _open.handle(request);
            }

            if (message.topic == Topics::CloseValve)
            {
                return _close.handle(request);
            }

            return Result::Failure("Unknown command");
        }

    private:
        OpenHandler &_open;
        CloseHandler &_close;
        ResetHandler &_reset;
    };
}