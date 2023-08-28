using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance {get; private set;}

    public List<int> initYearValues;
    public List<float> incomeValues;

    public List<float> fireRateValues;
    public List<float> fireRangeValues;
    
    public List<int> costs;

    private void Awake() 
    {
        if(instance ==null)
        {
            instance = this;
        }   
        initYearValues.Clear();
        costs.Clear();

        SetInitYearValues();
        SetCostValues();
    }
    public void SetInitYearValues()
    {
        int firstValue = 1700;
        initYearValues.Add(firstValue);

        for(int i = 1; i < 1000 ; i++)
        {
            int valueNext = initYearValues[i - 1] + 5;
            initYearValues.Add(valueNext);
        }
    }
    /*public void SetFireRangeValues()
    {
        fireRangeValues[0] = 
    } */
    public void SetCostValues()
    {
        int firstValue = 50;
        costs.Add(firstValue);
        for (int i = 1; i < 1000; i++)
        {
            int nextValue = costs[i - 1] + 50;
            costs.Add(nextValue);
        }
    }
}
