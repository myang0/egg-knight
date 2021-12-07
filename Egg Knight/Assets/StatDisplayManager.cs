using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatDisplayManager : MonoBehaviour {
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
    }

    public void UpdateStats(object sender, EventArgs e) {
        attack.SetText(_playerWeapons.GetDamageMultiplier().ToString("F2"));
        attackSpeed.SetText(_playerWeapons.GetSpeedMultiplier().ToString("F2"));
        armour.SetText(_playerHealth.GetArmour().ToString("F2"));
        speed.SetText(_playerMovement._currentMovementSpeed.ToString("F2"));
    }
    
    void OnDestroy() {
        PlayerWeapons.OnDamageMultiplierChange -= UpdateStats;
        PlayerWeapons.OnAttackSpeedChange -= UpdateStats;
        PlayerHealth.OnArmourChange -= UpdateStats;
        PlayerMovement.OnSpeedChange -= UpdateStats;
    }
}
