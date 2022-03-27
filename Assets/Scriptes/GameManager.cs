using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public List<GameObject> GearPrefabs;
    public CameraControlls cameraControlls;
    public Gear RootGear;
    public List<Gear> allGears;

    public static GameManager Instance;
    private void Awake() {
        if(Instance == null) Instance = this;
        if(allGears == null) allGears = new List<Gear>();
    }
    private void Start() {
        
        OnAdjustSpeed(currentRotationSpeed/maxRotationSpeed);
    }

    public void AddGear() {
        //select a random Gear
        GameObject prefab = GearPrefabs[Random.Range (0, GearPrefabs.Count)];
        
        //Create the game object
        GameObject obj = GameObject.Instantiate(prefab, Vector3.zero,Quaternion.identity);
        obj.name = $"Gear {allGears.Count}";//for debuging
        Gear g = obj.GetComponent<Gear>();
        int attemptsLeft = 10; //So we don't freeze up
        //try and place a new gear somewhere in the chain
        while(!RootGear.AddGear(g) && attemptsLeft > 0) { 
            attemptsLeft--;
        }
        if(attemptsLeft <=0) {
            allGears.Remove(g);
            GameObject.Destroy(obj);
        } else {
            allGears.Add(g); 
        }

        if(cameraControlls != null) cameraControlls.ZoomToFitAllGears(allGears);
        
    }
    public void OnReset() {
        foreach(Gear g in allGears) {
            if(g != RootGear) {
                GameObject.Destroy(g.gameObject);
            }
        }
        RootGear.attachedGears = new List<Gear>();
        allGears = new List<Gear>() {RootGear};

        if(cameraControlls != null) cameraControlls.ZoomToFitAllGears(allGears);

    }

    public bool doseGearOverlay(Gear child, Gear Parrent) {
        foreach(Gear g in allGears) {
            if(g != Parrent && g.DoseOverlap(child)) return true;
        }
        return false;
    }
    public float currentRotationSpeed = 25;
    public float maxRotationSpeed = 200;
    public void OnAdjustSpeed(float p) {
        currentRotationSpeed = p * maxRotationSpeed;
        if(RootGear == null) return;
        RootGear.isRotating = true;
        RootGear.RotationSpeed = currentRotationSpeed;
    }

    public void OnSwitchRotation(bool isClockwise) {
        if(RootGear == null) return;
        RootGear.RotateClockWise = isClockwise;
    }
    public void OnShuffle() {
        OnReset();
        for(int i = 0; i < 10; i++) {
            AddGear();
        }
    }
    public void OnZoom() {
        cameraControlls.ZoomToFitAllGears(allGears);
    }
    public void OnPlay() {
        RootGear.isRotating = true;
    }
    public void OnPause() {
        RootGear.isRotating = false;
    }
    public void OnReverse() {
        RootGear.RotateClockWise = !RootGear.RotateClockWise;
    }
}
