#pragma once

#include <Preferences.h>

namespace Irrigation::WiFi
{
    class Options
    {
    public:
        Options() = default;

        String ssid;
        String password;

        [[nodiscard]]
        bool isValid() const
        {
            return !ssid.isEmpty();
        }

        [[nodiscard]]
        static Options load()
        {
            Preferences preferences;
            if (!preferences.begin("wifi", true))
            {
                return {};
            }

            Options options(
                preferences.getString("wifi_ssid", ""),
                preferences.getString("wifi_password", ""));

            preferences.end();

            return options;
        }

        void clear() const
        {
            Preferences preferences;
            if (!preferences.begin("wifi", false))
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
            if (!preferences.begin("wifi", false))
            {
                return false;
            }

            const bool saved =
                preferences.putString("wifi_ssid", ssid) > 0 &&
                preferences.putString("wifi_password", password) >= 0;

            preferences.end();

            return saved;
        }

    private:
        Options(const String &ssid,
                const String &password)
            : ssid(ssid),
              password(password)
        {
        }
    };
}