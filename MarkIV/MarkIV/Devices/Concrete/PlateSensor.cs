using MarkIV.Devices.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace MarkIV.Devices.Concrete
{
    public class PlateSensor : IPlateSensor
    {
        private PlateSensorStatus _status;
        private Subject<IPlateSensorEvent> _eventSource;

        public PlateSensor()
        {
            _status = PlateSensorStatus.HAS_EMPTY_POT;
            _eventSource = new Subject<IPlateSensorEvent>();
        }

        public void PutEmptyPot()
        {
            if (_status != PlateSensorStatus.HAS_EMPTY_POT)
            {
                _status = PlateSensorStatus.HAS_EMPTY_POT;
                _eventSource.OnNext(new PlateSensorHasEmptyPot());
            }
        }

        public void PutNonEmptyPot()
        {
            if (_status != PlateSensorStatus.HAS_NON_EMPTY_POT)
            {
                _status = PlateSensorStatus.HAS_NON_EMPTY_POT;
                _eventSource.OnNext(new PlateSensorHasNonEmptyPot());
            }
        }

        public void RemovePot()
        {
            if (_status != PlateSensorStatus.HAS_NO_POT)
            {
                _status = PlateSensorStatus.HAS_NO_POT;
                _eventSource.OnNext(new PlateSensorHasNoPot());
            }
        }

        public PlateSensorStatus GetStatus()
        {
            return _status;
        }

        public IDisposable Subscribe(IObserver<Events.IPlateSensorEvent> observer)
        {
            return _eventSource.Subscribe(observer);
        }
    }
}
