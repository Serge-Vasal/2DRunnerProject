using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunMover : MonoBehaviour {

    public Transform player;
    public RectTransform gunDirectionRect;
    public RectTransform canvas;
    public Transform gunNull;
    public RectTransform gunRect;
    public Transform gunTargetTransform;
    public Transform worldGunNull;
    public bool gunRight;
    public Transform gunRawRotation;

    private Vector3 worldPointTouchInGunDirectionRect;
    private Vector3 worldPointGunNull;  
    private Quaternion q;




	
	void Start () {
        gunRight = true;
	}
	
	
	void Update () {
        transform.position = new Vector3(gunRight?player.position.x+1f:player.position.x-1f,player.position.y,player.position.z);
        worldPointGunNull = Camera.main.ScreenToWorldPoint(gunNull.position);
        worldPointGunNull.z = 0.0f;

        Touch[] allTouches = Input.touches;
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(gunDirectionRect,allTouches[i].position))
            {

                RectTransformUtility.ScreenPointToWorldPointInRectangle(gunDirectionRect,allTouches[i].position,Camera.main,
                    out worldPointTouchInGunDirectionRect);
                if (worldPointTouchInGunDirectionRect.x <= worldPointGunNull.x&&gunRight)
                {
                    gunRight = !gunRight;
                    player.Rotate(Vector3.up, 180.0f, Space.World);
                }
                else if (worldPointTouchInGunDirectionRect.x > worldPointGunNull.x&&!gunRight)
                {
                    gunRight = !gunRight;
                    player.Rotate(Vector3.up, 180.0f, Space.World);
                }
                
                gunTargetTransform.position=worldPointTouchInGunDirectionRect;
                worldGunNull.position = worldPointGunNull;
                gunRawRotation.LookAt(gunTargetTransform);
                Debug.Log("Starting angle:  "+gunRawRotation.rotation.eulerAngles);
                q = gunRawRotation.rotation;
                q.x /= q.w;
                q.y /= q.w;
                q.z /= q.w;
                q.w = 1.0f;
                float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);
                Debug.Log("Modified angle:  "+q);
                angleX = Mathf.Clamp (angleX, -45f, 45f);
                if (worldPointTouchInGunDirectionRect.x <= worldPointGunNull.x&&!gunRight)
                {
                    worldGunNull.rotation = Quaternion.Euler(angleX,270f,0f);
                }
                else if (worldPointTouchInGunDirectionRect.x > worldPointGunNull.x&&gunRight)
                {
                    worldGunNull.rotation = Quaternion.Euler(angleX,90f,0f);
                }

                //q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);





                transform.rotation = worldGunNull.rotation;
                gunRect.rotation=worldGunNull.rotation;
                gunRect.Rotate(Vector3.up, 270.0f, Space.Self);

                gunNull.rotation=worldGunNull.rotation;


            }
        }		
	}


}
