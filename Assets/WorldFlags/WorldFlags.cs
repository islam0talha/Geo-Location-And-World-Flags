using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public static class CountriesFlags {
	public static Sprite LoadFlag(string CountryShortCode) {
		return Resources.Load<Sprite>(CountryShortCode);
	}
	public static Sprite LoadFlag(CountriesFlagsData.CountriesNames name) {
		return Resources.Load<Sprite>(name.ToString());
	}
	/// <summary>
	/// The Geo data for a user.
	/// You are free to use ip-api.com for non-commercial use. We do not allow commercial use without prior approval.
	/// So if you want to use it in commercial use -> https://signup.ip-api.com/
	/// 
	/// http://ip-api.com/docs/api:json
	/// 
	/// <code>
	/// {
	/// 	"status": "success",
	/// 	"country": "COUNTRY",
	/// 	"countryCode": "COUNTRY CODE",
	/// 	"region": "REGION CODE",
	/// 	"regionName": "REGION NAME",
	/// 	"city": "CITY",
	/// 	"zip": "ZIP CODE",
	/// 	"lat": LATITUDE,
	/// 	"lon": LONGITUDE,
	/// 	"timezone": "TIME ZONE",
	/// 	"isp": "ISP NAME",
	/// 	"org": "ORGANIZATION NAME",
	/// 	"as": "AS NUMBER / NAME",
	/// 	"query": "IP ADDRESS USED FOR QUERY"
	/// }
	/// </code>
	/// 
	/// </summary>
	public class LocationData {
		/// <summary>
		/// The status that is returned if the response was successful.
		/// </summary>
		public string status;
		public string country;
		public string countryCode;
		public string region;
		public string regionName;
		public string city;
		public string zip;
		public string lat;
		public string lon;
		public string timezone;
		public string isp;
		public string org;
		public string query;
	}

	//Check http://ip-api.com/docs/api:json for commercial and usage limits

	/// <summary>
	/// Method to get your current location with IP and return the results in Done Action
	/// </summary>
	/// <param name="MonoThread">MonoBehaviour class to run a Coroutine</param>
	/// <param name="Done">Action will be called after Web Request return</param>
	public static void GetLocation(MonoBehaviour MonoThread, UnityAction<LocationData, bool> Done) {
		MonoThread.StartCoroutine(GetLocationCoroutine(Done));
	}
	public static IEnumerator GetLocationCoroutine(UnityAction<LocationData, bool> Done) {
		UnityWebRequest www = UnityWebRequest.Get("http://ip-api.com/json");
		www.SendWebRequest();
		while (!www.isDone) {
			yield return new WaitForSeconds(0.1f);
		}
		Debug.Log(www.downloadHandler.text);
		if (www.isNetworkError) {
			Debug.LogWarning("Error While Sending: " + www.error);
			Done(null, true);
		} else {
			LocationData data = JsonUtility.FromJson<LocationData>(www.downloadHandler.text);
			//Debug.Log("Received: " + www.downloadHandler.text);
			//Debug.Log(JsonUtility.ToJson(data));
			Done(data, false);
		}
	}
}
