
namespace Assets.Perceptron
{
    public class Perceptron
    {
        public Layer[] layers { get; set; }

        public Perceptron(int layersNum, int[] neurons)
        {
            if(neurons.Length == layersNum) //They both need the same size.
            {
                layers = new Layer[layersNum];
                for (int i = 0; i < layers.Length; i++)
                {
                    if (i == 0)
                        layers[i] = new Layer(neurons[i], neurons[i]);
                    else
                        layers[i] = new Layer(neurons[i], neurons[i - 1]);
                }
            }
        }

        public Perceptron(double[][,] neuronWeights, bool isFirstKid)
        {
            layers = new Layer[neuronWeights.Length];
            for (int i = 0; i < neuronWeights.Length; i++)
            {
                layers[i] = new Layer(neuronWeights[i], isFirstKid);
            }
        }

        public double[] GenerateOutputs(double[] inputs)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                if (i == 0)
                    layers[i].GenerateOutput(inputs);
                else
                    layers[i].GenerateOutput(layers[i - 1].outputs);
            }

            double speed = layers[layers.Length - 1].outputs[0];
            double dir = layers[layers.Length - 1].outputs[1];
            return new double[]{speed, dir};
        }
    }
}
