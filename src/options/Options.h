#include <Preferences.h>

class MqttOptions
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
    static const MqttOptions load()
    {
        Preferences preferences;
        if (!preferences.begin("mqtt", true))
        {
            return {};
        }

        MqttOptions options(
            preferences.getString("mqtt_host", ""),
            preferences.getUShort("mqtt_port", 1883),
            preferences.getString("mqtt_username", ""),
            preferences.getString("mqtt_password", ""));

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

private:
    MqttOptions() = default;

    MqttOptions(const String &host,
                uint16_t port,
                const String &username,
                const String &password)
        : host(host),
          port(port),
          username(username),
          password(password)
    {
    }
};

class NetworkOptions
{
public:
    String ssid;
    String password;

    [[nodiscard]]
    bool isValid() const
    {
        return !ssid.isEmpty();
    }

    [[nodiscard]]
    static const NetworkOptions load()
    {
        Preferences preferences;
        if (!preferences.begin("network", true))
        {
            return {};
        }

        NetworkOptions options(
            preferences.getString("network_ssid", ""),
            preferences.getString("network_password", ""));

        preferences.end();

        return options;
    }

    void clear() const
    {
        Preferences preferences;
        if (!preferences.begin("network", false))
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
        if (!preferences.begin("network", false))
        {
            return false;
        }

        const bool saved =
            preferences.putString("network_ssid", ssid) > 0 &&
            preferences.putString("network_password", password) >= 0;

        preferences.end();

        return saved;
    }

private:
    NetworkOptions() = default;

    NetworkOptions(const String &ssid,
                   const String &password)
        : ssid(ssid),
          password(password)
    {
    }
};
