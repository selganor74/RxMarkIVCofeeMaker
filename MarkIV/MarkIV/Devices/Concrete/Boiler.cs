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
    public class Boiler : IBoiler
    {
        private BoilerStatus _status;
        private WaterTemperatureStatus _waterTempStatus;
        private WaterLevelStatus _waterLevelStatus;
        private int _waterLevelInCups;

        // The mock implementation needs a plateSensor because as the waterboils, the pot will start filling
        private IPlateSensor plateSensor;

        private Subject<IBoilerEvent> _eventSource;
        private IDisposable _boilingTimer;
        private IDisposable _waterLevelTimer;

        public Boiler(IPlateSensor plateSensor)
        {
            this.plateSensor = plateSensor;

            _status = BoilerStatus.OFF;
            _waterTempStatus = WaterTemperatureStatus.NOT_BOILING;
            _waterLevelStatus = WaterLevelStatus.NOT_EMPTY;
            _waterLevelInCups = 12;

            _eventSource = new Subject<IBoilerEvent>();
        }

        public void TurnOn()
        {
            if (_status != BoilerStatus.ON)
            {
                _status = BoilerStatus.ON;

                _eventSource.OnNext(new BoilerStarted());

                _boilingTimer = Observable.Interval(TimeSpan.FromSeconds(10)).Subscribe(x => WaterStartBoiling());
            }
        }

        public void TurnOff()
        {
            if (_status != BoilerStatus.OFF)
            {
                _status = BoilerStatus.OFF;

                _eventSource.OnNext(new BoilerStopped());

                try
                {
                    _boilingTimer.Dispose();
                }
                catch { }
                _boilingTimer = Observable.Timer(TimeSpan.FromSeconds(5)).Subscribe(x => WaterNoMoreBoiling());
            }
        }

        BoilerStatus IManageStatus<BoilerStatus>.GetStatus()
        {
            return _status;
        }

        WaterTemperatureStatus IManageStatus<WaterTemperatureStatus>.GetStatus()
        {
            return _waterTempStatus;
        }

        WaterLevelStatus IManageStatus<WaterLevelStatus>.GetStatus()
        {
            return _waterLevelStatus;
        }

        public IDisposable Subscribe(IObserver<Events.IBoilerEvent> observer)
        {
            return _eventSource.Subscribe(observer);
        }

        private void WaterStartBoiling() 
        {
            if (_waterTempStatus != WaterTemperatureStatus.BOILING)
            {
                _waterTempStatus = WaterTemperatureStatus.BOILING;
                _eventSource.OnNext(new WaterTemperatureReached100Degrees());
                _waterLevelTimer = Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(x => DecreaseWaterLevel());
                
                plateSensor.PutNonEmptyPot();
                
                try
                {
                    _boilingTimer.Dispose();
                } catch {};
            }
        }

        private void WaterNoMoreBoiling()
        {
            if (_waterTempStatus != WaterTemperatureStatus.NOT_BOILING)
            {
                _waterTempStatus = WaterTemperatureStatus.NOT_BOILING;
                _eventSource.OnNext(new WaterTemperatureIsUnder100Degrees());
                try
                {
                    _boilingTimer.Dispose();
                }
                catch { };
            }
        }


        public void Refill()
        {
            try
            {
                _waterLevelTimer.Dispose();
            }
            catch { }

            _waterLevelInCups = 12;

            if (_waterLevelStatus != WaterLevelStatus.NOT_EMPTY)
            {
                _waterLevelStatus = WaterLevelStatus.NOT_EMPTY;
                _eventSource.OnNext(new BoilerRefilled());
            }
        }

        public void Empty()
        {
            try {
                _waterLevelTimer.Dispose();
            }
            catch { }

            if (_waterLevelStatus != WaterLevelStatus.EMPTY)
            {
                _waterLevelStatus = WaterLevelStatus.EMPTY;
                _eventSource.OnNext(new BoilerEmpty());
            }
        }

        private void DecreaseWaterLevel() {
            _waterLevelInCups -= 1;
            if (_waterLevelInCups <= 0)
            {
                _waterLevelInCups = 0;
                Empty();
            }
        }

    }
}
