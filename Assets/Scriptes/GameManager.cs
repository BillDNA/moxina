using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    #region Properties
        public List<GameObject> GearPrefabs;
        public CameraControlls cameraControlls;
        public Gear RootGear;
        public List<Gear> allGears;
        public static GameManager Instance;
    #endregion Properties
    #region Settings
        public float currentRotationSpeed = 25;
        public float maxRotationSpeed = 200;
    #endregion Settings
    private void Awake() {
        if(Instance == null) Instance = this; //Set up the singleton 
        if(allGears == null) allGears = new List<Gear>();
    }
    private void Start() {        
        OnAdjustSpeed(currentRotationSpeed/maxRotationSpeed);
    }
    #region Gear management
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
                //obviously this is not ideal for the function to fail to add a gear 
                //but it's practically imposable since there is no box that the gears
                //must fit it
                allGears.Remove(g);
                GameObject.Destroy(obj);
            } else {
                allGears.Add(g); 
            }
            //by zoom out so we can see the new gear
            if(cameraControlls != null) cameraControlls.ZoomToFitAllGears(allGears);
            
        }
        //checks to see that this new gear dose'nt overlap with any existing gears except the it's parrent gear
        public bool doseGearOverlay(Gear child, Gear Parrent) {
            foreach(Gear g in allGears) {
                if(g != Parrent && g.DoseOverlap(child)) return true;
            }
            return false;
        }
        //Clears all of the gears except the root gear.
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
    #endregion Gear management

    #region Button Handles
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
    #endregion Button Handles
}
