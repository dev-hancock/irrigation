#pragma once

#include <vector>

namespace Irrigation
{
    class IService
    {
    public:
        virtual ~IService() = default;

        virtual void update() = 0;
    };

    class Services
    {
    public:

        void add(IService &service)
        {
            _services.push_back(&service);
        }

        void update()
        {
            for (IService *service : _services)
            {
                service->update();
            }
        }

    private:
        std::vector<IService *> _services;
    };
}