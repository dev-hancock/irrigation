#pragma once

#include <ArduinoJson.h>
#include "options/options.h"

struct MqttState
{
    MqttOptions options;

    MqttConnectionState connection = MqttConnectionState::Disconnected;

    [[nodiscard]]
    bool isConnected() const
    {
        return connection == MqttConnectionState::Connected;
    }

    [[nodiscard]]
    bool isConfigured() const
    {
        return options.isValid();
    }
};