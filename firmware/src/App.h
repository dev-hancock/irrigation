#pragma once

#include <Arduino.h>

#include <cstdlib>

#include "Common/State.h"
#include "Common/Api.h"

#include "WiFi/Module.h"

#include "Mqtt/Module.h"

#include "Valves/Module.h"
#include "Device/Module.h"

namespace Irrigation
{
	class App
	{
	public:
		App()
			: _state(),
			  _router(),
			  _wifi(),
			  _mqtt(
				  _router,
				  _wifi.client()),
			  _valves(
				  _state.valves,
				  _router,
				  _mqtt.events()),
			  _device(_mqtt.events())
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

			// _valves.update();
		}

	private:
		State _state;
		Router _router;

		WiFi::Module _wifi;
		Mqtt::Module _mqtt;

		Device::Module _device;
		Valve::Module _valves;
	};

}