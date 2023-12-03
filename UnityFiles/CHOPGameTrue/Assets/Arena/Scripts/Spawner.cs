using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public float STime = 10f; //Spawns every 10 seconds.
    public float changeInTime = 0.1f; //How much the minus time gets compounded over time.
    private float minusTime = 0f; //The compounding amount removed out of the spawn time.
    public float changeRate = 5f; //The changeintime will be added to the minus time every 5 seconds.
    public GameObject objectToSpawn;

    void Start()
    {
        Instantiate(objectToSpawn, this.transform); // repeats the spawn of the enemy in the location of the spawner, hence "this.transform". However instantiate cant be changed while playing, so the spawn rate isnt instantiated.

        InvokeRepeating("TimeChange", changeRate, changeRate);

        StartCoroutine(wave()); //This coroutine can be changed while playing so it controls the spawning.
    }
    

    // Update is called once per frame
    void Update()
    {
        if (STime <= 0.1f) //Keeps the time above 0.1 seconds so that enemy spawn isnt too high which can be detrimental to the system.
        {
            STime = 0.1f;
        }

    }
    void TimeChange() //The rate of change function.
    {
        minusTime += changeInTime;

        STime -= minusTime;
    }

    IEnumerator wave()
    {
        while(true)
        {
            yield return new WaitForSeconds(STime);
            Spawn();
        }
    }
    void Spawn()
    {
        Instantiate(objectToSpawn, this.transform);
    }
}
