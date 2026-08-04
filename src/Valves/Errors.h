#pragma once

#include "Common/Result.h"

namespace Irrigation::Valve
{
    namespace Errors
    {
        inline Result InvalidState()
        {
            return Result::Failure("Invalid valve state");
        }

        inline Result NotFound()
        {
            return Result::Failure("Valve not found");
        }
    }
}