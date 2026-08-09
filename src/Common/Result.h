#pragma once

#include <Arduino.h>
#include <utility>

#include "Error.h"

namespace Irrigation
{
    class Result
    {
    public:
        static Result Success()
        {
            return Result(true, Error::None, {});
        }

        static Result Failure(
            String message)
        {
            return Result(false, Error::None, message);
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
            return _success;
        }

        [[nodiscard]]
        bool isFailure() const
        {
            return !_success;
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
            const bool success,
            const Error error,
            String message)
            : _success(success),
              _error(error),
              _message(message)
        {
        }

        bool _success;
        Error _error;
        String _message;
    };
}