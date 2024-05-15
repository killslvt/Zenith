using System;

namespace Zenith.Injector
{
    public enum InjectionStatus
    {
        FAILED,
        FAILED_ADMINISTRATOR_ACCESS,
        ALREADY_INJECTING,
        ALREADY_INJECTED,
        SUCCESS
    }
}
