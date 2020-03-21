using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelManager : MonoBehaviour
{
    public enum WheelState
    {
        Idle,
        Spinning,
        Interacted,
        ReachingDestination,

    }
    
	[SerializeField] private GameObject boxPrefab = null;
	[SerializeField] private int visibleBoxes = 5;
	[SerializeField] private float distanceInDegrees = 5f;
	[SerializeField] private float radius = 5f;
    float distanceRad;
    [SerializeField] private float speedDampening = 0.9f;
	[SerializeField] private float userSpeedMod = 0.25f;
	[SerializeField] private float snapForce = 1f;
	[SerializeField] private AnimationCurve snapForceCurve = null;

    WheelState state;

	[SerializeField] private float speed = 1f;
	private float userSpeed = 0;
	private List<GameObject> boxes = new List<GameObject>();
	private Rigidbody rb;

	private float wheelRotation = 0;

	private Vector2 lastTouchPos = new Vector2 (-1, -1);
	
	// Start is called before the first frame update
    void Start()
    {
		rb = gameObject.GetComponent<Rigidbody>();
		rb.centerOfMass = gameObject.transform.position;
        for(int i = 0; i < visibleBoxes; i++)
		{
			AddBox();
		}
        distanceRad = Mathf.Deg2Rad * distanceInDegrees;
    }

    // Update is called once per frame
    void Update()
    {
		Touchinput();
		CalcSpeed ();
		//rb.AddTorque(new Vector3(userSpeed *100, 0, 0), ForceMode.Acceleration);
		//gameObject.transform.Rotate(Vector3.right, speed * Time.deltaTime);
		gameObject.transform.rotation = Quaternion.Euler(wheelRotation, 0, 0);
		if (Mathf.Abs(wheelRotation % distanceInDegrees) < Mathf.Abs(speed))
		{
			SwapBoxPosition(speed > 0);
		}
    }

	private void AddBox ()
	{
		var box = Instantiate(boxPrefab, transform);
		boxes.Add(box);
		box.transform.position = GetPostitonForBox(boxes.Count - 1);
	}

	private void SwapBoxPosition (bool topToBottom)
	{
		GameObject movedBox;
		if (topToBottom)
		{
			movedBox = boxes[boxes.Count - 1];
			boxes.RemoveAt(boxes.Count - 1);
			boxes.Insert(0, movedBox);
			movedBox.transform.position = GetPostitonForBox(0);
		} else
		{
			movedBox = boxes[0];
			boxes.RemoveAt(0);
			boxes.Add(movedBox);
			movedBox.transform.position = GetPostitonForBox(boxes.Count - 1);
		}
	} 

	private Vector3 GetPostitonForBox (int index)
	{
		float angle = index * distanceInDegrees - 2 * distanceInDegrees;
		return gameObject.transform.position - (Quaternion.Euler(angle, 0, 0) * new Vector3(0, 0, radius));
	}

	private void CalcSpeed ()
	{
		float distanceToRest = (wheelRotation - distanceInDegrees/2) % distanceInDegrees - distanceInDegrees / 2;
		float distanceToRestPercent = distanceToRest / (distanceInDegrees / 2);
		Debug.Log("distance to rest: " + distanceToRest + " | " + distanceToRestPercent);
		if (Input.GetMouseButton(0))
		{
			speed = userSpeed * userSpeedMod;
			wheelRotation += speed;
			
			
		}
        else
		{




			speed += (distanceToRestPercent) * snapForce * -1;


			//if (speed > 0.1f)
			//{
				speed *= speedDampening;
			//}
			//else
			//{
			//	speed *= speedDampening + (speed -0.1f);
			//}
			wheelRotation += speed;



			//         float angleSegment = (transform.rotation.eulerAngles.x - distanceInDegrees / 2);
			//         float modAngles =  angleSegment % distanceInDegrees;
			//         Debug.Log(modAngles);
			//         float distFromRestPos = modAngles - distanceInDegrees / 2;
			//float distFromRestPercent = distFromRestPos / (distanceInDegrees / 2);

			//speed += distFromRestPercent * snapForce; 

			//speed *= speedDampening;

			/*

            float angleSegment = (Mathf.Deg2Rad * (transform.rotation.eulerAngles.x - (distanceInDegrees / 2.0f))) % distanceRad;
            float sinDist = Mathf.Sin(transform.rotation.x * Mathf.Deg2Rad) - Mathf.Sin(angleSegment);
            float roundedSpeed = Mathf.Round(100.0f * (Mathf.Asin(sinDist)));
            roundedSpeed = roundedSpeed / 100.0f;
            roundedSpeed *= Mathf.Rad2Deg;
            if (Mathf.Abs(roundedSpeed) > 0.0f)
            {
                speed += snapForce / roundedSpeed * Time.deltaTime;
            }
            else
            {
                speed *= 1 / snapForce;
            }
            speed *= speedDampening;
            speed *= 100.0f;
            speed = Mathf.Round(speed);
            speed /= 100.0f;*/
		}

		if (wheelRotation > 360)
		{
			wheelRotation -= 360;
		} else if (wheelRotation < 0)
		{
			wheelRotation += 360;
		}
		Debug.Log(wheelRotation);
	}


	private void Touchinput ()
	{
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastTouchPos = Input.mousePosition;
            }
            else
            {
                userSpeed = (Input.mousePosition.y - lastTouchPos.y) / Screen.height;
                lastTouchPos = Input.mousePosition;
            }
        }
        else
        {
            userSpeed = 0;
        }
#elif UNITY_ANDROID

        if (Input.touchCount == 1)
		{
			if (Input.GetTouch(0).phase == TouchPhase.Began)
			{
				lastTouchPos = Input.GetTouch(0).position;
			} else
			{
				userSpeed = (Input.GetTouch(0).position.y - lastTouchPos.y) / Screen.height;
				lastTouchPos = Input.GetTouch(0).position;
			}
		} else
		{
			userSpeed = 0;
		}
#endif
    }

}
