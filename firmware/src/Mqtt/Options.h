#pragma once

#include <Preferences.h>

namespace Irrigation::Mqtt
{

    class Options
    {
    public:
        String host;
        uint16_t port = 1883;
        String username;
        String password;

        [[nodiscard]]
        bool isValid() const
        {
            return !host.isEmpty() && port > 0;
        }

        [[nodiscard]]
        static const Options load()
        {
            Preferences preferences;
            if (!preferences.begin("mqtt", true))
            {
                return {};
            }

            Options options;

            options.host = preferences.getString("mqtt_host", "");
            options.port = preferences.getUShort("mqtt_port", 1883);
            options.username = preferences.getString("mqtt_username", "");
            options.password = preferences.getString("mqtt_password", "");

            preferences.end();

            return options;
        }

        void clear()
        {
            Preferences preferences;
            if (!preferences.begin("mqtt", false))
            {
                return;
            }

            preferences.clear();

            preferences.end();
        }

        bool save() const
        {
            if (!isValid())
            {
                return false;
            }

            Preferences preferences;
            if (!preferences.begin("mqtt", false))
            {
                return false;
            }

            const bool saved =
                preferences.putString("mqtt_host", host) > 0 &&
                preferences.putUShort("mqtt_port", port) > 0;

            preferences.putString("mqtt_username", username);
            preferences.putString("mqtt_password", password);

            preferences.end();

            return saved;
        }
    };
}
