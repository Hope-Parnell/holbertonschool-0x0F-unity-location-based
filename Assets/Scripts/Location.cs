using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Location : MonoBehaviour
{
    public LocationInfo startPos;
    private LocationInfo currentPos;
    // Start is called before the first frame update
    void Start()
    {
        //initialize location services
        // startLocationServices();
        // currentPos = Input.location.lastData;
    }

    // Update is called once per frame
    void Update()
    {
        // currentPos = Input.location.lastData;
    }

    void startLocationServices()
    {
        Input.location.Start(1f, 1f);
    }
}
