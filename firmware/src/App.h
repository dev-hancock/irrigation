#pragma once

#include <Arduino.h>

#include <cstdlib>

#include "State.h"
#include "Common/Api.h"

#include "WiFi/Service.h"
#include "Mqtt/Service.h"

#include "Device/Module.h"

#include "Valves/Module.h"
#include "Health/Module.h"

namespace Irrigation
{
	class App
	{
	public:
		App(const Device::Definition &device)
			: _state(),
			  _router(),
			  _wifi(),
			  _mqtt(
				  _router,
				  _wifi.client()),
			  _device(
				  device,
				  _mqtt.events()),
			  _valves(
				  _state.valves,
				  device.valves,
				  _router,
				  _mqtt.events()),
			  _health(
				  device,
				  _router,
				  _mqtt.events())
		{
		}

		Result begin()
		{
			Result result = Result::Success();

			Serial.println("System begin...");

			result = _wifi.begin();
			if (result.isFailure())
			{
				return result;
			}

			result = _mqtt.begin();
			if (result.isFailure())
			{
				return result;
			}

			result = _device.begin();
			if (result.isFailure())
			{
				return result;
			}

			result = _health.begin();
			if (result.isFailure())
			{
				return result;
			}

			result = _valves.begin();
			if (result.isFailure())
			{
				return result;
			}

			Serial.println("System initialized");

			return result;
		}

		void update()
		{
			_wifi.update();

			_mqtt.update();
		}

	private:
		State _state;
		Router _router;

		WiFi::Service _wifi;
		Mqtt::Service _mqtt;

		Device::Module _device;
		Health::Module _health;
		Valve::Module _valves;
	};

}