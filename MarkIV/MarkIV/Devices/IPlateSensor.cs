using MarkIV.Devices.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkIV.Devices
{
    public interface IPlateSensor : IManageStatus<PlateSensorStatus>, IObservable<IPlateSensorEvent>
    {
        void PutEmptyPot();
        void PutNonEmptyPot();
        void RemovePot();
    }

    public enum PlateSensorStatus
    {
        HAS_NO_POT = 0,
        HAS_EMPTY_POT,
        HAS_NON_EMPTY_POT
    }
}
