using MarkIV.Devices.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace MarkIV.Devices.Concrete
{
    public class PlateHeater : IPlateHeater
    {
        private PlateHeaterStatus _status;
        private Subject<IPlateHeaterEvent> _eventSource;

        public PlateHeater()
        {
            _status = PlateHeaterStatus.OFF;
            _eventSource = new Subject<IPlateHeaterEvent>();
        }

        public void TurnOn()
        {
            if(_status != PlateHeaterStatus.ON){
                _status = PlateHeaterStatus.ON;
                _eventSource.OnNext(new PlateHeaterTurnedOn());
            }
        }

        public void TurnOff()
        {
            if (_status != PlateHeaterStatus.OFF)
            {
                _status = PlateHeaterStatus.OFF;
                _eventSource.OnNext(new PlateHeaterTurnedOff());
            }
        }

        public PlateHeaterStatus GetStatus()
        {
            return _status;
        }

        public IDisposable Subscribe(IObserver<Events.IPlateHeaterEvent> observer)
        {
            return _eventSource.Subscribe(observer);
        }
    }
}
