using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WheelObject : MonoBehaviour
{
	[SerializeField] private float stateChangeTime = 1f;
	[SerializeField] private AnimationCurve flipCurve = null;
	[SerializeField] private TextMeshPro companyNameText = null;
	[SerializeField] private TextMeshPro voucherNameText = null;
	[SerializeField] private GameObject voucherNameObj = null;
	[SerializeField] private GameObject companyLogoObj = null;
	[SerializeField] private GameObject boxObj = null;
	[SerializeField] private AnimationCurve wiggleWhenSelectingCurve = null;
	[SerializeField] private float wiggleAngle = 10f;
	[SerializeField] private AnimationCurve removeScaleCurve = null;
	[SerializeField] private float removeTimer = 1f;
	[SerializeField] private Vector3 dropTorque = new Vector3(0,0,0);

	private bool companyState = true;
	private float stateTransitionTimer = 1;
	private float removeT = -1;

	private CompanyScriptable _company;
	public CompanyScriptable company { get { return _company; } }

	private int _voucherID;
	public int voucherID { get { return _voucherID; } }

	// Start is called before the first frame update
	void Start()
    {
		//DropBox();
    }

    // Update is called once per frame
    void Update()
    {
		if (stateTransitionTimer >= stateChangeTime)
		{
			if (companyState)
			{
				transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
			} else
			{
				transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 90, 0);
			}
		} else
		{
			stateTransitionTimer += Time.deltaTime;
			float angle = 0;
			if (companyState)
			{
				angle = LerpOvershoot(90, 0, flipCurve.Evaluate(stateTransitionTimer / stateChangeTime));
			} else
			{
				angle = LerpOvershoot(0, 90, flipCurve.Evaluate(stateTransitionTimer / stateChangeTime));
			}
			transform.rotation = Quaternion.Euler(0, angle, 0);
		}

		if (removeT != -1)
		{
			removeT += Time.deltaTime;
			float scale = removeScaleCurve.Evaluate(removeT / removeTimer);
			transform.localScale = new Vector3(scale, scale, scale);
		}
    }

	public void ReSkinCompany (CompanyScriptable company)
	{
		this._company = company;
		Material boxMat = boxObj.GetComponent<Renderer>().material;
		boxMat.SetColor("_BaseColor", company.companyColor);
		Material companyLogoMat = companyLogoObj.GetComponent<Renderer>().material;
		companyLogoMat.SetTexture("_BaseMap", company.companyLogo);
		companyNameText.text = company.companyName;

		companyNameText.gameObject.SetActive(true);
		companyLogoObj.SetActive(true);
		voucherNameObj.SetActive(false);

	
	}

	public void ReSkinVoucher (CompanyScriptable company, int voucherID)
	{
		this._company = company;
		this._voucherID = voucherID;
		Material boxMat = boxObj.GetComponent<Renderer>().material;
		boxMat.SetColor("_BaseColor", company.companyColor);
		voucherNameText.text = company.vouchers[voucherID].name;
		companyNameText.gameObject.SetActive(false);
		companyLogoObj.SetActive(false);
		voucherNameObj.SetActive(true);
	}

	public void StateChanged (bool companyState)
	{
		this.companyState = companyState;
		stateTransitionTimer = 0;
	}

	public void SetState (bool companyState)
	{
		this.companyState = companyState;
		if (companyState)
		{
			transform.rotation = Quaternion.Euler(0, 0, 0);
		} else
		{
			transform.rotation = Quaternion.Euler(0, 90, 0);
		}
	}

	public static float LerpOvershoot(float a, float b, float t)
	{
		return t * b + (1 - t) * a;
	}

	public void WiggleWhenSelecting (float t)
	{
		if (companyState)
		{
			transform.rotation = Quaternion.Euler(0, 0, wiggleWhenSelectingCurve.Evaluate(t) * wiggleAngle);
		} else
		{
			Debug.Log("Voucher wiggle");
			transform.rotation = Quaternion.Euler(wiggleWhenSelectingCurve.Evaluate(t) * wiggleAngle, 90, 0);
		}
	}

	public void EndWiggle()
	{
		Debug.Log("Endwiggle");
		transform.rotation = Quaternion.Euler(0, 0, 0);
        
    }

	public void RemoveBox()
	{
		Destroy(gameObject, removeTimer);
		removeT = 0;
	}

	public void DropBoxWhenSelected ()
	{
		Debug.Log("Drop");
		Invoke("DropBox", removeTimer);
        LootboxAnchorController ac = FindObjectOfType<LootboxAnchorController>();
        ac.transform.position = transform.position;
        transform.SetParent(FindObjectOfType<LootBoxAnchorTarget>().transform, true);
    }

	private void DropBox()
	{
        LootboxAnchorController ac = FindObjectOfType<LootboxAnchorController>();
        var rb = ac.gameObject.AddComponent<Rigidbody>();
        rb.AddTorque(dropTorque);
    }
}
