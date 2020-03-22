using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class WheelManager : MonoBehaviour, ISceneUpdatable
{    
	[SerializeField] private GameObject boxPrefab = null;
	[SerializeField] private int visibleBoxes = 5;
	[SerializeField] private float distanceInDegrees = 5f;
	[SerializeField] private float radius = 5f;
    float distanceRad;
    [SerializeField] private float speedDampening = 0.9f;
	[SerializeField] private float userSpeedMod = 0.25f;
	[SerializeField] private float snapForce = 1f;
	[SerializeField] private AnimationCurve snapForceCurve = null;
	[SerializeField] private AnimationCurve dampeningBelow1Curve = null;
	[SerializeField] private Camera cam;
	[SerializeField] private float holdToSelectTime = 1f;
	[SerializeField] private DataManager dataManager;
	[SerializeField] private Animator sceneAnimator = null;

	[SerializeField] private float speed = 1f;
	private float userSpeed = 0;
	private bool lastUserDirectionPositive = true;
	private List<GameObject> boxes = new List<GameObject>();

	private float wheelRotation = 360;

	private Vector2 lastTouchPos = new Vector2 (-1, -1);
	private Vector2 startTouchPos = new Vector2 (-1, -1);
	private bool touchHolding = false;

	private float HoldingStartTime = 0;
	private WheelObject centerBox;

	private bool companyState = true;
	private int lowestVisibleIndex = 0;
	private int highestVisibleIndex = 0;
	private bool stopUpdate = false;

	public List<string> targetStateNames => _targetStateNames;
	[SerializeField] List<string> _targetStateNames;



	public void OnInit(string stateName)
	{
		if (stateName == "SelectingStore")
		{
			CleanUp();
			for (int i = 0; i < visibleBoxes; i++)
			{
				AddBox();
			}
		}

    }

	// Update is called once per frame
	public void OnUpdate()
    {
		if (stopUpdate)
		{
			return;
		}
		Touchinput();
		CalcSpeed ();
		gameObject.transform.rotation = Quaternion.Euler(wheelRotation, 0, 0);

		float moduloPrep = (wheelRotation - distanceInDegrees / 2) % distanceInDegrees;
		if (Mathf.Abs(moduloPrep) < Mathf.Abs(speed))
		{
			SwapBoxPosition(speed >= 0);
		}

    }

	public void OnEnter()
	{

	}

	private void AddBox ()
	{
		var box = GameManager.Instantiate(boxPrefab, transform);
		boxes.Add(box);
		box.transform.position = GetPostitonForBox(boxes.Count - 1, true);

		ReskinToHighestIndex(box.GetComponent<WheelObject>());
	}

	private void SwapBoxPosition (bool topToBottom)
	{
		GameObject movedBox;
		if (topToBottom)
		{
			movedBox = boxes[boxes.Count - 1];
			boxes.RemoveAt(boxes.Count - 1);
			boxes.Insert(0, movedBox);
			movedBox.transform.position = GetPostitonForBox(0, false);
			ReskinToLowestIndex(movedBox.GetComponent<WheelObject>());

		} else
		{
			movedBox = boxes[0];
			boxes.RemoveAt(0);
			boxes.Add(movedBox);
			movedBox.transform.position = GetPostitonForBox(boxes.Count - 1, false);
			ReskinToHighestIndex(movedBox.GetComponent<WheelObject>());
		}
	} 

	private void ReskinToHighestIndex (WheelObject obj)
	{
		int max = dataManager.companies.Length;
		if (!companyState)
		{
			max = dataManager.selectedCompany.vouchers.Count;
		}

		highestVisibleIndex++;

		if (highestVisibleIndex >= max)
		{
			highestVisibleIndex = 0;
		}

		if (companyState)
		{
			obj.ReSkinCompany(dataManager.companies[highestVisibleIndex]);
		}
		else
		{
			obj.ReSkinVoucher(dataManager.selectedCompany, highestVisibleIndex);
		}
	}

	private void ReskinToLowestIndex(WheelObject obj)
	{
		int max = dataManager.companies.Length - 1;
		if (!companyState)
		{
			max = dataManager.selectedCompany.vouchers.Count - 1;
		}

		lowestVisibleIndex--;
		if (lowestVisibleIndex < 0)
		{
			lowestVisibleIndex = max;
		}

		if (companyState)
		{
			obj.ReSkinCompany(dataManager.companies[lowestVisibleIndex]);
		}
		else
		{
			obj.ReSkinVoucher(dataManager.selectedCompany, lowestVisibleIndex);
		}
	}

	private Vector3 GetPostitonForBox (int index, bool firstSpawn)
	{
		float angle = index * distanceInDegrees - (int)(visibleBoxes / 2) * distanceInDegrees;
		if (!firstSpawn)
		{
			angle += distanceInDegrees / 2;
		}
		return gameObject.transform.position - (Quaternion.Euler(angle, 0, 0) * new Vector3(0, 0, radius));
	}

	private void CalcSpeed ()
	{
		float distanceToRest = (wheelRotation - distanceInDegrees/2) % distanceInDegrees - distanceInDegrees / 2;
		float distanceToRestPercent = distanceToRest / (distanceInDegrees / 2);
		//Debug.Log("distance to rest: " + distanceToRest + " | " + distanceToRestPercent);
		if (Input.GetMouseButton(0))
		{
			speed = userSpeed * userSpeedMod;
			wheelRotation += speed;
		}
        else
		{
			speed += (distanceToRestPercent) * snapForce * -1;
			speed *= speedDampening - dampeningBelow1Curve.Evaluate(Mathf.Abs(speed));

			if (Mathf.Abs(speed) < 0.025f && Mathf.Abs(distanceToRestPercent) < 0.005f)
			{
				speed = 0;
			}
			wheelRotation += speed;
		}


		if (wheelRotation > 720)
		{
			wheelRotation -= 360;
		} else if (wheelRotation < 360)
		{
			wheelRotation += 360;
		}
		//Debug.Log(wheelRotation);
	}


	private void Touchinput ()
	{
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
			if (Input.GetMouseButtonDown(0))
            {
                lastTouchPos = Input.mousePosition;
				startTouchPos = Input.mousePosition;
				touchHolding = RaycastToCenterBox(Input.mousePosition);
				
			}
            else
            {
                userSpeed = (Input.mousePosition.y - lastTouchPos.y) / Screen.height;
				lastUserDirectionPositive = lastTouchPos.y > Input.mousePosition.y;
				lastTouchPos = Input.mousePosition;
				HoldTouchPosition(Input.mousePosition);
				
            }
        }
        else
        {
			if (touchHolding)
			{
				touchHolding = false;
				//centerBox?.EndWiggle();
			}
			userSpeed = 0;
		}
#elif UNITY_ANDROID

        if (Input.touchCount == 1)
		{
			if (Input.GetTouch(0).phase == TouchPhase.Began)
			{
				lastTouchPos = Input.GetTouch(0).position;
				startTouchPos = Input.GetTouch(0).position;
			} else
			{
				userSpeed = (Input.GetTouch(0).position.y - lastTouchPos.y) / Screen.height;
				lastTouchPos = Input.GetTouch(0).position;
				HoldTouchPosition(Input.GetTouch(0).position);
			}
		} else
		{
			userSpeed = 0;
		}
#endif
    }

	private void HoldTouchPosition (Vector2 screenPos)
	{
		if (touchHolding && Vector2.Distance(screenPos, startTouchPos) < 40)
		{
			Debug.Log("Holding");
			centerBox.WiggleWhenSelecting((Time.time - HoldingStartTime) / holdToSelectTime);
			if (HoldingStartTime + holdToSelectTime < Time.time)
			{
				Debug.Log("Selected");
				BoxSelected();
				touchHolding = false;
			}
		} else
		{
			if (touchHolding)
			{
				centerBox?.EndWiggle();
			}
			touchHolding = false;
		}
	}

	private bool RaycastToCenterBox (Vector2 screenPos)
	{
		Ray ray = cam.ScreenPointToRay(screenPos);
		RaycastHit hit;
		Debug.Log("Raycast");
		if (Physics.Raycast(ray, out hit))
		{
			centerBox = null;
			float lowestValue = float.MaxValue;
			Debug.Log(hit.transform.gameObject.name);
			foreach (var box in boxes)
			{
				if (Mathf.Abs(box.transform.position.y - gameObject.transform.position.y) < lowestValue)
				{
					centerBox = box.GetComponent<WheelObject>();
					lowestValue = Mathf.Abs(box.transform.position.y - gameObject.transform.position.y);
				}
			}
			if (hit.transform.parent.gameObject == centerBox.gameObject)
			{
				Debug.Log("hit");
				HoldingStartTime = Time.time;
				return true;
			}
		}
		return false;
	}

	private void BoxSelected ()
	{
		if (companyState) {
			companyState = !companyState;
			dataManager.SetSelectedCompany(centerBox.company);
			highestVisibleIndex = 0;
			foreach (var box in boxes)
			{
				box.GetComponent<WheelObject>().StateChanged(companyState);

				ReskinToHighestIndex(box.GetComponent<WheelObject>());
			}
			sceneAnimator?.SetBool("StoreSelected", true);
		} else
		{
			sceneAnimator?.SetBool("CouponSelected", true);
			centerBox?.DropBoxWhenSelected();
			dataManager.SetSelectedVoucher(centerBox.voucherID);
			RemoveAllBoxes();

		}
		
	}

	private void RemoveAllBoxes ()
	{
		foreach (var box in boxes)
		{
			if (box == centerBox.gameObject)
			{
				continue;
			}
			box.GetComponent<WheelObject>().RemoveBox();
		}
		boxes = new List<GameObject>();
		stopUpdate = true;
	}

	private void CleanUp()
	{
		for (int i = transform.childCount - 1; i >= 0; i--)
		{
			Destroy(transform.GetChild(i).gameObject);
		}
		boxes = new List<GameObject>();
		stopUpdate = false;
		companyState = true;
		lowestVisibleIndex = 0;
		highestVisibleIndex = 0;
		touchHolding = false;
		wheelRotation = 360;
		speed = 0;
	}

	private void GoBackToCompany ()
	{
		OnInit("SelectingStore");
	}
}
