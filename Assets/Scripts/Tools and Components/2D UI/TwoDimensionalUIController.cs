using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class TwoDimensionalUIController : MonoBehaviour {

	public int Power { get; set; }
	public int SolderWireAmount { get; set; }
	public int TransistorAmount { get; set; }
	public int ResistorAmount { get; set; }
	private Text _powerField;
	private Text _solderField;
	private Text _transistorField;
	private Text _resistorField;
	private bool _firstRun = true;

	// Use this for initialization
	void OnEnable()
	{
		_powerField = transform.FindChild("txtPower").GetComponent<Text>();
		_solderField = transform.FindChild("txtSolderAmount").GetComponent<Text>();
		_transistorField = transform.FindChild("txtTransistorAmount").GetComponent<Text>();
		_resistorField = transform.FindChild("txtResistorAmount").GetComponent<Text>();
		Power = Player.player.Inventory.GetItems().Select(x => x.Prefab.GetComponent<WorldBattery>()).Sum(x => x.Power);
		SolderWireAmount = Player.player.Inventory.GetItems().Select(x => x.Prefab.GetComponent<WorldSolder>()).Sum(x => x.TileAmount);
	}
	
	// Update is called once per frame
	void Update ()
	{
		ResistorAmount = Player.player.Inventory.ResistorCount;
		TransistorAmount = Player.player.Inventory.TransistorCount;
		_resistorField.text = ResistorAmount.ToString("000");
		_transistorField.text = TransistorAmount.ToString("000");
		_powerField.text = Power.ToString("000");
		_solderField.text = SolderWireAmount.ToString("000");
		if (_firstRun)
		{
			transform.FindChild("lblTutorial").gameObject.SetActive(true);
		}
	}

	void OnDisable()
	{
		if (_firstRun)
		{
			transform.FindChild("lblTutorial").gameObject.SetActive(false);
			_firstRun = false;
		}
		List<WorldBattery> batteryAmount = Player.player.Inventory.GetItems().Select(x => x.Prefab.GetComponent<WorldBattery>()).ToList();
		List<WorldSolder> solderAmount = Player.player.Inventory.GetItems().Select(x => x.Prefab.GetComponent<WorldSolder>()).ToList();
		if (Convert.ToDecimal(batteryAmount.Sum(x => x.Power) % Power) != 0)
		{
			Player.player.Inventory.RemoveItem(batteryAmount[0].ItemID, 1);
			OnDisable();
		}
		if (Convert.ToDecimal(solderAmount.Sum(x => x.TileAmount) % SolderWireAmount) != 0)
		{
			Player.player.Inventory.RemoveItem(solderAmount[0].ItemID, 1);
			OnDisable();
		}
	}
}
