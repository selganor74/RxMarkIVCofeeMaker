using MarkIV.Devices.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkIV.Devices
{
    public interface IPlateHeater : IManageStatus<PlateHeaterStatus>, IObservable<IPlateHeaterEvent>
    {
        void TurnOn();
        void TurnOff();
    }

    public enum PlateHeaterStatus
    {
        OFF = 0,
        ON
    }
}
