using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatDisplayManager : MonoBehaviour {
    public Toggle toggle;
    public const string StatDisplayKey = "EnableStatDisplay";

    public GameObject attackObj;
    public GameObject attackSpeedObj;
    public GameObject armourObj;
    public GameObject speedObj;
    
    public TextMeshProUGUI attack;
    public TextMeshProUGUI attackSpeed;
    public TextMeshProUGUI armour;
    public TextMeshProUGUI speed;
    
    private PlayerWeapons _playerWeapons;
    private PlayerHealth _playerHealth;
    private PlayerMovement _playerMovement;
    void Awake() {
        PlayerWeapons.OnDamageMultiplierChange += UpdateStats;
        PlayerWeapons.OnAttackSpeedChange += UpdateStats;
        PlayerHealth.OnArmourChange += UpdateStats;
        PlayerMovement.OnSpeedChange += UpdateStats;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _playerWeapons = player.GetComponent<PlayerWeapons>();
        _playerHealth = player.GetComponent<PlayerHealth>();
        _playerMovement = player.GetComponent<PlayerMovement>();

        UpdateStats(this, EventArgs.Empty);
        
        bool on = PlayerPrefs.GetInt(StatDisplayKey, 1) == 1;
        toggle.isOn = on;
        TurnOnStatDisplay(on);
    }

    public void UpdateStats(object sender, EventArgs e) {
        attack.SetText(_playerWeapons.GetDamageMultiplier().ToString("F2") + "x");
        attackSpeed.SetText(_playerWeapons.GetSpeedMultiplier().ToString("F2") + "x");
        armour.SetText(_playerHealth.GetArmour().ToString("F2") + "x");
        speed.SetText(_playerMovement._currentMovementSpeed.ToString("F2"));
    }
    
    public void TurnOnStatDisplay(bool on) {
        if (on) {
            attackObj.SetActive(true);
            attackSpeedObj.SetActive(true);
            armourObj.SetActive(true);
            speedObj.SetActive(true);
            UpdateStats(this, EventArgs.Empty);
            PlayerPrefs.SetInt(StatDisplayKey, 1);
        }
        else {
            attackObj.SetActive(false);
            attackSpeedObj.SetActive(false);
            armourObj.SetActive(false);
            speedObj.SetActive(false);
            PlayerPrefs.SetInt(StatDisplayKey, 0);
        }
        PlayerPrefs.Save();
    }
    
    void OnDestroy() {
        PlayerWeapons.OnDamageMultiplierChange -= UpdateStats;
        PlayerWeapons.OnAttackSpeedChange -= UpdateStats;
        PlayerHealth.OnArmourChange -= UpdateStats;
        PlayerMovement.OnSpeedChange -= UpdateStats;
    }
}
