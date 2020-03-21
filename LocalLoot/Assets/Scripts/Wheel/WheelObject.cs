using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WheelObject : MonoBehaviour
{
	[SerializeField] private float stateChangeTime = 1f;
	[SerializeField] private AnimationCurve flipCurve = null;
	[SerializeField] private TextMeshPro companyNameText;
	[SerializeField] private TextMeshPro voucherNameText;
	[SerializeField] private AnimationCurve wiggleWhenSelectingCurve = null;
	[SerializeField] private float wiggleAngle = 10f;

	private Material mat; 
	private bool companyState = true;
	private float stateTransitionTimer = 1;
	
	// Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
		if (stateTransitionTimer >= stateChangeTime)
		{
			if (companyState)
			{
				transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
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

		
    }

	public void ReSkin ()
	{
		mat = transform.GetChild(0).GetComponent<Renderer>().material;
		mat.color = new Color(Random.value, Random.value, Random.value);

	
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
		transform.rotation = Quaternion.Euler(0, 0, wiggleWhenSelectingCurve.Evaluate(t) * wiggleAngle);
	}

	public void EndWiggle()
	{
		transform.rotation = Quaternion.Euler(0, 0, 0);
	}
}
