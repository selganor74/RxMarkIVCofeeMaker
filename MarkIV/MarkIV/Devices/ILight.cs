using MarkIV.Devices.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkIV.Devices
{
    public interface ILight : IManageStatus<LightStatus>, IManageStatus<FlashStatus>, IObservable<ILightEvent>
    {
        void TurnOn();
        void TurnOff();
        void Flash(TimeSpan? interval);
        void StopFlashing();
    }

    public enum LightStatus
    {
        OFF = 0,
        ON
    }

    public enum FlashStatus
    {
        NOT_FLASHING = 0,
        FLASHING
    }
}
