using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {

    public float xMargin = 1.0f;
    public float yMargin = 1.0f;

    public float xSmooth=10.0f;
    public float ySmooth=10.0f;

    public Vector2 maxXAndY;
    public Vector2 minXAndY;

    public Transform cameraTarget; 

    private float positionDeltaX;
    private float positionDeltaY;

	void Awake () 
    {
        cameraTarget = GameObject.FindGameObjectWithTag("CameraTarget").transform;
        Screen.orientation = ScreenOrientation.LandscapeLeft;

	}

    private bool CheckXMargin()
    {
        return Mathf.Abs(positionDeltaX)>xMargin;
    }

    private bool CheckYMargin()
    {
        return Mathf.Abs(positionDeltaY)>yMargin;
    }
	

	void FixedUpdate () 
    {
        positionDeltaX = transform.position.x - cameraTarget.position.x;
        positionDeltaY = transform.position.y - cameraTarget.position.y;
        TrackPlayer();
	}

    void TrackPlayer()
    {
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        if (CheckXMargin())
        {
            targetX = Mathf.Lerp(transform.position.x, cameraTarget.position.x, xSmooth * Time.deltaTime);
        }


        if (CheckYMargin())
        {
            targetX = Mathf.Lerp(transform.position.x, cameraTarget.position.x, xSmooth * Time.deltaTime);
        }

        targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
        targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
}
