using MarkIV.Devices.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkIV.Devices
{
    public interface IButton : IManageStatus<ButtonStatus>, IObservable<IButtonEvent>
    {
        void Press(TimeSpan? pressTime);
    }

    public enum ButtonStatus
    {
        DEPRESSED = 0,
        PRESSED
    }
}
