using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public Image knife;
    public Image fork;
    public Image spoon;

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
    
    void Start() {
        HideWeapon(SelectedWeapon.Fork);
        HideWeapon(SelectedWeapon.Spoon);
        SetKnife(this, EventArgs.Empty);
        PlayerWeapons.OnSwitchKnife += SetKnife;
        PlayerWeapons.OnSwitchFork += SetFork;
        PlayerWeapons.OnSwitchSpoon += SetSpoon;
        UnlockWeaponItem.OnPickup += ShowWeapon;
        PlayerControls.OnUnlockAllWeapons += ShowWeapon;
        PlayerControls.OnUnlockAllWeapons += ShowWeapon;
    }

    void Update() {
        if (fork.color.a == 1 && spoon.color.a == 1) {
            UnlockWeaponItem.OnPickup -= ShowWeapon;
            PlayerControls.OnUnlockAllWeapons -= ShowWeapon;
            PlayerControls.OnUnlockAllWeapons -= ShowWeapon;
        }
    }

    private void OnDestroy() {
        PlayerWeapons.OnSwitchKnife -= SetKnife;
        PlayerWeapons.OnSwitchFork -= SetFork;
        PlayerWeapons.OnSwitchSpoon -= SetSpoon;
        UnlockWeaponItem.OnPickup -= ShowWeapon;
        PlayerControls.OnUnlockAllWeapons -= ShowWeapon;
        PlayerControls.OnUnlockAllWeapons -= ShowWeapon;
    }

    private void ShowWeapon(object sender, System.EventArgs e) {
        if (fork.color.a == 0) {
            Color c = fork.color;
            fork.color = new Color(c.r, c.g, c.b, unselectedAlpha);
        } else if (spoon.color.a == 0) {
            Color c = spoon.color;
            spoon.color = new Color(c.r, c.g, c.b, unselectedAlpha);
        }
    }

    private void HideWeapon(SelectedWeapon weapon) {
        switch (weapon) {
            case SelectedWeapon.Fork: {
                Color c = fork.color;
                fork.color = new Color(c.r, c.g, c.b, 0);
                break;
            }
            case SelectedWeapon.Spoon: {
                Color c = spoon.color;
                spoon.color = new Color(c.r, c.g, c.b, 0);
                break;
            }
        }
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
        knife.color = new Color(255, 255, 255, 1f);
        if (fork.color.a != 0) fork.color = new Color(255, 255, 255, unselectedAlpha);
        if (spoon.color.a != 0) spoon.color = new Color(255, 255, 255, unselectedAlpha);
        
        StopCoroutine(AnimateMovement());
        _currentWeapon = SelectedWeapon.Knife;
        StartCoroutine(AnimateMovement());
    }
    
    private void SetFork(object sender, EventArgs e)
    {
        fork.color = new Color(255, 255, 255, 1f);
        knife.color = new Color(255, 255, 255, unselectedAlpha);
        if (spoon.color.a != 0) spoon.color = new Color(255, 255, 255, unselectedAlpha);
        StopCoroutine(AnimateMovement());
        _currentWeapon = SelectedWeapon.Fork;
        StartCoroutine(AnimateMovement());
    }
    
    private void SetSpoon(object sender, EventArgs e)
    {
        spoon.color = new Color(255, 255, 255, 1f);
        fork.color = new Color(255, 255, 255, unselectedAlpha);
        knife.color = new Color(255, 255, 255, unselectedAlpha);
        StopCoroutine(AnimateMovement());
        _currentWeapon = SelectedWeapon.Spoon;
        StartCoroutine(AnimateMovement());
    }
}
