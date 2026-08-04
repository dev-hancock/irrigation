# Product Requirements Document (PRD)

## Project

**ESP32 Irrigation Controller Firmware**

## Overview

Develop firmware for an ESP32 that exposes a reliable MQTT API for controlling irrigation valves. The ESP32 is **not** responsible for scheduling, zones, weather, automation, or business logic. Those responsibilities belong to a Raspberry Pi running a .NET backend.

The ESP32 should behave as a secure network-connected hardware controller.

---

# Goals

The firmware shall:

* Control irrigation valves.
* Expose a clean MQTT command API.
* Publish state and events.
* Safely recover from power and network failures.
* Support secure Wi-Fi provisioning.
* Be reusable for future ESP32-based projects.

---

# Non-Goals

The firmware shall **not** implement:

* Zone logic
* Watering schedules
* Cron expressions
* Weather integration
* Soil moisture decisions
* Database persistence
* User interface
* Automation rules

These belong to the Raspberry Pi.

---

# High Level Architecture

```text
                    Raspberry Pi

        ASP.NET Core
              │
        Irrigation Domain
              │
            MQTT
              │
──────────────────────────────────────────────
              │
          ESP32 Firmware
              │
      MQTT Command Handler
              │
      Valve Controller
              │
      Valve Driver (GPIO)
              │
          Solenoid Valve
```

---

# Functional Requirements

## Device Identity

Each ESP32 shall expose a permanent controller identifier derived from the manufacturer chip ID.

The identifier shall never change.

Example

```
esp32-84f703c1a920
```

This identifier shall be used for:

* MQTT client id
* MQTT topics
* Device registration

---

## Valve Management

The firmware shall support:

* Add valve
* Remove valve
* Open valve
* Close valve
* Emergency stop
* Close all valves

Each valve shall contain only:

```
ValveId
GPIO configuration
State
```

Valve names shall not exist on the ESP32.

---

## MQTT API

Commands

```
irrigation/{deviceId}/command/valves/add

irrigation/{deviceId}/command/valves/{id}/remove

irrigation/{deviceId}/command/valves/{id}/open

irrigation/{deviceId}/command/valves/{id}/close

irrigation/{deviceId}/command/emergency-stop
```

State

```
irrigation/{deviceId}/state/valves/{id}
```

Events

```
irrigation/{deviceId}/events/...
```

Status

```
irrigation/{deviceId}/status
```

---

## Wi-Fi Provisioning

When no Wi-Fi credentials exist:

The controller shall enter provisioning mode.

The controller shall expose a secure SoftAP.

The user shall provide:

* Wi-Fi SSID
* Wi-Fi Password

Credentials shall be stored in NVS.

Upon successful connection:

* provisioning mode ends
* normal boot begins

---

## Safety

The controller shall:

* boot with all valves closed
* close all valves during emergency stop
* close valves after watchdog reset
* never leave outputs energised during startup

Future support:

* configurable maximum valve open duration

---

## Networking

The controller shall:

* reconnect automatically to Wi-Fi
* reconnect automatically to MQTT
* publish online/offline status
* retain configuration after reboot

---

## Storage

Persist:

* Wi-Fi credentials
* MQTT credentials
* Valve configuration

Do not persist:

* Zone information
* Valve names
* Schedules

---

## Security

Use:

* unique device id
* unique MQTT client id
* proof-of-possession during provisioning (future)
* encrypted credential storage (future)

---

# Software Structure

```text
src/

Application/

Connectivity/
    NetworkManager
    ProvisioningManager
    MqttClient

Controllers/
    ValveController

Drivers/
    IValveDriver
    Drv8871ValveDriver

Domain/
    Valve
    ValveState
    ControlResult

Storage/
    ConfigurationStore

Api/
    MqttCommandHandler
```

---

# Milestones

## Milestone 1

GPIO Driver

### Tasks

* Implement valve driver
* Open valve
* Close valve
* Close all

### Exit Criteria

* GPIO correctly controls valve hardware
* Unit tests pass

---

## Milestone 2

Valve Controller

### Tasks

* Add valve
* Remove valve
* Track state
* Emergency stop

### Exit Criteria

* Multiple valves supported
* Invalid IDs handled correctly

---

## Milestone 3

MQTT

### Tasks

* Subscribe to command topics
* Execute commands
* Publish state
* Publish errors

### Exit Criteria

* Valve commands operate entirely over MQTT

---

## Milestone 4

Provisioning

### Tasks

* Detect missing credentials
* Start SoftAP
* Web configuration page
* Save credentials
* Connect to Wi-Fi

### Exit Criteria

* Fresh ESP32 can join a home network without recompilation

---

## Milestone 5

Persistence

### Tasks

* Store valve configuration
* Restore on reboot

### Exit Criteria

* Power cycle restores previous configuration

---

## Milestone 6

Reliability

### Tasks

* Wi-Fi reconnect
* MQTT reconnect
* Watchdog
* Safe startup
* Safe shutdown

### Exit Criteria

* Controller survives network outages
* Controller survives power loss
* All valves remain safe

---

# Definition of Done

The project is complete when:

* A brand-new ESP32 can be powered on.
* The user provisions Wi-Fi without flashing firmware.
* The controller automatically connects to MQTT.
* The Raspberry Pi discovers the controller.
* MQTT commands can:

  * add valves
  * remove valves
  * open valves
  * close valves
* Valve state is published.
* The controller survives reboot without losing configuration.
* All outputs default to a safe state.
* The firmware contains no scheduling, zone management, or business logic.

---

# Future Enhancements (Out of Scope)

* OTA firmware updates
* BLE provisioning
* TLS client certificates
* Flow sensors
* Pressure sensors
* Current sensing
* Leak detection
* Multiple output driver types
* Secure boot
* Flash encryption
* Remote diagnostics
* Device claiming
* Fleet management
