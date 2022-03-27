using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePlayButton : MonoBehaviour {
    public bool isPlay;
    private Button btn;
    void Start() {
        btn = GetComponent<Button>();
    }
    void Update() {
        if(btn == null) return;
        if(GameManager.Instance == null) return;
        if(GameManager.Instance.RootGear == null) return;
        
        btn.interactable = isPlay != GameManager.Instance.RootGear.isRotating;
    }
}
