#include <vector>
#include <Arduino.h>

#pragma region Result

enum class Error
{
    None,

    ValveNotFound,
    ValveAlreadyExists,
    ValveAlreadyAssigned,
    ValveNotAssigned,
    ValveOpen,
    ValveHardwareFailure,

    ZoneNotFound,
    ZoneAlreadyExists,
    ZoneEmpty,
    ZoneNotEmpty,

    InvalidName,
    InvalidPin
};
class Result
{
public:
    static Result Success()
    {
        return Result(true, Error::None, {});
    }

    static Result Failure(
        const Error error,
        String message)
    {
        return Result(false, error, message);
    }

    [[nodiscard]]
    bool isSuccess() const
    {
        return _isSuccess;
    }

    [[nodiscard]]
    bool isFailure() const
    {
        return !_isSuccess;
    }

    [[nodiscard]]
    Error error() const
    {
        return _error;
    }

    [[nodiscard]]
    const String &message() const
    {
        return _message;
    }

private:
    Result(
        const bool isSuccess,
        const Error error,
        String message)
        : _isSuccess(isSuccess),
          _error(error),
          _message(message)
    {
    }

    bool _isSuccess;
    Error _error;
    String _message;
};

#pragma endregion

#pragma region State

class AppState
{
public:
    ValveState valves;
};

#pragma endregion

#pragma region Mediator

#pragma endregion

#pragma region API

class IMessageHandler
{
public:
    virtual ~IMessageHandler() = default;

    [[nodiscard]]
    virtual bool canHandle(const String &topic) const = 0;

    virtual void handle(
        const String &topic,
        const JsonDocument &payload) = 0;
};

class Router
{
public:
    Router(std::vector<IMessageHandler *> handlers)
        : _handlers(std::move(handlers))
    {
    }

    void add(IMessageHandler &handler)
    {
        // todo: remove this - use ctor
        _handlers.push_back(&handler);
    }

    [[nodiscard]]
    bool route(
        const String &topic,
        const JsonDocument &payload) const
    {
        for (IMessageHandler *handler : _handlers)
        {
            if (!handler->canHandle(topic))
            {
                continue;
            }

            handler->handle(topic, payload);
            return true;
        }

        return false;
    }

private:
    std::vector<IMessageHandler *> _handlers;
};

#pragma endregion

#pragma region Options

class MqttOptions;    // placeholder for mqtt options
class NetworkOptions; // placeholder for network options

class Options
{
public:
    MqttOptions mqtt;
    NetworkOptions network;

    void begin()
    {
        mqtt = MqttOptions::load();
        network = NetworkOptions::load();
    }

    void save() const
    {
        // somehow detect if options have changed and only save if they have changed
        mqtt.save();
        network.save();
    }
};

#pragma endregion
