//Actual GPS service which is used to handle GPS data and paired to PLAYER and not to MAP (player moves on map according to GPS)
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GPSService : MonoBehaviour
{
    public float Latitude;
    public float Longitude;
    public float centerOfMapLatitude;
    public float centerOfMapLongitude;
    public Time time;
    private Image gpsError;

    IEnumerator Start()
    {
        gpsError = GameObject.Find("GPSErrorPanel").GetComponent<Image>();
        gpsError.gameObject.SetActive(false);
        // First, check if user has location service enabled
        //For testing use row below (no gps on test environment)
        //dummyGPSdata(65.05783f, 25.464038f);
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service before querying location
        Input.location.Start(5, 10);

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            InvokeRepeating("retrieveGPSdata", 0, 5f);
        }

    }

    void retrieveGPSdata()
    {
        //if (Latitude != Input.location.lastData.latitude || Longitude != Input.location.lastData.longitude)
        //{
        //    Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        Latitude = Input.location.lastData.latitude;
        Longitude = Input.location.lastData.longitude;
        //}

        float z = latToZ(Latitude);
        float x = lonToX(Longitude);
        Vector3 newposition = new Vector3(x, 0f, z);
        transform.position = Vector3.Lerp(transform.position, newposition, 60 * Time.deltaTime);
        Debug.Log("player position: " + x + "," + z);

        if (Longitude < 25.461735f || Longitude > 25.470498f || Latitude < 65.056384f || Latitude > 65.061963f)
        {
            gpsError.gameObject.SetActive(true);
        }
        else
        {
            gpsError.gameObject.SetActive(false);
        }
    }

    void dummyGPSdata(float dummylatitude, float dummylongitude)
    {

        Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        Latitude = dummylatitude;
        Longitude = dummylongitude;

        float z = latToZ(Latitude);
        float x = lonToX(Longitude);

        transform.position = new Vector3(x, 0f, z);
        LineRenderer renderer = new LineRenderer();
        renderer.SetPosition(0, new Vector3(x, 0f, z));
        renderer.SetPosition(1, new Vector3(20f, 0f, 190f));
        renderer.startColor = Color.green;
        renderer.endColor = Color.black;

        Debug.Log("player position: " + x + "," + z);

    }

    float latToZ(double lat)
    {
        //Original
        //lat = (lat - centerOfMapLatitude) / 0.00001 * 0.12179047095976932582726898256213;
        //Tuomas
        lat = (lat - centerOfMapLatitude) / 0.00001 * 1.77;
        double z = lat;

        return (float)z;
    }

    float lonToX(double lon)
    {
        //Original
        //lon = (lon - centerOfMapLongitude) / 0.000001 * 0.00728553580298947812081345114627;
        //Tuomas
        lon = (lon - centerOfMapLongitude) / 0.000001 * 0.0748;
        double x = lon;

        return (float)x;
    }


    public void OnGUI()
    {
        //GUI.Label(new Rect(10, 70, 100, 20), "Lat : " + Latitude.ToString(), GUI.skin.box);
        //GUI.Label(new Rect(10, 100, 100, 20), "Lon : " + Longitude.ToString(), GUI.skin.box);
    }
    public void OnDisable()
    {
        Input.location.Stop();
    }

    public void OnDestroy()
    {
        Input.location.Stop();
    }

}