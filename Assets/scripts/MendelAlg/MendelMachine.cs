using UnityEngine;

public class MendelMachine : MonoBehaviour {

    public float TimeScale = 1;
    int generation = 0;
    public int individuals;
    public GameObject car;
    GameObject[] cars = null;
    public Transform map;
    public Transform initPoint;
    public static int _maxCheckPoints = 0;

    private void Start()
    {
        Time.timeScale = TimeScale;
        generateCars(individuals);
    }

    private void Update()
    {
        if(!areRunning())
        {
            generateCars(individuals);
        }
    }

    private void generateCars(int amount)
    {
        generation++;

        GameObject[] oldCars = null;

        if (cars != null)
            oldCars = cars;
        
        cars = new GameObject[individuals];
        
        double[][,] genes = generation == 1 ? null : meanGeneticalMatrix(oldCars);

        for (int i = 0; i < cars.Length; i++)
        {
            GameObject c = Instantiate(car);
            c.transform.position = initPoint.transform.position;
            c.transform.rotation = new Quaternion(0, 0, 0, 0);
            c.transform.SetParent(map);

            c.GetComponent<CarController>().isTheFirstKid = i == 0 ? true : false;
            c.GetComponent<CarController>().defaultGenes = genes;

            cars[i] = c;
        }

        if(oldCars != null)
        {
            foreach (GameObject c in oldCars)
            {
                Destroy(c);
            }
        }
    }

    private bool areRunning()
    {
        foreach (GameObject c in cars)
        {
            if (c.GetComponent<CarController>().isRunning)
                return true;
        }

        return false;
    }

    private double[][,] meanGeneticalMatrix(GameObject[] samples)
    {
        _maxCheckPoints = maxCheckPoints(samples);

        if (_maxCheckPoints != 0)
        {
            int optimalIndividuals = 0;
            double[][,] geneticMatrix = generateMatrix(samples[0].GetComponent<CarController>());
            foreach (GameObject c in samples)
            {
                CarController carCtrl = c.GetComponent<CarController>();
                if (carCtrl.checkPoints == _maxCheckPoints)
                {
                    optimalIndividuals++;
                    for (int i = 0; i < geneticMatrix.Length; i++)
                    {
                        for (int j = 0; j < geneticMatrix[i].GetLength(0); j++)
                        {
                            for (int k = 0; k < geneticMatrix[i].GetLength(1); k++)
                            {
                                geneticMatrix[i][j, k] += carCtrl.perceptron.layers[i].neurons[j].weights[k];
                            }
                        }
                    }
                }
            }
            Debug.Log("Generation: " + generation + ". Optimal individuals: " + optimalIndividuals);
            for (int i = 0; i < geneticMatrix.Length; i++)
            {
                for (int j = 0; j < geneticMatrix[i].GetLength(0); j++)
                {
                    for (int k = 0; k < geneticMatrix[i].GetLength(1); k++)
                    {
                        geneticMatrix[i][j, k] /= optimalIndividuals;
                    }
                }
            }
            return geneticMatrix;
        }
        else
            return null;     
    }

    private int maxCheckPoints(GameObject[] samples)
    {
        int maxCheckPoint = 0;
        foreach (GameObject c in samples)
        {
            int cPoints = c.GetComponent<CarController>().checkPoints;
            if (cPoints > maxCheckPoint)
                maxCheckPoint = cPoints;
        }

        return maxCheckPoint;
    }

    private double[][,] generateMatrix(CarController sample)
    {
        int layers = sample.perceptron.layers.Length;
        double[][,] matrix = new double[layers][,];

        for (int i = 0; i < layers; i++)
        {
            int neurons = sample.perceptron.layers[i].neurons.Length;
            int weights = sample.perceptron.layers[i].neurons[0].weights.Length;

            matrix[i] = new double[neurons, weights];
        }

        return matrix;
    }
}
