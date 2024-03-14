using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class ObjectInfo
    {
        public static double massExponent = -2;
        public static double momentumExponent = 2;
        public static double angleExponent = -3;
        public static double gravityAccelerationExponent = 1;

        public double mass;
        public double momentum;
        public double angle;
        public double gravityAcceleration;
        public double path => Math.Pow(mass, massExponent) * Math.Pow(momentum, momentumExponent) * Math.Pow(angle, angleExponent) * Math.Pow(gravityAcceleration, gravityAccelerationExponent);
        public ObjectInfo(double mass, double momentum, double angle, double accelerationOfGravity)
        {
            this.mass = mass;
            this.momentum = momentum;
            this.angle = angle;
            this.gravityAcceleration = accelerationOfGravity;
        }
        static public ObjectInfo RandomObject(
            (double min, double max) massRange,
            (double min, double max) momentumRange,
            (double min, double max) angleRange,
            (double min, double max) accelRange)
        {
            Random _random = new Random();
            double mass = massRange.min + (massRange.max - massRange.min) * _random.NextDouble();
            double momentum = momentumRange.min + (momentumRange.max - momentumRange.min) * _random.NextDouble();
            double angle = angleRange.min + (angleRange.max - angleRange.min) * _random.NextDouble();
            double accel = accelRange.min + (accelRange.max - accelRange.min) * _random.NextDouble();

            return new ObjectInfo(mass, momentum, angle, accel);
        }
    }
    public class NeuroRandomSearch
    {
        public double[] weights;
        private double educationAcuracy;
        private int selectionSize;
        private double areaRadius;
        private double areaModifier;

        public NeuroRandomSearch()
        {
            Initialize();
        }
        private void Initialize()
        {
            educationAcuracy = 0.1;
            selectionSize = 3;
            areaRadius = 1;
            areaModifier = 0.9999;

            Random rand = new Random();
            weights = new double[4];
            for (int i = 0; i < weights.Length; i++)
                weights[i] = 10 * rand.NextDouble() - 5;
        }

        public double Predict(ObjectInfo objectInfo, double[] tempWeights)
        {
            if (tempWeights == null)
            {
                tempWeights = weights;
            }
            double[] input = [objectInfo.mass, objectInfo.momentum, objectInfo.angle, objectInfo.gravityAcceleration];
            double result = 1;
            for (int i = 0; i < tempWeights.Length; i++)
            {
                //result += input[i] * weights[i];
                result *= Math.Pow(input[i], tempWeights[i]);
            }
            return result;
        }
        public double ObjectiveFunction(ObjectInfo objectInfo, double[] tempWeights)
        {
            if (tempWeights == null)
            {
                tempWeights = weights;
            }
            return Math.Abs(objectInfo.path - Predict(objectInfo, tempWeights));
        }
        private double[] GetRandomPointInArea()
        {
            Random random = new Random();
            double[] resultPoint = new double[4];
            for (int i = 0; i < resultPoint.Length; i++)
            {
                double epsilon = random.NextDouble() * 2 - 1;
                resultPoint[i] = weights[i] + epsilon * areaRadius;
            }
            return resultPoint;
        }

        public void Education()
        {
            while (true)
            {
                double[] extraWeights = GetRandomPointInArea();
                double newSum = 0;
                double oldSum = 0;
                for (int i = 0; i < selectionSize; i++)
                {
                    ObjectInfo objectInfo = ObjectInfo.RandomObject
                    (
                        (1, 5),
                        (5, 25),
                        (0, double.Pi / 2),
                        (9.7, 9.9)
                    );
                    newSum += ObjectiveFunction(objectInfo, extraWeights) / selectionSize;
                    oldSum += ObjectiveFunction(objectInfo, weights) / selectionSize;
                }

                Console.WriteLine(newSum);
                if (newSum < oldSum)
                {
                    weights = extraWeights;
                    areaRadius *= areaModifier;
                    if (areaRadius <= double.Epsilon)
                        break;
                    if (newSum < educationAcuracy)
                        break;
                }
            }
        }

    }
}
