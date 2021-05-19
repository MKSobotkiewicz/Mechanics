using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Math
{
    public class PredictiveController : IController
    {
        private ControllerTypeE type;
        private IController internalController;
        private IModel model;
        private float delay;
        private bool debug;

        private LinkedList<Prediction> predictions=new LinkedList<Prediction>();

        public PredictiveController(IController _internalController, IModel _model, float _delay, float startingRotation, ControllerTypeE _type = ControllerTypeE.Linear)
        {
            delay = _delay;
            internalController = _internalController;
            model = _model;
            type = _type;
            predictions.AddLast(new Prediction(startingRotation, 0));
        }

        public float Update(float desiredValue, float measuredValue, float deltaTime)
        {
            var modeledValue = model.GetLastValue();
            UpdatePredictions(modeledValue,measuredValue,deltaTime);
            var control=internalController.Update(desiredValue, predictions.Last().Value, deltaTime);
            model.Update(control, predictions.Last().Value, deltaTime);
            return control;
        }

        private void UpdatePredictions(float modeledValue,float measuredValue, float deltaTime)
        {
            predictions.AddLast(new Prediction(modeledValue,deltaTime));
            float elapsedTime;
            do
            {
                elapsedTime = 0;
                foreach (var prediction in predictions)
                {
                    elapsedTime += prediction.ElapsedTime;
                }
                if (elapsedTime > delay)
                {
                    var oldestPrediction = predictions.First();
                    predictions.RemoveFirst();
                    var error = modeledValue - measuredValue;
                    foreach(var prediction in predictions)
                    {
                        prediction.Value -= error;
                    }
                }
            } while (elapsedTime > delay);
        }

        public void SetDebug(bool _debug)
        {
            debug = _debug;
        }

        public new ControllerTypeE GetType()
        {
            return type;
        }

        class Prediction
        {
            public float Value;
            public float ElapsedTime { get; private set; }

            public Prediction(float value, float elapsedTime)
            {
                Value = value;
                ElapsedTime = elapsedTime;
            }
        }
    }
}
