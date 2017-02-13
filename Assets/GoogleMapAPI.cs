using UnityEngine;
using System.Collections;

public class GoogleMapAPI : MonoBehaviour {
	public float lat;
	public float lon;
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
		/*lat = Input.location.lastData.latitude;
		lon = Input.location.lastData.longitude;
		Debug.Log( lat.ToString( ) + " : " + lon.ToString( ) );*/
		StartCoroutine( GetGoogleMap( ) );
	}

	void Update( ) {
		
	}

	IEnumerator GetGoogleMap( ) {
		url = "https://maps.googleapis.com/maps/api/staticmap?center=" 
			+ lat + "," + lon + "&zoom=" + zoom + "&size=" 
			+ mapWidth + "x" + mapHeigh + "&scale=" + scale
			+ "&maptype=" + mapSelected 
			+ "&markers=color:blue%7Clabel:S%7C40.702147,-74.015794"
			+ "&markers=color:green%7Clabel:G%7C40.711614,-74.012318"
			+ "&markers=color:red%7Clabel:C%7C40.718217,-73.998284"
			+ "&key=YOUR_KEY_CODE";
		loadingMap = true;
		WWW www = new WWW( url );
		yield return www;
		loadingMap = false;
		
		gameObject.GetComponent<Renderer>( ).material.mainTexture = www.texture;
	}
}