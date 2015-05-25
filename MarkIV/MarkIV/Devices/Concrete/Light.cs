using MarkIV.Devices.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace MarkIV.Devices.Concrete
{
    public class Light : ILight
    {
        LightStatus _status;
        FlashStatus _fStatus;
        Subject<ILightEvent> _eventSource;
        IDisposable _flashTimer;

        public Light()
        {
            _status = LightStatus.OFF;
            _fStatus = FlashStatus.NOT_FLASHING;

            _eventSource = new Subject<ILightEvent>();
        }
        
        public void TurnOn()
        {
            StopFlashing();
            if (_status != LightStatus.ON)
            {
                Toggle();
            }
        }

        public void TurnOff()
        {
            StopFlashing();
            if (_status != LightStatus.OFF)
            {
                Toggle();
            }
        }

        public void Flash(TimeSpan? interval)
        {
            _fStatus = FlashStatus.FLASHING;
            try
            {
                _flashTimer.Dispose();
            }
            catch { }

            TimeSpan t = interval == null ? TimeSpan.FromMilliseconds(1000) : (TimeSpan)interval;
            _flashTimer = Observable.Interval(t).Subscribe(x => Toggle());
        }

        public void StopFlashing()
        {
            try
            {
                _flashTimer.Dispose();
            } catch 
            {

            }
            _fStatus = FlashStatus.NOT_FLASHING;
        }

        private void Toggle() 
        {
            _status = _status == LightStatus.ON ? LightStatus.OFF : LightStatus.ON;

            ILightEvent evt;
            if (_status == LightStatus.ON )
                evt = new LightLit();
            else 
                evt = new LightUnlit();

            _eventSource.OnNext(evt);
        }

        public LightStatus GetStatus()
        {
            return _status;
        }

        FlashStatus IManageStatus<FlashStatus>.GetStatus()
        {
            return _fStatus;
        }

        public IDisposable Subscribe(IObserver<Events.ILightEvent> observer)
        {
            return _eventSource.Subscribe(observer);
        }

    }
}
