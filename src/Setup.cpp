#include "Application.h"

#include <ArduinoJson.h>
#include <WiFi.h>
#include <WiFiManager.h>

#include <cstdlib>

// temp code needs refactoring so its fucking readable
namespace
{
    constexpr char SetupAccessPointName[] = "Irrigation Setup";
    constexpr char SetupAccessPointPassword[] = "irrigation-setup";
    constexpr uint32_t WifiConnectionTimeoutMs = 20000;
    constexpr uint32_t WifiRetryDelayMs = 250;
    constexpr uint32_t WifiReconnectDelayMs = 10000;
}

Application *Application::_instance = nullptr;

Application::Application()
    : _deviceId(deviceId()),
      _mqttClient(_deviceId, onMqttCommand)
{
    _instance = this;
}

void Application::begin()
{
    // Configure every known driver and force the physical valves closed before networking.
    _valveController.restore(_configurationStore.loadValves());
    _mqttClient.configure(_networkConfiguration.loadMqttCredentials());
    WiFi.mode(WIFI_STA);

    const WifiCredentials credentials = _networkConfiguration.loadWifiCredentials();
    if (!credentials.isValid())
    {
        Serial.println("No saved Wi-Fi credentials. Starting setup mode.");
        startSetupMode();
        return;
    }

    if (!connectToWifi(credentials))
    {
        Serial.println("Unable to connect with saved Wi-Fi credentials. Starting setup mode.");
        startSetupMode();
    }
}

void Application::update()
{
    if (WiFi.status() != WL_CONNECTED)
    {
        if (millis() - _lastWifiAttempt >= WifiReconnectDelayMs)
        {
            _lastWifiAttempt = millis();
            const WifiCredentials credentials = _networkConfiguration.loadWifiCredentials();
            if (credentials.isValid())
            {
                WiFi.disconnect();
                WiFi.begin(credentials.ssid.c_str(), credentials.password.c_str());
            }
        }
        return;
    }

    _mqttClient.update();
    if (_mqttClient.isConnected() && !_mqttWasConnected)
    {
        _mqttWasConnected = true;
        for (const Valve &valve : _valveController.valves())
        {
            publishValveState(valve.id);
        }
        _mqttClient.publishEvent("ready", _deviceId);
    }
    else if (!_mqttClient.isConnected())
    {
        _mqttWasConnected = false;
    }
}

bool Application::connectToWifi(const WifiCredentials &credentials)
{
    Serial.printf("Connecting to Wi-Fi network '%s'.\n", credentials.ssid.c_str());
    WiFi.begin(credentials.ssid.c_str(), credentials.password.c_str());

    const uint32_t startedAt = millis();
    while (WiFi.status() != WL_CONNECTED && millis() - startedAt < WifiConnectionTimeoutMs)
    {
        delay(WifiRetryDelayMs);
    }

    if (WiFi.status() != WL_CONNECTED)
    {
        WiFi.disconnect();
        return false;
    }

    Serial.printf("Connected to Wi-Fi. IP address: %s\n", WiFi.localIP().toString().c_str());
    return true;
}

void Application::startSetupMode()
{
    WiFiManager wifiManager;
    wifiManager.setConfigPortalBlocking(true);
    const MqttCredentials existingMqtt = _networkConfiguration.loadMqttCredentials();
    char mqttPort[6];
    snprintf(mqttPort, sizeof(mqttPort), "%u", existingMqtt.port);
    WiFiManagerParameter mqttHost("mqttHost", "MQTT broker host", existingMqtt.host.c_str(), 64);
    WiFiManagerParameter mqttPortParameter("mqttPort", "MQTT broker port", mqttPort, 6);
    WiFiManagerParameter mqttUser("mqttUser", "MQTT username (optional)", existingMqtt.username.c_str(), 64);
    WiFiManagerParameter mqttPassword("mqttPassword", "MQTT password (optional)", "", 64, "type=\"password\"");
    wifiManager.addParameter(&mqttHost);
    wifiManager.addParameter(&mqttPortParameter);
    wifiManager.addParameter(&mqttUser);
    wifiManager.addParameter(&mqttPassword);

    Serial.printf("Connect to '%s' to configure Wi-Fi and MQTT.\n", SetupAccessPointName);
    if (!wifiManager.startConfigPortal(SetupAccessPointName, SetupAccessPointPassword))
    {
        Serial.println("Wi-Fi setup did not complete.");
        return;
    }

    const WifiCredentials credentials{
        .ssid = wifiManager.getWiFiSSID(),
        .password = wifiManager.getWiFiPass()};

    if (!_networkConfiguration.saveWifiCredentials(credentials))
    {
        Serial.println("Failed to save Wi-Fi credentials.");
        return;
    }

    const uint16_t port = static_cast<uint16_t>(strtoul(mqttPortParameter.getValue(), nullptr, 10));
    MqttCredentials mqttCredentials{
        .host = String(mqttHost.getValue()),
        .port = port,
        .username = String(mqttUser.getValue()),
        .password = String(mqttPassword.getValue())};
    if (!mqttCredentials.isValid() || !_networkConfiguration.saveMqttCredentials(mqttCredentials))
    {
        Serial.println("Failed to save MQTT configuration.");
        return;
    }

    Serial.println("Wi-Fi credentials saved. Restarting device.");
    delay(250);
    ESP.restart();
}

