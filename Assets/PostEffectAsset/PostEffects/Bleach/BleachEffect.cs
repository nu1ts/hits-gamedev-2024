﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleachEffect : MonoBehaviour {

	public bool on = false;

	[Range(0f, 2f)]
	public float strength = 1f;

	public Material material;


	void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		if(on)
		{
			Graphics.Blit(src, dest, material);
		}
		else
		{
			Graphics.Blit(src, dest);
		}
	}


	void Update () {
		if(on)
		{
			material.SetFloat("_Strength", strength);
		}
	}
}
