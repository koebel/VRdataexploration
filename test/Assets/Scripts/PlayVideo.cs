using UnityEngine;
using System.Collections;

public class PlayVideo : MonoBehaviour {

	Renderer r;
	MovieTexture movieTexture;

	// Use this for initialization
	void Awake () {

		r = GetComponent<Renderer>();
		movieTexture = (MovieTexture)r.material.mainTexture;
		movieTexture.Play();
		movieTexture.loop = true;
	}﻿
}