void Application::onMqttCommand(const String &topic, const uint8_t *payload, size_t length)
{
    if (_instance != nullptr)
    {
        _instance->handleMqttCommand(topic, payload, length);
    }
}

void Application::handleMqttCommand(const String &topic, const uint8_t *payload, size_t length)
{
    const String commandPrefix = "irrigation/" + _deviceId + "/command/";
    if (!topic.startsWith(commandPrefix))
    {
        return;
    }

    const String command = topic.substring(commandPrefix.length());
    String error;
    if (command == "emergency-stop")
    {
        _valveController.closeAll();
        for (const Valve &valve : _valveController.valves())
        {
            publishValveState(valve.id);
        }
        _mqttClient.publishEvent("emergency-stop", "all valves closed");
        return;
    }

    if (command == "valves/add")
    {
        DynamicJsonDocument document(512);
        if (deserializeJson(document, payload, length))
        {
            publishError(command, "Payload must be JSON with id, in1Pin, and in2Pin.");
            return;
        }

        const char *id = document["id"] | "";
        const int in1Pin = document["in1Pin"] | -1;
        const int in2Pin = document["in2Pin"] | -1;
        if (in1Pin < 0 || in1Pin > 255 || in2Pin < 0 || in2Pin > 255 ||
            !_valveController.add(String(id), static_cast<uint8_t>(in1Pin), static_cast<uint8_t>(in2Pin), error))
        {
            publishError(command, error);
            return;
        }

        if (!_configurationStore.saveValves(_valveController.valves()))
        {
            publishError(command, "Valve was added but its configuration could not be saved.");
            return;
        }
        publishValveState(String(id));
        return;
    }

    if (!command.startsWith("valves/"))
    {
        publishError(command, "Unsupported command.");
        return;
    }

    const int actionSeparator = command.lastIndexOf('/');
    if (actionSeparator <= static_cast<int>(String("valves/").length()))
    {
        publishError(command, "Malformed valve command.");
        return;
    }

    const String valveId = command.substring(String("valves/").length(), actionSeparator);
    const String action = command.substring(actionSeparator + 1);
    bool succeeded = false;
    if (action == "open")
    {
        succeeded = _valveController.open(valveId, error);
    }
    else if (action == "close")
    {
        succeeded = _valveController.close(valveId, error);
    }
    else if (action == "remove")
    {
        succeeded = _valveController.remove(valveId, error);
        if (succeeded && !_configurationStore.saveValves(_valveController.valves()))
        {
            error = "Valve was removed but its configuration could not be saved.";
            succeeded = false;
        }
    }
    else
    {
        error = "Unsupported valve command.";
    }

    if (!succeeded)
    {
        publishError(command, error);
        return;
    }

    if (action == "remove")
    {
        _mqttClient.publishEvent("valve-removed", valveId);
    }
    else
    {
        publishValveState(valveId);
    }
}

void Application::publishValveState(const String &valveId)
{
    const Valve *valve = _valveController.find(valveId);
    if (valve != nullptr)
    {
        _mqttClient.publishState(valve->id, valve->state == ValveState::Open);
    }
}

void Application::publishError(const String &command, const String &message)
{
    DynamicJsonDocument document(384);
    document["command"] = command;
    document["error"] = message;
    String payload;
    serializeJson(document, payload);
    _mqttClient.publishEvent("error", payload);
}

String Application::deviceId() const
{
    char identifier[20];
    snprintf(identifier, sizeof(identifier), "esp32-%012llx", ESP.getEfuseMac());
    return String(identifier);
}
