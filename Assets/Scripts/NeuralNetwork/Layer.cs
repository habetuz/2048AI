using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;
using UnityEngine;
using static UnityEngine.EventSystems.StandaloneInputModule;

namespace Assets.Scripts.NeuralNetwork
{
    public class Layer
    {
        private Vector<double> bias;
        private Matrix<double> weights;

        public Vector<double> Bias { get { return bias; } }
        public Matrix<double> Weights { get { return weights; } }

        public Layer(ushort inputNodes, ushort layerNodes)
        {
            bias = Vector<double>.Build.Dense(layerNodes);
            weights = Matrix<double>.Build.Dense(layerNodes, inputNodes);
        }

        public void Shuffle(double maxBias)
        {
            bias = Vector<double>.Build.Dense(SystemRandomSource.Doubles(bias.Count, RandomSeed.Guid()));
            bias = bias * 2 * maxBias - maxBias;

            weights = Matrix<double>.Build.Dense(weights.RowCount, weights.ColumnCount, SystemRandomSource.Doubles(weights.ColumnCount * weights.RowCount, RandomSeed.Guid()));
            weights = weights * 2 - 1;
        }

        public Vector<double> Output(Vector<double> input)
        {
            var output = weights * input + bias;

            output.Map((num) =>
            {
                return SpecialFunctions.Logistic(num);
            }, output);
            return output;
        }
    }
}
