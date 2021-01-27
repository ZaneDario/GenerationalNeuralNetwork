using Assets.Perceptron;
using Assets.scripts.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

    double speed;
    double dir;
    int rayNumber = 7;
    public Perceptron perceptron;
    public double[][,] defaultGenes;
    public int checkPoints = 0;
    public bool isRunning = true;
    public float checkPointTime = 0;
    private List<GameObject> checkPointsList = new List<GameObject>();
    public bool isTheFirstKid { get; set; }

    private void Start()
    {
        if (defaultGenes == null)
            perceptron = new Perceptron(5, new int[] { rayNumber, 4, 2, 2, 2 });
        else 
            perceptron = new Perceptron(defaultGenes, isTheFirstKid);

        checkPointTime = Time.time;
    }

    void FixedUpdate()
    {
        if(isRunning)
        {
            double[] distances = GenerateRaycast();
            Move(distances);

            if (Time.time - checkPointTime >= 15)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                isRunning = false;
            } 
        }
    }

    private double[] GenerateRaycast()
    {
        double[] distanceToPoint = new double[rayNumber];
        float rayGap = 180f / (rayNumber - 1f);
        float laserLength = 200f;
        int layerMask = 1 << 2;

        for (int i = 0; i < rayNumber; i++)
        {
            Vector3 eulerAngles = new Vector3(rayGap * i -90 + gameObject.transform.eulerAngles.z, 90, 0);
            Vector2 rotation = Quaternion.Euler(-gameObject.transform.eulerAngles + eulerAngles) * Vector3.forward;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rotation, laserLength, ~layerMask);
            distanceToPoint[i] = hit.distance;
            //Debug.DrawRay(transform.position, rotation * laserLength, Color.red);
        }

        return distanceToPoint;
    }

    private void Move(double[] distances)
    {
        double[] outputs = perceptron.GenerateOutputs(distances);
        float speed = (float)Maths.LerpValue(outputs[0], -100, 100);
        float dir = (float)outputs[1];
        //Debug.Log("Speed: " + speed + ", Direction: " + dir);
        gameObject.transform.Rotate(new Vector3(0, 0, dir) * Time.deltaTime, Space.Self);
        gameObject.GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("wall"))
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            isRunning = false;
        }

        if (col.gameObject.CompareTag("checkpoint"))
        {
            sumCheckPoint(col);      
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("wall"))
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            isRunning = false;
        }
    }

    private void sumCheckPoint(Collider2D col)
    {
        string[] checkPoint = col.gameObject.name.Split(' ');
        string checkPointNum = checkPoint[1];
        int checkPointNumber;
        int.TryParse(checkPointNum, out checkPointNumber);

        for (int i = 0; i <= Mathf.FloorToInt(checkPointsList.Count / 13); i++)
        {
            if (checkPointsList.Count - (13*i) == checkPointNumber - 1)
            {
                checkPointsList.Add(col.gameObject);
                checkPoints++;
                checkPointTime = Time.time;
            }
        }      
    }
}
