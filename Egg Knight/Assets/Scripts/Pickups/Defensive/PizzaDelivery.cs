using System;
using Stage;
using UnityEngine;

public class PizzaDelivery : BaseItem {
    public GameObject pizzaSlice;

    protected override void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            StageManager.OnStageStart += SpawnPizza;
            base.PickUp();
        }
    }

    private void SpawnPizza(object sender, EventArgs e) {
        LevelManager levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        StageManager currentStage = levelManager.GetCurrentStage();
        DeliverySpot deliveryLocation = currentStage.GetComponentInChildren<DeliverySpot>();
        if (deliveryLocation) {
            Instantiate(pizzaSlice, deliveryLocation.transform.position, Quaternion.identity);
        }
        else {
            Debug.LogError("Stage " + currentStage.gameObject.name + " does not have a pizza delivery spot.");
        }
        
    }
}