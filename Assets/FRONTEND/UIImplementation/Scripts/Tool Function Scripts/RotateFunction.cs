using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RotateFunction : MonoBehaviour
{
	IMovable movableObject;
	IMovable tempObject;
	Rigidbody rb;
	[SerializeField]Quaternion startRot, endRot;    
	string ID;
	bool isSelected = false;
	Camera mainCam;
	float targetRot;   

	public float force = 0.2f;
	public float maxSpeed = 360f;

	GameObject confirmHolder, denyHolder;
	Button confirmButton, denyButton;

	List<Tuple<string, Quaternion>> rotationHistory;   //stores id of the object moved and the net translation in world space

	bool enableMotion = true;
	public bool isCursorOverButton = false;

	float lastMousePos, newMousePos, mouseVel;



	private void Start()
	{
		lastMousePos = 10000;

		rotationHistory = new List<Tuple<string, Quaternion>>();
		mainCam = Camera.main;

		confirmHolder = new GameObject();
		denyHolder = new GameObject();

		SetUpButton(confirmHolder);
		SetUpButton(denyHolder);

		confirmButton = confirmHolder.AddComponent<Button>();
		denyButton = denyHolder.AddComponent<Button>();

		confirmButton.onClick.AddListener(ConfirmPlacement);
		denyButton.onClick.AddListener(DenyPlacement);

		confirmHolder.GetComponent<Image>().color = Color.green;
		denyHolder.GetComponent<Image>().color = Color.red;
	}



	private void Update()
	{
		


		if (Input.GetMouseButtonDown(0))
		{
			isCursorOverButton = EventSystem.current.IsPointerOverGameObject();			

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Get user input based on click from camera in game view
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit) && !isCursorOverButton) //if we have clicked on an object
			{
				tempObject = hit.collider.gameObject.GetComponentInParent<IMovable>();
				if (!isSelected & tempObject != null)
				{
					movableObject = tempObject;
					startRot = movableObject.rot;
					ID = movableObject.ID;
					rb = movableObject.rigidBody;
					rb.isKinematic = false;
					rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
					rb.angularDrag = 10;
					isSelected = true;
					Debug.Log("initalised");
				}
			}
			else
			{
				tempObject = null;
			}
		}


		if (tempObject != null && isSelected)         //If the selected object can be moved
		{
			if (Input.GetMouseButton(0) && movableObject.ID == tempObject.ID)      //If we are holding the mouse down and have clicked on the current object
			{			
				confirmHolder.SetActive(false);
				denyHolder.SetActive(false);
				targetRot =  Input.mousePosition.x - mainCam.WorldToScreenPoint(movableObject.pos).x;
				enableMotion = true;
				Debug.Log("moving");
               
			}

			if (Input.GetMouseButtonUp(0))
			{
				endRot = movableObject.rot;
				confirmHolder.SetActive(true);
				denyHolder.SetActive(true);
				enableMotion = false;
				lastMousePos = 10000;
			};
		}

		if (isSelected)  //constantly updating this position for now
		{
			confirmButton.transform.position = mainCam.WorldToScreenPoint(movableObject.pos) + new Vector3(50, 0, 0);
			denyButton.transform.position = mainCam.WorldToScreenPoint(movableObject.pos) - new Vector3(50, 0, 0);
			GetMouseVelocity();
		}
	}



	private void FixedUpdate()
	{
		if (isSelected)
		{
			if (enableMotion) //This was always on before if we had an object selected, what was changing was targetrotation
			{
				rb.AddTorque(0, mouseVel * force * Time.deltaTime, 0);
			}
			else
			{
				rb.angularVelocity = Vector3.zero;
			}
		}
	}


















	void RecordRotation()
	{
		Quaternion netRotation = endRot * Quaternion.Inverse(startRot); // check sign of this
		rotationHistory.Add(new Tuple<string, Quaternion>(ID, netRotation));
	}

	void LoadLastMove()
	{
		Tuple<string, Quaternion> entry = rotationHistory[-1];
		IMovable[] movables = (IMovable[])FindObjectsOfType<GameObject>().OfType<IMovable>(); //need to check this works
		foreach (var item in movables)
		{
			if (item.ID == entry.Item1)
			{
				item.rot = entry.Item2;
				break;
			}
		}
	}

	void SetUpButton(GameObject rootObject)
	{
		rootObject.transform.parent = FindObjectOfType<Canvas>().transform;
		rootObject.AddComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
		rootObject.AddComponent<Image>();
		rootObject.SetActive(false);
	}

	void ConfirmPlacement()
	{
		RecordRotation();
		isSelected = false;
		confirmHolder.SetActive(false);
		denyHolder.SetActive(false);
		movableObject.rigidBody.isKinematic = true;
		rb.constraints = RigidbodyConstraints.None;
	}

	void DenyPlacement()
	{
		movableObject.rot = startRot;
		isSelected = false;
		confirmHolder.SetActive(false);
		denyHolder.SetActive(false);
		movableObject.rigidBody.isKinematic = true;
		rb.constraints = RigidbodyConstraints.None;
	}

	void GetMouseVelocity()
    {
		newMousePos = Input.mousePosition.x;
		if (lastMousePos == 10000)
        {
			mouseVel = 0;
			lastMousePos = newMousePos;			
		}
        else
        {
			mouseVel = (newMousePos - lastMousePos) / Time.deltaTime;
			lastMousePos = newMousePos;
		}
		
    }
}
