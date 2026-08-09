#pragma once

#include <Arduino.h>

#include "Common/Api.h"
#include "Common/Result.h"

#include "Valves/Handlers/Open.h"
#include "Valves/Handlers/Close.h"
#include "Valves/Handlers/Reset.h"

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
            return topic.startsWith("valve/");
        }

        Result handle(const Message &message) override
        {
            JsonDocument request;

            if (deserializeJson(request, message.payload))
            {
                return Result::Failure("Invalid JSON");
            }

            if (message.topic == "valve/open")
            {
                return _open.handle(request);
            }

            if (message.topic == "valve/close")
            {
                return _close.handle(request);
            }

            if (message.topic == "valve/reset")
            {
                return _reset.handle();
            }

            return Result::Failure("Unknown valve command");
        }

    private:
        OpenHandler &_open;
        CloseHandler &_close;
        ResetHandler &_reset;
    };
}