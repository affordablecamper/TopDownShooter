using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {
	public Transform trackedObject,trackedObjectZoom,targetCamera;
	public Vector3 offset;
    public float cameraMovement;
    static GameCamera myslf;
	Misc_Timer shakeTimer = new Misc_Timer ();
	Transform currentTrackedObject;
	void Awake(){
		myslf = this;
	}
	// Use this for initialization
	void Start () {
		currentTrackedObject = trackedObject;

	}

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentTrackedObject = trackedObjectZoom;
            targetCamera.position = Vector3.Lerp(targetCamera.position, currentTrackedObject.position, 0.05f) + offset;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentTrackedObject = trackedObject;
            targetCamera.position = Vector3.Lerp(targetCamera.position, currentTrackedObject.position, 0.05f) + offset;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {

        //targetCamera.position = Vector3.Lerp (targetCamera.position, currentTrackedObject.position, 0.05f)+offset;
        Vector3 pos = Vector3.Lerp(targetCamera.position, currentTrackedObject.position, 0.05f) + offset;
        targetCamera.position = pos;
        shakeTimer.UpdateTimer ();
		if (shakeTimer.IsActive())
			UpdateShake ();
		
	}
	float shakeDelay=0.03f,lastShakeTime=float.MinValue;
	void UpdateShake(){
		if (lastShakeTime + shakeDelay < Time.time) {
			Vector3 shakePosition = Vector3.zero;
			shakePosition.x += Random.Range (-0.25f, 0.25f);
			shakePosition.y += Random.Range (-0.25f, 0.25f);
			targetCamera.transform.Translate(shakePosition);
			//targetCamera.transform.localPosition = shakePosition+targetCamera.transform.localPosition;
			lastShakeTime=Time.time;
		}
	}
	//Vector3 camLocalPos;
	//bool shakeActive;
	public static void ToggleShake(float shakeTime){
		myslf.shakeTimer.StartTimer (shakeTime);
	//	myslf.shakeActive = toggleValue;
		//if (!toggleValue) {
		//	myslf.targetCamera.transform.localPosition=myslf.camLocalPos;
		//}
	}
}
