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
    // private int angle = 15;
    // private int angledHeight = 174;
    // private int centerHeight = 199;
    // private int leftX = 149;
    // private int centerX = 199;
    // private int rightX = 249;
    private float unselectedAlpha = 155f / 255f;
    
    // Start is called before the first frame update
    void Start()
    {
        // _positionLeft.position = new Vector3(leftX, angledHeight, 0);;
        // _positionCenter.position = new Vector3(centerX, centerHeight, 0);;
        // _positionRight.position = new Vector3(rightX, angledHeight, 0);;
        
        // fork.transform.rotation = 
        
        // SetKnife(this, EventArgs.Empty);
        PlayerControls.On1Press += SetKnife;
        PlayerControls.On2Press += SetFork;
        PlayerControls.On3Press += SetSpoon;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("KNIFE: " + knife.transform.position);
        // Debug.Log("FORK: " + fork.transform.position);
        // Debug.Log("SPOON: " + spoon.transform.position);
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
        // knife.transform.position = forkStart.position;
        // fork.transform.position = spoonStart.position;
        // spoon.transform.position = knifeStart.position;
    }
    
    private void SetFork(object sender, EventArgs e)
    {
        fork.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
        knife.GetComponent<Image>().color = new Color(255, 255, 255, unselectedAlpha);
        spoon.GetComponent<Image>().color = new Color(255, 255, 255, unselectedAlpha);
        StopCoroutine(AnimateMovement());
        _currentWeapon = SelectedWeapon.Fork;
        StartCoroutine(AnimateMovement());
        // fork.transform.position = forkStart.position;
        // spoon.transform.position = spoonStart.position;
        // knife.transform.position = knifeStart.position;
    }
    
    private void SetSpoon(object sender, EventArgs e)
    {
        spoon.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
        fork.GetComponent<Image>().color = new Color(255, 255, 255, unselectedAlpha);
        knife.GetComponent<Image>().color = new Color(255, 255, 255, unselectedAlpha);
        StopCoroutine(AnimateMovement());
        _currentWeapon = SelectedWeapon.Spoon;
        StartCoroutine(AnimateMovement());
        // spoon.transform.position = forkStart.position;
        // knife.transform.position = spoonStart.position;
        // fork.transform.position = knifeStart.position;
    }
}
