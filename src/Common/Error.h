#pragma once

namespace Irrigation
{
    enum class Error
    {
        None,

        NotFound,
        AlreadyExists,
        AlreadyAssigned,
        NotAssigned,
        Open,
        HardwareFailure,

        InvalidName,
        InvalidPin
    };
}