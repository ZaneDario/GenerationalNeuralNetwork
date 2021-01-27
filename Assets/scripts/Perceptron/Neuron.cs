using Assets.scripts.Resources;
using System;
using UnityEngine;

namespace Assets.Perceptron
{
    public class Neuron : MonoBehaviour
    {
        double[] inputs;
        public double[] weights {get; set; }
        double output;

        public Neuron(int inputs)
        {
            weights = new double[inputs];
            for (int i = 0; i < weights.Length; i++)
            {
                float random = RandomGenerator.RandomFloat();
                weights[i] = random;
            }           
        }

        public Neuron(double[] neuronWeights, bool isFirstKid)
        {
            weights = new double[neuronWeights.Length];
            for (int i = 0; i < weights.Length; i++)
            {
                float random = isFirstKid ? 0 : (RandomGenerator.RandomFloat() / (2f * (MendelMachine._maxCheckPoints+1)));
                weights[i] = neuronWeights[i] + random;
            }
        }

        public double GenerateOutputs(double[] inputs)
        {
            output = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                output += inputs[i] * weights[i];
            }
            return output;
        }
    }
}
