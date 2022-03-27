using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Gear : MonoBehaviour {
    #region Properties
        [Tooltip("The number of teeth on the gear")]
        public int toothCount = 5;
        [Tooltip("The angle needed such that at the 3 O'clock position a tooth is centered.")]
        public float toothOffset = 26.6f; 
        //for conveniance
        public float toothAngle {
            get {
                return 360f / toothCount;
            }
        }
        [Tooltip("The distance from which non attached gears can not overlay")]
        public float OverlapRadius = 1.5f; 
        [Tooltip("The distance this gear's center needs to be from the edge of another gears mesh radius")]
        public float MeshRadius = 0.75f; 
        public List<Gear> attachedGears; //the gears which this gear turns

        //Root Gear properties
        public bool isRootGear;
        public bool isRotating; //turns on and of animations
        public bool RotateClockWise; //determins intital direction
        [Range(0,200)]
        public float RotationSpeed; //how fast we rotate in degrees/second
    #endregion Properties
    #region MonoBehaviour
        void Start() {
            if(attachedGears == null) attachedGears = new List<Gear>(); 
        }
        
        void Update() {
            if(!isRootGear) return; //if we are not the root gear our parrent gears should rotate us
            
            if(!isRotating) return; //if we are paused then do nothing

            //calculate how far we rotated in the last frame
            float delta = (RotateClockWise ? -1 : 1) * RotationSpeed * Time.deltaTime; 
            //update our rotation
            transform.localEulerAngles = new Vector3(0,0,transform.localEulerAngles.z+delta);
            //have all attached gears rotate based on our delta
            RotateChildren(delta);
        }
    #endregion MonoBehaviour

    #region  Gear Creation
        public bool AddGear(Gear other) {
            //first we try an add it directly to us
            float mr = MeshRadius + other.MeshRadius; //how far appart these two should be
            float theta = Random.Range(0,2*Mathf.PI); //the angle at which we will try to add this gear
            //calculate the location
            float x = mr*Mathf.Cos(theta) + transform.position.x;
            float y = mr*Mathf.Sin(theta) + transform.position.y;
            //set the location
            other.transform.position = new Vector3(x,y,0);
            //check if we are overlaping any existing gears
            if(!GameManager.Instance.doseGearOverlay(other,this)) {
                float a = GetRotationOffestForChild(theta,other); //the starting offset of the gears rotation to make the gears mesh
                other.transform.localEulerAngles = new Vector3(0,0,a);
                //attach the gear
                attachedGears.Add(other);
                return true; //gear got added
            } else if(attachedGears.Count > 0) { //if we have attached gears ask them to add the gear
                foreach(Gear g in attachedGears) {
                    if(g.AddGear(other)) return true; //if they did retrun out
                }
            }
            return false; //no gear was added
        }  
    public bool DoseOverlap(Gear other) {
        if(attachedGears.Contains(other)) return false; //they are allowed to overlap
        float distSq =
            Mathf.Pow(transform.position.x - other.transform.position.x,2) +
            Mathf.Pow(transform.position.y - other.transform.position.y,2);
        float radSumSq = Mathf.Pow(OverlapRadius + other.OverlapRadius,2);

        return distSq < radSumSq;
    }
    #endregion gear creation
    #region Rotation
        public void RotateBy(float delta, Gear other) {
            delta *=  toothAngle / other.toothAngle; //adjust the angle change based on teeth count
            //update the rotation
            transform.localEulerAngles = new Vector3(0,0,transform.localEulerAngles.z+delta);
            //rotate all attached gears
            RotateChildren(delta);
        }
        //Rotates all attached gears by given amount
        private void RotateChildren(float delta) {
            foreach(Gear g in attachedGears) {
                g.RotateBy(-delta,this);//negative Because gears alternate direction each mesh
            }
        }

        //calculated the rotation for a child gear so the teeth mesh properly
        //theta - the angle of center point to center point 
        //child - the gear to be rotated
        private  float GetRotationOffestForChild(float theta, Gear child) {
            theta = theta * 57.2958f; //convert to degree
            float r = child.toothAngle / 2f + child.toothOffset + 180; //adjust to not tooth zero
            float t = (transform.localEulerAngles.z - toothOffset) * (-child.toothAngle/toothAngle);  //match spin off set
            float progressThroughArc = ((theta / toothAngle) - Mathf.Floor(theta / toothAngle))*child.toothAngle;
            return r +t  + theta +progressThroughArc;
        }
    #endregion Rotation

#if UNITY_EDITOR
    private void  OnDrawGizmos() {
        //Gizmos.DrawCube(transform.position,MeshRadius);
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position,Vector3.forward,MeshRadius,2);
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position,Vector3.forward,OverlapRadius,2);
        
    }
#endif
}
#if UNITY_EDITOR

//So we can tell whats going on as we set up the scene
[CustomEditor(typeof(Gear))]
public class DrawWireArc : Editor {
    void OnSceneGUI() {

        Gear g = (Gear)target;
        Handles.color = Color.blue;
        foreach(Gear c in g.attachedGears) {
            if(g != null) Handles.DrawLine(g.transform.position,c.transform.position,2);
        }
    }
}
#endif 
