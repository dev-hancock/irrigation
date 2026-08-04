#pragma once

#include <ArduinoJson.h>
#include <WiFi.h>
#include <WiFiManager.h>

#include <cstdlib>

#include "Common/State.h"
#include "Common/Messaging.h"
#include "Common/Api.h"

#include "Valves/Module.h"

namespace Irrigation
{
	class App
	{
	public:
		App()
			: _state(),
			  _router(),
			  _events(),
			  _valves(_state.valves, _router, _events)
		{
		}

		void begin()
		{
			_valves.begin();
		}

		void update()
		{
		}

	private:
		State _state;
		Router _router;
		Events _events;

		Valve::Module _valves;
	};

}