using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSimulator : MonoBehaviour
{
    public int slots = 12;
    public float chargerStrength = 20;
    public float maximumPower = 100;
    public ComputerSimulator cs;
    public float[] carDemand;
    public InputBoxGenerator ibg;
    private Tooltip tt;
    // Start is called before the first frame update
    void Start()
    {
        
        
        if (cs != null)
        {
            carDemand = new float[cs.carDemand.Length];
            for (int i = 0; i < cs.carDemand.Length; i++)
            {
                carDemand[i] = cs.carDemand[i];
            }
            
        }
        tt = this.GetComponent<Tooltip>();
        spawnVehicles(carDemand);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayTurn()
    {
        if (!ibg.Error)
        {
            //Debug.Log("Button Pressed");
            float[] result = ibg.userInputs;
            for (int i = 0; i < slots; i++)
            {
                carDemand[i] += result[i];
                Debug.Log(result[i]);
            }
            changeVehicles(carDemand);
        }
        
    }
    private void spawnVehicles(float[] cars)
    {
        for (int i = 0; i < cars.Length; i++)
        {
            if (cars[i] != 0)
            {
                tt.Summon(i, cars[i]);
            }
        }
    }
    private void changeVehicles(float[] demand)
    {
        for (int i = 0; i < demand.Length; i++)
        {
            if (demand[i] < 0)
            {
                tt.ChangeValue(i, demand[i]);
            }
            else
            {
                tt.ChangeValue(i, 0);
                //tt.Remove(i);
            }
        }
    }
}
