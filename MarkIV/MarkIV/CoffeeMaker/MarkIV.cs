using MarkIV.Devices;
using MarkIV.Devices.Concrete;
using MarkIV.Devices.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkIV.CoffeeMaker
{
    public class MarkIV : IManageStatus<MarkIVStatus>
    {
        // Items Managed by the GUI
        public IBoiler Boiler;
        public IButton BrewButton;
        public IPlateSensor PlateSensor;
        
        // Internals
        private ILight BrewingLight;
        private IPlateHeater PlateHeater;
        private IReliefValve ReliefValve;

        // MarkIVStatus
        private MarkIVStatus _status;

        private IObservable<IEvent> _eventStream;
        private IDisposable _evtDisposable;

        private IDisposable _brewingComplete;

        public MarkIV(
                // Items Managed by the GUI
                IButton BrewButton,
                ILight BrewingLight,
                IPlateSensor PlateSensor,

                // Internals
                IBoiler Boiler,
                IPlateHeater PlateHeater,
                IReliefValve ReliefValve            
            )
        {
            this.BrewButton = BrewButton;
            this.BrewingLight = BrewingLight;
            this.PlateSensor = PlateSensor;
            this.Boiler = Boiler;
            this.PlateHeater = PlateHeater;
            this.ReliefValve = ReliefValve;

            SetupEventStream();
        }

        private void SetupEventStream() {
            _eventStream = (IObservable<IEvent>)BrewButton
                .Merge<IEvent>(Boiler)
                .Merge<IEvent>(BrewingLight)
                .Merge<IEvent>(PlateSensor)
                .Merge<IEvent>(PlateHeater)
                .Merge<IEvent>(ReliefValve);

            _evtDisposable = _eventStream
                .Subscribe(evt => EventProcessor(evt));
        }

        private void EventProcessor(IEvent evt) {

            Console.WriteLine(DateTime.Now.ToString() + " - Received Event: " + evt.GetType().ToString());

            if (evt is ButtonPressed)
            {
                StartBrewing();
                return;
            }

            if (evt is WaterTemperatureReached100Degrees)
            {
                BrewingLight.TurnOn();
                return;
            }

            if (evt is BoilerEmpty)
            {
                StopBrewing();
                return;
            }

            if (evt is PlateSensorHasNoPot)
            {
                SuspendBrewing();
                PlateHeater.TurnOff();
                return;
            }

            if (evt is PlateSensorHasEmptyPot)
            {
                ResumeBrewing();
                PlateHeater.TurnOff();
                return;
            }

            if (evt is PlateSensorHasNonEmptyPot)
            {
                ResumeBrewing();
                PlateHeater.TurnOn();
                return;
            }
        }

        private void StopBrewing()
        {
            _status = MarkIVStatus.STOPPED;
            Boiler.TurnOff();
            ReliefValve.Close();
            BrewingLight.TurnOff();
        }

        private void SuspendBrewing()
        {
            if (_status == MarkIVStatus.IS_BREWING)
            {
                _status = MarkIVStatus.IS_SUSPENDED;
                Boiler.TurnOff();
                ReliefValve.Open();
                BrewingLight.Flash(TimeSpan.FromSeconds(1));
            }
        }

        private void ResumeBrewing()
        {
            if (_status == MarkIVStatus.IS_SUSPENDED)
            {
                StartBrewing();
            }
        }

        private void StartBrewing()
        {
            if (CanStartBrewing())
            {
                _status = MarkIVStatus.IS_BREWING;
                BrewingLight.Flash(TimeSpan.FromSeconds(1));
                Boiler.TurnOn();
                ReliefValve.Close();
            }
        }

        private bool CanStartBrewing()
        {
            return ((IManageStatus<MarkIVStatus>)this).GetStatus() != MarkIVStatus.IS_BREWING 
                && ((IManageStatus<WaterLevelStatus>)Boiler).GetStatus() == WaterLevelStatus.NOT_EMPTY
                && ((IManageStatus<PlateSensorStatus>)PlateSensor).GetStatus() != PlateSensorStatus.HAS_NO_POT;
        }

        public string GetStatusAsString()
        {
            return String.Format("Machine Status: {0}\n", ((IManageStatus<MarkIVStatus>)this).GetStatus().ToString())
                 + String.Format("Boiler Status : {0}\n", ((IManageStatus<BoilerStatus>)Boiler).GetStatus().ToString())
                 + String.Format("Boiler Temp   : {0}\n", ((IManageStatus<WaterTemperatureStatus>)Boiler).GetStatus().ToString())
                 + String.Format("Boiler Level  : {0}\n", ((IManageStatus<WaterLevelStatus>)Boiler).GetStatus().ToString())
                 + String.Format("Pot Presence  : {0}\n", ((IManageStatus<PlateSensorStatus>)PlateSensor).GetStatus().ToString())
                 + String.Format("Pot Heater    : {0}\n", ((IManageStatus<PlateHeaterStatus>)PlateHeater).GetStatus().ToString())
                 + String.Format("Light Status  : {0}\n", ((IManageStatus<LightStatus>)BrewingLight).GetStatus().ToString())
                 + String.Format("Flash Status  : {0}\n", ((IManageStatus<FlashStatus>)BrewingLight).GetStatus().ToString())
                 + String.Format("Relief Valve  : {0}\n", ((IManageStatus<ReliefValveStatus>)ReliefValve).GetStatus().ToString());

        }

        public MarkIVStatus GetStatus()
        {
            return _status;
        }
    }

    public enum MarkIVStatus
    {
        STOPPED = 0,
        IS_BREWING,
        IS_SUSPENDED
    }
}
