using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Title : MonoBehaviour
{
	public GUIContoller Controls { get; private set; }

	void Awake()
	{
		Debug.Log("awake");
		Controls = new GUIContoller();

		Controls.Title.Enter.performed += MainSceneTransfer;
	}

	// Use this for initialization
	void Start()
	{

	}

	void MainSceneTransfer(InputAction.CallbackContext context)
	{
		SceneManager.LoadScene("scene_Main");
	}

	// Update is called once per frame
	void Update()
	{
	}

	private void OnEnable() => Controls.Enable();
	private void OnDisable() => Controls.Disable();
}
