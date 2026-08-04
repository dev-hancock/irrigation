#include "MqttClient.h"

MqttClient *MqttClient::_instance = nullptr;

MqttClient::MqttClient(const String &deviceId, CommandHandler commandHandler)
    : _client(_wifiClient),
      _deviceId(deviceId),
      _prefix("irrigation/" + deviceId),
      _commandHandler(commandHandler)
{
    _instance = this;
    _client.setCallback(onMessage);
}

void MqttClient::configure(const MqttCredentials &credentials)
{
    _credentials = credentials;
    if (credentials.isValid())
    {
        _client.setServer(credentials.host.c_str(), credentials.port);
    }
}

void MqttClient::update()
{
    if (!_credentials.isValid() || WiFi.status() != WL_CONNECTED)
    {
        return;
    }

    if (!_client.connected())
    {
        if (millis() - _lastConnectAttempt >= 5000)
        {
            _lastConnectAttempt = millis();
            connect();
        }
        return;
    }

    _client.loop();
}

void MqttClient::publishState(const String &valveId, bool isOpen)
{
    if (_client.connected())
    {
        _client.publish(topic("state/valves/" + valveId).c_str(), isOpen ? "open" : "closed", true);
    }
}

void MqttClient::publishEvent(const char *eventName, const String &payload)
{
    if (_client.connected())
    {
        _client.publish(topic("events/" + String(eventName)).c_str(), payload.c_str(), false);
    }
}

bool MqttClient::isConnected() const
{
    return _client.connected();
}

void MqttClient::onMessage(char *topic, uint8_t *payload, unsigned int length)
{
    if (_instance != nullptr && _instance->_commandHandler != nullptr)
    {
        _instance->_commandHandler(String(topic), payload, length);
    }
}

bool MqttClient::connect()
{
    const String statusTopic = topic("status");
    const bool hasCredentials = !_credentials.username.isEmpty();
    const bool connected = hasCredentials
        ? _client.connect(_deviceId.c_str(), _credentials.username.c_str(), _credentials.password.c_str(), statusTopic.c_str(), 1, true, "offline")
        : _client.connect(_deviceId.c_str(), statusTopic.c_str(), 1, true, "offline");
    if (!connected)
    {
        return false;
    }

    _client.publish(statusTopic.c_str(), "online", true);
    _client.subscribe(topic("command/#").c_str());
    return true;
}

String MqttClient::topic(const String &suffix) const
{
    return _prefix + "/" + suffix;
}