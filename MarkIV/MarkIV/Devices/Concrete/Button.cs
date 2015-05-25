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
    public class Button : IButton
    {
        private ButtonStatus _status;
        private Subject<IButtonEvent> _eventSource;
        private IDisposable _timer;

        public Button()
        {
            _eventSource = new Subject<IButtonEvent>();

            _status = ButtonStatus.DEPRESSED;
        }

        public void Press(TimeSpan? pressTime)
        {
            TimeSpan t;
            
            _status = ButtonStatus.PRESSED;
            
            _eventSource.OnNext(new ButtonPressed());
            
            t = pressTime == null ? TimeSpan.FromMilliseconds(100) : (TimeSpan)pressTime;
            _timer = Observable.Interval(t).Subscribe(x => Depress());
        }

        private void Depress() 
        {
            _status = ButtonStatus.DEPRESSED;

            _eventSource.OnNext(new ButtonDepressed());
            
            _timer.Dispose();
        }

        public ButtonStatus GetStatus()
        {
            return _status;
        }

        public IDisposable Subscribe(IObserver<Events.IButtonEvent> observer)
        {
            return _eventSource.Subscribe(observer);
        }
    }
}
