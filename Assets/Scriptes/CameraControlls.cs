using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlls : MonoBehaviour { 
    private Camera camera;
    private void Start() {
        camera = GetComponent<Camera>();
    }
    [Range(1,500)]
    public float ZoomSpeed = 5;

    public float edgePanSpeed =0.5f;
    [Range(0,0.15f)]
    public float edgePercent = 0.05f;

    private bool isInPanMode;
    private Vector3 panLastPos = Vector3.zero;
    public RectTransform EdgePanNorth;
    public RectTransform EdgePanSouth;
    public RectTransform EdgePanWest;
    public RectTransform EdgePanEast;

    private void Update() {
        
        if(Input.mouseScrollDelta.y < 0) {
            camera.orthographicSize += Time.deltaTime * ZoomSpeed;
        } else if(Input.mouseScrollDelta.y > 0) {
            camera.orthographicSize = Mathf.Max(5,camera.orthographicSize - Time.deltaTime * ZoomSpeed);
        }
        if( Input.GetMouseButton(1)) {
            if(!isInPanMode) panLastPos = Input.mousePosition;
            isInPanMode = true;
        } else {
            isInPanMode = false;
        }
        
        if(isInPanMode) {
            Vector3 delta = Input.mousePosition - panLastPos;
            transform.localPosition -= delta * Time.deltaTime;// * camera.orthographicSize;
            panLastPos = Input.mousePosition;
        } else {

            float h = Screen.height * edgePercent;
            float w = Screen.width * edgePercent;
            float mx = Input.mousePosition.x;
            float my = Input.mousePosition.y;
            EdgePanNorth.sizeDelta = new Vector2(h,Screen.width);
            EdgePanNorth.gameObject.SetActive(my >= Screen.height - h);
            if (my >= Screen.height - h) transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * edgePanSpeed,-10);

            EdgePanSouth.sizeDelta =  new Vector2(h,Screen.width);
            EdgePanSouth.gameObject.SetActive(my <= h);
            if (my <= h) transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * edgePanSpeed,-10);

            EdgePanEast.sizeDelta = new Vector2(w, Screen.height);
            EdgePanEast.gameObject.SetActive(mx >= Screen.width - w);
            if (mx >= Screen.width - w) transform.position = new Vector3(transform.position.x + Time.deltaTime * edgePanSpeed,transform.position.y,-10 );
           
           
            EdgePanWest.sizeDelta = new Vector2(w, Screen.height);
            EdgePanWest.gameObject.SetActive(mx <= w);
            if (mx <= w) transform.position = new Vector3(transform.position.x - Time.deltaTime * edgePanSpeed,transform.position.y,-10 );
                
                
        }

        if(isInDynamicZoomMode) DynamicZoomUpdate();
    }


    #region Dynamic Zooming
        [Tooltip("how long should it take to zoom.")]
        public float DynamicZoomTime = 1f;
        private float DynamicZoomTimeLeft = 1f;
        public bool isInDynamicZoomMode;
        private Vector2 DynamicZoomOrthographicSizeChange;
        public Vector3 DynamicZoomOrigin;
        public Vector3 DynamicZoomDestination;

        private void DynamicZoomUpdate() {
            if(DynamicZoomTime <= 0 || DynamicZoomTimeLeft <= 0) {
                camera.orthographicSize = DynamicZoomOrthographicSizeChange.y;
                transform.position = DynamicZoomDestination;
                isInDynamicZoomMode = false;
                return;
            }
            camera.orthographicSize = Mathf.Lerp(DynamicZoomOrthographicSizeChange.x,DynamicZoomOrthographicSizeChange.y,1 - DynamicZoomTimeLeft / DynamicZoomTime);

            transform.position = Vector3.Lerp(DynamicZoomOrigin,DynamicZoomDestination,1 - DynamicZoomTimeLeft / DynamicZoomTime);
            
            DynamicZoomTimeLeft -= Time.deltaTime;
        }
        private Vector2 min = Vector2.zero;
        private Vector2 max = Vector2.zero;
        public void ZoomToFitAllGears(List<Gear> allGears) {
            min = Vector2.zero;
            max = Vector2.zero;

            foreach(Gear g in allGears) {
                float left = g.transform.position.x - g.OverlapRadius;
                float right = g.transform.position.x + g.OverlapRadius;
                float top = g.transform.position.y + g.OverlapRadius;
                float bot = g.transform.position.y - g.OverlapRadius;

                if(min.x > left) min.x = left;
                if(min.y > bot) min.y = bot;
                if(max.x < right) max.x = right;
                if(max.y < top) max.y = top;
            }
            //Add some padding to the zoom
            min = min - new Vector2(1,1);
            max = max + new Vector2(1,1); 
            DynamicZoomOrthographicSizeChange = new Vector2(
                camera.orthographicSize,
                Mathf.Max(2,(max.y - min.y)/2f)
            );
            DynamicZoomOrigin = transform.position;
            DynamicZoomDestination = new Vector3(
                min.x + (max.x - min.x)/2f,
                min.y + (max.y - min.y)/2f,
                -10
            );
            DynamicZoomTimeLeft = DynamicZoomTime;
            isInDynamicZoomMode = true;
        }
        #if UNITY_EDITOR
            private void OnDrawGizmos() {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(new Vector3(min.x,min.y,-10),new Vector3(max.x,min.y,-10));
                Gizmos.DrawLine(new Vector3(max.x,min.y,-10),new Vector3(max.x,max.y,-10));
                Gizmos.DrawLine(new Vector3(max.x,max.y,-10),new Vector3(min.x,max.y,-10));
                Gizmos.DrawLine(new Vector3(min.x,max.y,-10),new Vector3(min.x,min.y,-10));
            }
        #endif
    #endregion Dynamic Zooming
}
