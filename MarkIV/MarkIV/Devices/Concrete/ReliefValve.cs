using MarkIV.Devices.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace MarkIV.Devices.Concrete
{
    public class ReliefValve : IReliefValve
    {
        private ReliefValveStatus _status;
        private Subject<IReliefValveEvent> _eventSource;

        public ReliefValve()
        {
            _status = ReliefValveStatus.CLOSED;
            _eventSource = new Subject<IReliefValveEvent>();
        }

        public void Open()
        {
            if (_status != ReliefValveStatus.OPENED)
            {
                _status = ReliefValveStatus.OPENED;
                _eventSource.OnNext(new ReliefValveOpened());
            }
        }

        public void Close()
        {
            if (_status != ReliefValveStatus.CLOSED)
            {
                _status = ReliefValveStatus.CLOSED;
                _eventSource.OnNext(new ReliefValveClosed());
            }
        }

        public ReliefValveStatus GetStatus()
        {
            return _status;
        }

        public IDisposable Subscribe(IObserver<Events.IReliefValveEvent> observer)
        {
            return _eventSource.Subscribe(observer);
        }
    }
}
