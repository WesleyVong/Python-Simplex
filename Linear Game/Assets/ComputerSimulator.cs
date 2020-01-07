using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerSimulator : MonoBehaviour
{
    public int slots;
    public float chargerStrength = 20;
    public float maximumPower = 100;
    public float[] carDemand;
    public float[] resultAfterCalculation;
    public InputBoxGenerator ibg;
    private Tooltip tt;
    private float[][] matrixToCalculate;
    private ServerHandler sh;
    bool gotData = true;
    // Start is called before the first frame update
    void Start()
    {
        sh = this.GetComponent<ServerHandler>();
        tt = this.GetComponent<Tooltip>();
        spawnVehicles(carDemand);
    }

    // Update is called once per frame
    void Update()
    {
        if (sh.uploadFinished && !sh.error && !gotData)
        {
            gotData = true;
            resultAfterCalculation = new float[slots];
            for (int i = 0; i < slots; i++)
            {
                resultAfterCalculation[i] = sh.resultMat.data[0][i];
                try
                {
                    carDemand[i] += sh.resultMat.data[0][i];
                }
                catch
                {

                }
                
            }
            ibg.SetComp(resultAfterCalculation);
            changeVehicles(carDemand);
        }
    }

    public void PlayTurn()
    {
        if (!ibg.Error)
        {
            //Debug.Log("Button Pressed");
            gotData = false;
            matrixToCalculate = GenerateArray(chargerStrength, maximumPower, carDemand);
            sh.Compute(matrixToCalculate);
        }
            
    }
    private void spawnVehicles(float[] cars)
    {
        for (int i = 0; i < cars.Length; i++)
        {
            if (cars[i] != 0)
            {
                tt.Summon(i,cars[i]);
            }
        }
    }
    private void changeVehicles(float[] demand)
    {
        for (int i = 0; i < demand.Length; i++)
        {
            if (demand[i] < 0)
            {
                this.tt.ChangeValue(i, demand[i]);
            } else
            {
                this.tt.ChangeValue(i, 0);
                //tt.Remove(i);
            }
        }
    }
    private float[][] GenerateArray(float strength, float maxPow, float[] demands)
    {
        float[][] finalArray;
        finalArray = new float[slots + 2][];
        // Generates matrix without constraints

        // Generate maximum charger consumption
        finalArray[0] = new float[slots + 1];
        for (int i = 0; i < slots; i++)
        {
            finalArray[0][i] = 1;
        }
        finalArray[0][slots] = maxPow;


        // Generate individual charger constraints
        for (int i = 0; i < slots; i++)
        {
            finalArray[i + 1] = new float[slots + 1];
            for (int j = 0; j < slots; j++)
            {
                if (i == j)
                {
                    finalArray[i + 1][j] = 1;
                }
                else
                {
                    finalArray[i + 1][j] = 0;
                }
            }
            finalArray[i + 1][slots] = strength;
        }

        // Generate Constraints
        finalArray[slots + 1] = new float[slots + 1];
        for (int i = 0; i < slots; i++)
        {
            try
            {
                finalArray[slots + 1][i] = demands[i];
            }
            catch
            {
                finalArray[slots + 1][i] = 0;
            }

        }
        finalArray[slots + 1][slots] = 0;
        return finalArray;
    }
}
