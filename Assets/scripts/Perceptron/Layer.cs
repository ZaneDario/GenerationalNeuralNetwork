using System;
using UnityEngine;

namespace Assets.Perceptron
{
    public class Layer : MonoBehaviour
    {
        public Neuron[] neurons { get; set; }
        public double[] outputs { get; set; }

        public Layer(int neuronsNum, int prevNeuronsNum)
        {
            neurons = new Neuron[neuronsNum];
            outputs = new double[neuronsNum];

            for (int i = 0; i < neurons.Length; i++)
            {
                Neuron n = new Neuron(prevNeuronsNum);
                neurons[i] = n;
            }
        }

        public Layer(double[,] neuronWeights, bool isFirstKid)
        {
            neurons = new Neuron[neuronWeights.GetLength(0)];
            outputs = new double[neuronWeights.GetLength(0)];
            for (int i = 0; i < neuronWeights.GetLength(0); i++)
            {
                Neuron n = new Neuron(getMatrixColumn(neuronWeights, i), isFirstKid);
                neurons[i] = n;
            }
        }

        public double[] GenerateOutput(double[] inputs)
        {
            for (int i = 0; i < neurons.Length; i++)
            {
                outputs[i] = neurons[i].GenerateOutputs(inputs);
                //Debug.Log("Layer: " + layer + " --> Neuron: " + i + " --> Output:" + outputs[i]);
            }
            return outputs;
        }

        private double[] getMatrixColumn(double[,] matrix, int column)
        {
            double[] newMatrix = new double[matrix.GetLength(1)];
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                newMatrix[j] = matrix[column, j];
            }

            return newMatrix;
        }
    }
}
