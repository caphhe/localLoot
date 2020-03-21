using UnityEngine;

using System.Collections.Generic;

[CreateAssetMenu(fileName = "Company", menuName = "ScriptableObjects/company", order = 1)]
public class CompanyScriptable : ScriptableObject
{
	public string companyName;
	public Texture companyLogo;
	public List<Voucher> vouchers;
}

public struct Voucher
{
	public string name;
	public float value;
}
