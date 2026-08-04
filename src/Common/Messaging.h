#pragma once

#include <ArduinoJson.h>

#include "Common/Result.h"
#include <vector>

namespace Irrigation
{
  


    
    class IHandler
    {
    public:
        virtual ~IHandler() = default;
    };

    template <typename TCommand>
    class ICommandHandler : public IHandler
    {
    public:
        virtual Result handle(const TCommand &command) = 0;
    };

    template <typename TEvent>
    class IEventHandler : public IHandler
    {
    public:
        virtual void handle(const TEvent &event) = 0;
    };

    class Commands
    {
    public:
        template <typename TCommand>
        void add(ICommandHandler<TCommand> &handler)
        {
            _handlers.push_back(&handler);
        }

        template <typename TCommand>
        Result dispatch(const TCommand &command)
        {
            for (IHandler *handler : _handlers)
            {
                auto *handler =
                    dynamic_cast<ICommandHandler<TCommand> *>(
                        handler);

                if (handler != nullptr)
                {
                    return handler->handle(command);
                }
            }

            return Result::Failure(
                Error::CommandNotHandled,
                "No handler found for command");
        }

    private:
        std::vector<IHandler *> _handlers;
    };

    class Events
    {
    public:
        template <typename TEvent>
        void add(IEventHandler<TEvent> &handler)
        {
            _handlers.push_back(&handler);
        }

        template <typename TEvent>
        void publish(const TEvent &event)
        {
            for (IHandler *handler : _handlers)
            {
                auto *handler =
                    dynamic_cast<IEventHandler<TEvent> *>(
                        handler);

                if (handler != nullptr)
                {
                    handler->handle(event);
                }
            }
        }

    private:
        std::vector<IHandler *> _handlers;
    };
}