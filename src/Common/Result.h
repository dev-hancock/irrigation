#pragma once

#include <Arduino.h>
#include <utility>

namespace Irrigation
{
    class Result
    {
    public:
        static Result Success()
        {
            return Result(true, {});
        }

        static Result Failure(
            String message)
        {
            return Result(false, message);
        }

        [[nodiscard]]
        bool isSuccess() const
        {
            return _success;
        }

        [[nodiscard]]
        bool isFailure() const
        {
            return !_success;
        }

        [[nodiscard]]
        const String &message() const
        {
            return _message;
        }

    private:
        Result(
            const bool success,
            String message)
            : _success(success),
              _message(message)
        {
        }

        bool _success;
        String _message;
    };
}