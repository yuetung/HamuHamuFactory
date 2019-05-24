using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public class Employee
    {
        string field;
        int sprite;
        string name;
        List<int> stationStats;
        int costPerDay;
        public Employee(string field, int sprite, string name, List<int> stationStats, int costPerDay)
        {
            this.field = field;
            this.name = name;
            this.stationStats = stationStats;
            this.costPerDay = costPerDay;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Employee> UpdateHiring(string field, int num, int level, List<int> stations)
    {
        List<Employee> employees = new List<Employee>();
        // TODO: do random generating
        return employees;
    }
}
