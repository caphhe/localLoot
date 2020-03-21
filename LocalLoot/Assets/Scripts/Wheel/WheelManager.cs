using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelManager : MonoBehaviour
{
	[SerializeField] private GameObject boxPrefab = null;
	[SerializeField] private int visibleBoxes = 5;
	[SerializeField] private float distanceInDegrees = 5f;
	[SerializeField] private float radius = 5f;


	[SerializeField] private float speed = 1f;
	private List<GameObject> boxes = new List<GameObject>();
	
	// Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < visibleBoxes; i++)
		{
			AddBox();
		}
    }

    // Update is called once per frame
    void Update()
    {
		gameObject.transform.Rotate(Vector3.right, speed);
		if (Mathf.Abs(transform.rotation.eulerAngles.x % distanceInDegrees) < Mathf.Abs(speed))
		{
			Debug.Log("swap");

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
}
