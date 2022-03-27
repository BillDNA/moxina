using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RotationDirectionToggle : MonoBehaviour {
    public bool isClockwise;
    private Toggle toggle;
    void Start(){
        toggle = GetComponent<Toggle>();
    }

    // Update is called once per frame
    void Update() {
        if(toggle == null) return;
        if(GameManager.Instance == null) return;
        if(GameManager.Instance.RootGear == null) return;
        
        toggle.isOn = GameManager.Instance.RootGear.RotateClockWise == isClockwise;
        
    }
}
