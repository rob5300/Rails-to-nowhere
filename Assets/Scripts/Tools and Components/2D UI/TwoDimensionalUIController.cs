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

    public static TwoDimensionalUIController UI;
    private int _oldPower;
    private int _oldSolderCount;
    private int _oldBatteryCount;

    private void Start()
    {
        if (!UI) UI = this;
        gameObject.SetActive(false);
    }

    // Use this for initialization

	// Use this for initialization
	public void FindComponents()
	{
		_powerField = transform.FindChild("txtPower").GetComponent<Text>();
		_solderField = transform.FindChild("txtSolderAmount").GetComponent<Text>();
		_transistorField = transform.FindChild("txtTransistorAmount").GetComponent<Text>();
		_resistorField = transform.FindChild("txtResistorAmount").GetComponent<Text>();
        List<Inventory.ItemSlot> slots = Player.player.Inventory.GetPopulatedItemSlots().Where(x => x.ItemID == "puzzle.battery").ToList();
        List<WorldBattery> batteries = Player.player.Inventory.GetItems().Where(x => x.Prefab.GetComponentInChildren<WorldBattery>() != null).Select(x => x.Prefab.GetComponentInChildren<WorldBattery>()).ToList();
        if (slots.Sum(x => x.ItemQuantity) != _oldBatteryCount || _oldBatteryCount == 0)
        {
            if (slots.Count != 0)
            {
                Power = slots.Sum(x => x.ItemQuantity) * batteries[0].Power;
            }
            else
            {
                Power = 0;
            }
        }
        else
        {
            Power = _oldBatteryCount;
        }
        List<Inventory.ItemSlot> solderSlots = Player.player.Inventory.GetPopulatedItemSlots().Where(x => x.ItemID == "puzzle.solderwire").ToList();
        List<WorldSolder> solderCoils = Player.player.Inventory.GetItems().Where(x => x.Prefab.GetComponentInChildren<WorldSolder>() != null).Select(x => x.Prefab.GetComponentInChildren<WorldSolder>()).ToList();
        if (solderSlots.Sum(x => x.ItemQuantity) != _oldSolderCount || _oldSolderCount == 0)
        {
            if (solderSlots.Count != 0)
            {
                SolderWireAmount = solderSlots.Sum(x => x.ItemQuantity) * solderCoils[0].TileAmount;
            }
            else
            {
                SolderWireAmount = 0;
            }
        }
        else
        {
            SolderWireAmount = _oldSolderCount;
        }
        ResistorAmount = Player.player.Inventory.ResistorCount;
        TransistorAmount = Player.player.Inventory.TransistorCount;

    }
	
	// Update is called once per frame
	void Update ()
	{
		_resistorField.text = ResistorAmount.ToString("000");
		_transistorField.text = TransistorAmount.ToString("000");
		_powerField.text = Power.ToString("000");
		_solderField.text = SolderWireAmount.ToString("000");
		if (_firstRun)
		{
			transform.FindChild("lblTutorial").gameObject.SetActive(true);
		}
	}

	public void ValidateInventory()
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
            ValidateInventory();
		}

        _oldPower = Power;
        List<Inventory.ItemSlot> slots = Player.player.Inventory.GetPopulatedItemSlots().Where(x => x.ItemID == "puzzle.battery").ToList();
        _oldBatteryCount = slots.Sum(x => x.ItemQuantity);

        if (Convert.ToDecimal(solderAmount.Sum(x => x.TileAmount) % SolderWireAmount) != 0)
        {
            Player.player.Inventory.RemoveItem(solderAmount[0].ItemID, 1);
            ValidateInventory();
        }
        _oldSolderCount = SolderWireAmount;
        List<Inventory.ItemSlot> solderSlots = Player.player.Inventory.GetPopulatedItemSlots().Where(x => x.ItemID == "puzzle.solderwire").ToList();
        _oldSolderCount = solderSlots.Sum(x => x.ItemQuantity);
        gameObject.SetActive(false);
	}
}
