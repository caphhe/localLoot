﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
	private CompanyScriptable[] _companies;
	public CompanyScriptable[] companies { get { return _companies; } }

	public CompanyScriptable _selectedCompany;
	public CompanyScriptable selectedCompany { get { return _selectedCompany; } }

	public int _selectedVoucher;
	public int selectedVoucher { get { return _selectedVoucher; } }

	// Start is called before the first frame update
	void Awake()
    {
		LoadData();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void LoadData()
	{
		_companies = Resources.LoadAll<CompanyScriptable>("Data");
	}

	public void SetSelectedCompany (CompanyScriptable selected)
	{
		_selectedCompany = selected;
	}

	public void SetSelectedVoucher(int selected)
	{
		_selectedVoucher = selected;
	}
}
