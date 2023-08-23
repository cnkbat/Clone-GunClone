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
    }
    
    public void SetCostValues()
    {
        costs[0] = 50;
        for (int i = 1; i < 1000; i++)
        {
            costs[i] = costs[i - 1] + 50;
        }
    }
}
