using MarkIV.Devices.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkIV.Devices
{
    public interface IBoiler : IManageStatus<BoilerStatus>, IManageStatus<WaterTemperatureStatus>, IManageStatus<WaterLevelStatus>, IObservable<IBoilerEvent>
    {
        void TurnOn();
        void TurnOff();
        void Refill();
        void Empty();
    }

    public enum BoilerStatus
    {
        OFF = 0,
        ON
    }

    public enum WaterTemperatureStatus
    {
        NOT_BOILING = 0,
        BOILING
    }

    public enum WaterLevelStatus
    {
        EMPTY,
        NOT_EMPTY
    }
}
