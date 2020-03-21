using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelManager : MonoBehaviour
{
	[SerializeField] private GameObject boxPrefab = null;
	[SerializeField] private int visibleBoxes = 5;
	[SerializeField] private float distanceInDegrees = 5f;
	[SerializeField] private float radius = 5f;
	[SerializeField] private float speedDampening = 0.9f;
	[SerializeField] private float userSpeedMod = 0.25f;
	[SerializeField] private float snapForce = 1f;



	[SerializeField] private float speed = 1f;
	private float userSpeed = 0;
	private List<GameObject> boxes = new List<GameObject>();
	private Rigidbody rb; 

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

		rb.centerOfMass = gameObject.transform.position;
	}

    // Update is called once per frame
    void Update()
    {
		Touchinput();
		CalcSpeed();
		//rb.AddTorque(new Vector3(userSpeed *100, 0, 0), ForceMode.Acceleration);
		gameObject.transform.Rotate(Vector3.right, speed );
		if (Mathf.Abs(transform.rotation.eulerAngles.x % distanceInDegrees) < Mathf.Abs(speed))
		{
			SwapBoxPosition(speed > 0);
		}
    }

	private void AddBox ()
	{
		var box = Instantiate(boxPrefab, transform);
		boxes.Add(box);
		box.transform.position = GetPostitonForBox(boxes.Count - 1);
		box.GetComponent<WheelObject>().ReSkin();
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
		movedBox.GetComponent<WheelObject>().ReSkin();
	} 

	private Vector3 GetPostitonForBox (int index)
	{
		float angle = index * distanceInDegrees - 2 * distanceInDegrees;
		return gameObject.transform.position - (Quaternion.Euler(angle, 0, 0) * new Vector3(0, 0, radius));
	}

	private void CalcSpeed ()
	{
		if (Input.GetMouseButton(0))
		{
			speed = userSpeed * userSpeedMod;
		} else
		{
			Debug.Log(transform.rotation.eulerAngles.x);
			float distFromRestPos = (transform.rotation.eulerAngles.x - distanceInDegrees/2) % distanceInDegrees - distanceInDegrees / 2;
			float distFromRestPercent = distFromRestPos / (distanceInDegrees / 2);

			speed += distFromRestPercent * snapForce * Time.deltaTime; 

			speed *= speedDampening;

			if (Mathf.Abs(distFromRestPos) < 0.1f && Mathf.Abs(speed) < 0.01f)
			{
				speed = 0;
			}
		}
	}


	private void Touchinput ()
	{
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
#endif
	}
}
