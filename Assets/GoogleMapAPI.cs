using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoogleMapAPI : MonoBehaviour {
    private float lat;
    private float lon;
    public int zoom = 14;
    public int mapWidth = 640;
    public int mapHeigh = 640;
    public enum mapType { roadmap, satellite, hybrid, terrain };
    public mapType mapSelected;
    public int scale = 1;

    LocationInfo li;
    private bool loadingMap;
    string url = "";

    void Start( ) {
        StartCoroutine( GetGPS( ) );
    }

    void Update( ) {
        GameObject.Find( "GPS" ).GetComponent<Text>( ).text = lat.ToString( ) + " : " + lon.ToString( );
    }

    IEnumerator GetGoogleMap( ) {
        url = "https://maps.googleapis.com/maps/api/staticmap?center="
            + lat + "," + lon + "&zoom=" + zoom + "&size="
            + mapWidth + "x" + mapHeigh + "&scale=" + scale
            + "&maptype=" + mapSelected
            + "&markers=color:red%7Clabel:S%7C" + lat + "," + lon
            + "&markers=color:green%7Clabel:G%7C40.711614,-74.012318"
            + "&markers=color:red%7Clabel:C%7C40.718217,-73.998284"
            + "&key=AIzaSyBa5xpdz76mxEDTj2aLSYhfH1gT1UjBWxo";
        loadingMap = true;
        WWW www = new WWW( url );
        yield return www;
        loadingMap = false;

        gameObject.GetComponent<Renderer>( ).material.mainTexture = www.texture;
    }

    IEnumerator GetGPS( ) {
        // First, check if user has location service enabled
        if ( !Input.location.isEnabledByUser )
            yield break;

        // Start service before querying location
        Input.location.Start( );

        // Wait until service initializes
        int maxWait = 20;
        while ( Input.location.status == LocationServiceStatus.Initializing && maxWait > 0 ) {
            yield return new WaitForSeconds( 1 );
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if ( maxWait < 1 ) {
            print( "Timed out" );
            yield break;
        }

        // Connection has failed
        if ( Input.location.status == LocationServiceStatus.Failed ) {
            print( "Unable to determine device location" );
            yield break;
        } else {
            // Access granted and location value could be retrieved
            print( "Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp );
        }

        lat = Input.location.lastData.latitude;
        lon = Input.location.lastData.longitude;

        StartCoroutine( GetGoogleMap( ) );

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop( );
    }

    public void updateGPS( ) {
        StartCoroutine( GetGPS( ) );
    }
}