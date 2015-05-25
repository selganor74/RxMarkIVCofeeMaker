using MarkIV.Devices.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkIV.Devices
{
    public interface IReliefValve : IManageStatus<ReliefValveStatus>, IObservable<IReliefValveEvent>
    {
        void Open();
        void Close();
    }

    public enum ReliefValveStatus 
    {
        CLOSED = 0,
        OPENED
    }
}
