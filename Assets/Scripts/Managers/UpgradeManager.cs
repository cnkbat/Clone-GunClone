using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance {get; private set;}

    [Header("Income")]
     public List<float> incomeValues;
    [SerializeField] float incomeStartValue, incomeIncreasingValue;
    [Header("InitYear")]
    public List<int> initYearValues;
    [SerializeField] int initYearStartValue, initYearIncreasingValue;
    
    [Header("Fire Rate")]
    public List<float> fireRateValues;
    [SerializeField] float  fireRateChangeLevelIndex, fireRateStartValue, fireRateIncreasingValue, secondFireRateIncreasingValue;

    [Header("Fire Range")]
    public List<float> fireRangeValues;
    [SerializeField] float fireRangeStartValue, fireRangeIncreasingValue;

    [Header("Cost")]
    public List<int> costs;
    [SerializeField] int costStartingValue, costIncreasingValue;

    private void Awake() 
    {
        if(instance ==null)
        {
            instance = this;
        }   
        initYearValues.Clear();
        costs.Clear();
        fireRangeValues.Clear();
        fireRateValues.Clear();
        incomeValues.Clear();

        SetIncomeValues();
        SetFireRangeValues();
        SetFireRateValues();
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
    public void SetFireRangeValues()
    {
        float firstValue = 12;
        fireRangeValues.Add(firstValue);

        for (int i = 1; i < 1000; i++)
        {
            float nextValue = fireRangeValues[i - 1] + 0.5f;
            fireRangeValues.Add(nextValue);
        }
    } 
    public void SetIncomeValues()
    {
        float firstValue = 1.25f;
        incomeValues.Add(firstValue);

        for (int i = 1; i < 1000; i++)
        {
            float nextValue = incomeValues[i - 1] + 0.05f;
            nextValue = Mathf.Round(nextValue * 100f) / 100f;
            incomeValues.Add(nextValue);
        }
    }
    public void SetFireRateValues()
    {
        float firstValue = 0.75f;
        fireRateValues.Add(firstValue);

        for (int i = 1; i < 5; i++)
        {
            float nextValue = fireRateValues[i - 1] - 0.03f;
            nextValue = Mathf.Round(nextValue * 100f) / 100f;
            fireRateValues.Add(nextValue);
        }
        for (int i = 5; i < 1000; i++)
        {
            float nextValue = fireRateValues[i -1] - 0.01f;
            nextValue = Mathf.Round(nextValue * 100f) / 100f;
            nextValue = Mathf.Clamp(nextValue,0.04f,5f);
            fireRateValues.Add(nextValue);
        }
    } 
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
