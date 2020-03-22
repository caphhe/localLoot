using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Coupon : MonoBehaviour, ISceneUpdatable
{
	[SerializeField] private TextMeshPro couponNameText = null;
	[SerializeField] private TextMeshPro shardText = null;
	[SerializeField] private GameObject couponLogoObj = null;
	[SerializeField] private bool isSelectedCoupon = false;
	[SerializeField] private Vector2 shardRange = new Vector2(10, 20);

	[SerializeField] private DataManager dataManager;

	public List<string> targetStateNames => _targetStateNames;
	[SerializeField] List<string> _targetStateNames;

	public void OnEnter()
	{
		Setup();
	}

	public void OnInit(string stateName)
	{
	}

	public void OnUpdate()
	{
	}

	private void Setup()
	{
		if (isSelectedCoupon)
		{
			
			dataManager = Object.FindObjectOfType<DataManager>();
			if (dataManager.selectedCompany == null)
			{
				return;
			}
			couponNameText.text = dataManager.selectedCompany.companyName + ": \n" + dataManager.selectedCompany.vouchers[dataManager.selectedVoucher].name;
			Material logoMat = couponLogoObj.GetComponent<Renderer>().material;
			logoMat.SetTexture("_BaseMap", dataManager.selectedCompany.companyLogo);
			shardText.gameObject.SetActive(false);
			couponNameText.gameObject.SetActive(true);
			couponLogoObj.SetActive(true);
		}
		else
		{
			couponNameText.text = (int)Random.Range(shardRange.x, shardRange.y) + " x Splitter";
			couponLogoObj.SetActive(false);
			shardText.gameObject.SetActive(true);
			couponNameText.gameObject.SetActive(false);
		}
	}
}
