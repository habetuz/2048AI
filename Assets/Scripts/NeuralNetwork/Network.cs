using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;
using UnityEngine;

namespace Assets.Scripts.NeuralNetwork
{
    public class Network
    {
        private Vector<double> inputNodes;

        private Layer[] hiddenLayers;

        private Layer outputLayer;

        public Vector<double> InputNodes { get { return inputNodes; } }
        public Layer[] HiddenLayers { get { return hiddenLayers; } }
        public Layer OuputLayer { get { return outputLayer; } }

        public Network(ushort inputNodes, sbyte hiddenLayers, ushort hiddenLayersNodes, ushort outputNodes)
        {
            this.inputNodes = Vector<double>.Build.Dense(inputNodes);
            this.hiddenLayers = new Layer[hiddenLayers];
            for (int i = 0; i < hiddenLayers; i++)
            {
                if (i == 0)
                {
                    this.hiddenLayers[i] = new Layer(inputNodes, hiddenLayersNodes);
                }
                else
                {
                    this.hiddenLayers[i] = new Layer(hiddenLayersNodes, hiddenLayersNodes);
                }
            }

            outputLayer = hiddenLayers > 0? new Layer(hiddenLayersNodes, outputNodes) : new Layer(inputNodes, outputNodes);
        }

        public void Shuffle(double maxBias)
        {
            inputNodes = Vector<double>.Build.Dense(SystemRandomSource.Doubles(inputNodes.Count, RandomSeed.Guid()));
            inputNodes = inputNodes * 2 - 1;

            foreach (var layer in hiddenLayers)
            {
                layer.Shuffle(maxBias);
            }

            outputLayer.Shuffle(maxBias);
        }

        public Vector<double> Output(Vector<double> input)
        {
            for (int i = 0; i < hiddenLayers.Length; i++)
            {
                input = hiddenLayers[i].Output(input);
            }

            return outputLayer.Output(input);
        }

        public Vector<double>[] ExpandedOutput(Vector<double> input)
        {
            var outputs = new Vector<double>[hiddenLayers.Length + 1];

            for (int i = 0; i < hiddenLayers.Length; i++)
            {
                input = hiddenLayers[i].Output(input);
                outputs[i] = input;
            }

            outputs[outputs.Length - 1] = outputLayer.Output(input);

            return outputs;
        }
    }
}
