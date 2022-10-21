using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Location : MonoBehaviour
{
    public LocationInfo startPos;
    public LocationInfo currentPos;
    [SerializeField] private TMP_Text currentLong;
    [SerializeField] private TMP_Text currentLat;
    [SerializeField] private TMP_Text currentAlt;
    [SerializeField] private TMP_Text startLong;
    [SerializeField] private TMP_Text startLat;
    [SerializeField] private TMP_Text startAlt;
    [SerializeField] private TMP_Text logText;
    [SerializeField] private TMP_Text distanceText;
    [SerializeField] private GameObject marker;
    // Start is called before the first frame update
    void Start()
    {
        //initialize location services
        currentLong.text = "      Not Available";
        currentLat.text = "      Not Available";
        currentAlt.text = "      Not Available";
        StartCoroutine("startLocationServices");
        startLong.text = "      Not Available";
        startLat.text = "      Not Available";
        startAlt.text = "      Not Available";
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            currentPos = Input.location.lastData;
            currentLong.text = $"{currentPos.longitude,19:N6}";
            currentLat.text = $"{currentPos.latitude,19:N6}";
            currentAlt.text = $"{currentPos.altitude,19:N6}";
        }
    }

    IEnumerator startLocationServices()
    {

        #if UNITY_ANDROID
                if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.CoarseLocation)) {
                    UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.CoarseLocation);
                }
        #endif
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
        {
            print("Location not enabled");
            logText.text ="Location not enabled";
            yield break;
        }

        // Starts the location service.
        logText.text = "Attempting to Start Location Services";
        Input.location.Start(1f, 1f);

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            print("Timed out");
            logText.text = "Timed out";
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            logText.text = "Unable to determine device location";
            yield break;
        }
        logText.gameObject.SetActive(false);
        currentPos = Input.location.lastData;
        currentLong.text = $"{currentPos.longitude,19:N6}";
        currentLat.text = $"{currentPos.latitude,19:N6}";
        currentAlt.text = $"{currentPos.altitude,19:N6}";
    }

    public void setStartPos()
    {
        startPos = currentPos;
        startLong.text = $"{startPos.longitude,19:N6}";
        startLat.text = $"{startPos.latitude,19:N6}";
        startAlt.text = $"{startPos.altitude,19:N6}";
    }
    public void calculateDistance(){
        double lat1 = toRadians(startPos.latitude);
        double lat2 = toRadians(currentPos.latitude);

        // Haversine formula
        double dlon = toRadians(currentPos.longitude - startPos.longitude);
        double dlat = toRadians(currentPos.latitude - startPos.latitude);
        double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(lat1) * Math.Cos(lat2) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        // Radius of earth in
        // kilometers. Use 3956
        // for miles
        double r = 6371;
        double distance = c * r * 1000;
        distanceText.text = $"{distance,7:N3}m";
    }
    static double toRadians(
           double angleIn10thofaDegree)
    {
        // Angle in 10th
        // of a degree
        return angleIn10thofaDegree * Math.PI / 180;
    }

    public void placeMarker(){
        // Vector3 placement = GPSEncoder.GPSToUCS(currentPos.latitude, currentPos.longitude);
        // logText.gameObject.SetActive(true);
        // logText.text = $"Attempting to Place marker at {placement.x,0:N3}, {placement.y,0:N3}, {placement.z,0:N3}";
        GameObject newMarker = Instantiate(marker, Camera.main.transform.position, Quaternion.identity);
    }
}
