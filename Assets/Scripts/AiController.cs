using Assets.Scripts.NeuralNetwork;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Network = Assets.Scripts.NeuralNetwork.Network;

namespace Assets.Scripts
{
    internal class AiController : GameController
    {
        [SerializeField] private double maxBias;

        private Network uiNetwork;
        private Vector<double>[] outputs;
        private Vector<double> input;

        public Network UiNetwork
        {
            get { return uiNetwork; }
        }

        public Vector<double>[] Outputs
        {
            get { return outputs; }
        }

        public Vector<double> Input { get { return input; } }

        public double MaxBias { get { return maxBias; } }

        public override void AnimationFinished()
        {
            input = Vector<double>.Build.Dense(16);

            for (int i = 0; i < 16; i++)
            {
                input[i] = Grid[i];
            }

            outputs = uiNetwork.ExpandedOutput(input);

            var moves = outputs[outputs.Length - 1].Clone();

            var movesOrdered = new List<int>();

            for (int i = 0; i < 4; i++)
            {
                movesOrdered.Add(moves.MaximumIndex());
                moves[moves.MaximumIndex()] = -1;
            }

            foreach (var move in movesOrdered)
            {
                if (Slide((Direction) move))
                {
                    return;
                }
            }

            Debug.LogWarning("Lost Game!");
        }

        private void Start()
        {
            uiNetwork = new Network(16, 2, 16, 4);
            uiNetwork.Shuffle(maxBias);
        }
    }
}
