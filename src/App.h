#pragma once

#include <Arduino.h>

#include <cstdlib>

#include "Common/State.h"
#include "Common/Api.h"

#include "WiFi/Module.h"

#include "Mqtt/Events.h"
#include "Mqtt/Module.h"

#include "Valves/Module.h"

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
			  _events(
				  _mqtt.publisher()),
			  _valves(
				  _state.valves,
				  _router,
				  _events)
		{
		}

		Result begin()
		{
			Result result = Result::Success();

			result = _valves.begin();
			if (result.isFailure())
			{
				return result;
			}

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

			return result;
		}

		void update()
		{
			_wifi.update();
			_mqtt.update();

			_valves.update();
		}

	private:
		State _state;
		Router _router;

		Mqtt::Events _events;
		WiFi::Module _wifi;
		Mqtt::Module _mqtt;

		Valve::Module _valves;
	};

}