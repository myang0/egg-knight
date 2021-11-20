using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public GameObject knife;
    public GameObject fork;
    public GameObject spoon;

    public Transform knifeStart;
    public Transform forkStart;
    public Transform spoonStart;

    public Transform knifeEnd;
    public Transform forkEnd;
    public Transform spoonEnd;

    public enum SelectedWeapon {
        Fork, Knife, Spoon
    }

    private SelectedWeapon _currentWeapon;
    private float unselectedAlpha = 155f / 255f;
    
    void Start()
    {
        SetKnife(this, EventArgs.Empty);
        PlayerWeapons.OnSwitchKnife += SetKnife;
        PlayerWeapons.OnSwitchFork += SetFork;
        PlayerWeapons.OnSwitchSpoon += SetSpoon;
    }
    
    private IEnumerator AnimateMovement()
    {
        var t = 0f;
        var timeToMove = 0.25f;
        
        while(t < 1)
        {
            t += Time.deltaTime / timeToMove;
            if (_currentWeapon == SelectedWeapon.Fork)
            {
                fork.transform.position = Vector3.Lerp(fork.transform.position, forkEnd.position, t);
                knife.transform.position = Vector3.Lerp(knife.transform.position, knifeStart.position, t);
                spoon.transform.position = Vector3.Lerp(spoon.transform.position, spoonStart.position, t);
            }
            else if (_currentWeapon == SelectedWeapon.Knife)
            {
                fork.transform.position = Vector3.Lerp(fork.transform.position, forkStart.position, t);
                knife.transform.position = Vector3.Lerp(knife.transform.position, knifeEnd.position, t);
                spoon.transform.position = Vector3.Lerp(spoon.transform.position, spoonStart.position, t);
            }
            else if (_currentWeapon == SelectedWeapon.Spoon)
            {
                fork.transform.position = Vector3.Lerp(fork.transform.position, forkStart.position, t);
                knife.transform.position = Vector3.Lerp(knife.transform.position, knifeStart.position, t);
                spoon.transform.position = Vector3.Lerp(spoon.transform.position, spoonEnd.position, t);
            }
            yield return null;
        }
    }
    
    private void SetKnife(object sender, EventArgs e)
    {
        knife.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
        fork.GetComponent<Image>().color = new Color(255, 255, 255, unselectedAlpha);
        spoon.GetComponent<Image>().color = new Color(255, 255, 255, unselectedAlpha);
        
        StopCoroutine(AnimateMovement());
        _currentWeapon = SelectedWeapon.Knife;
        StartCoroutine(AnimateMovement());
    }
    
    private void SetFork(object sender, EventArgs e)
    {
        fork.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
        knife.GetComponent<Image>().color = new Color(255, 255, 255, unselectedAlpha);
        spoon.GetComponent<Image>().color = new Color(255, 255, 255, unselectedAlpha);
        StopCoroutine(AnimateMovement());
        _currentWeapon = SelectedWeapon.Fork;
        StartCoroutine(AnimateMovement());
    }
    
    private void SetSpoon(object sender, EventArgs e)
    {
        spoon.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
        fork.GetComponent<Image>().color = new Color(255, 255, 255, unselectedAlpha);
        knife.GetComponent<Image>().color = new Color(255, 255, 255, unselectedAlpha);
        StopCoroutine(AnimateMovement());
        _currentWeapon = SelectedWeapon.Spoon;
        StartCoroutine(AnimateMovement());
    }
}
