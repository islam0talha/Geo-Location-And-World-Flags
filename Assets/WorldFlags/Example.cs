﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Example : MonoBehaviour {
	public Image Flag;
	public SkinnedMeshRenderer _3DFlag;
	public Text CountryName;
	public Dropdown CountryList;
	public Button GetLocation;
	public void Start() {

		CountryList.options.Clear();
		CountryList.onValueChanged.AddListener(OnCountriesListChange);

		//Subscribe To Get Location Button
		GetLocation.onClick.AddListener(() => {
			//To use this lib probably you need to send arefrence to MonoBehaviour 
			CountriesFlags.GetLocation(this, (LocationData, isNetworkError) => {
				if (!isNetworkError) {
					//Reading and viewing the location data
					//Debug.Log("CountryCode: " + countryCode);
					Sprite sprite = CountriesFlags.LoadFlag(LocationData.countryCode);
					Flag.sprite = sprite;
					_3DFlag.material.mainTexture = sprite.texture;
					CountryName.text = CountriesFlagsData.countryCodes_NamesMapping[LocationData.countryCode] + ", " + LocationData.countryCode;
				} else {
					Debug.LogError("You must be connected to internet");
				}
			});
		});

		//Parse CountriesNames Enum and get list with all Countries names
		List<string> CountriesNames = new List<string>(System.Enum.GetNames(typeof(CountriesFlagsData.CountriesNames)));
		List<Dropdown.OptionData> CountryListData = new List<Dropdown.OptionData>();
		foreach (var name in CountriesNames) {
			CountryListData.Add(new Dropdown.OptionData(CountriesFlagsData.countryCodes_NamesMapping[name], CountriesFlags.LoadFlag(name)));
		}
		//Filling the list in UI
		CountryList.AddOptions(CountryListData);
		OnCountriesListChange(0);
	}

	public void OnCountriesListChange(int flagIndex) {
		Sprite sprite = CountriesFlags.LoadFlag((CountriesFlagsData.CountriesNames)flagIndex);
		Flag.sprite = sprite;
		_3DFlag.material.mainTexture = sprite.texture;
		CountryName.text = CountriesFlagsData.countryCodes_NamesMapping[((CountriesFlagsData.CountriesNames)flagIndex).ToString()] + ", " + ((CountriesFlagsData.CountriesNames)flagIndex).ToString();
	}
}
