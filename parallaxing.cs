using UnityEngine;
using System.Collections;

public class parallaxing : MonoBehaviour {

	public Transform[] preSelectedBackgrounds;
	private ArrayList backgrounds = new ArrayList;
	private float[] parallaxScales;
	public float smoothing = 1;

	private Transform cam;
	private Vector3 previous_cam_pos;

	// called before start and after all game objects are started
	// great for references
	void Awake () {
		cam = Camera.main.transform;
	}

	// Use this for initialization
	void Start () {
		previous_cam_pos = cam.position;

		parallaxScales = new float[backgrounds.Length];

		for (int i = 0; i < backgrounds.Length; i++) {
			parallaxScales[i] = backgrounds[i].position.z * -1;
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < backgrounds.Length; i++) {
			float parallax = (previous_cam_pos.x - cam.position.x) * parallaxScales[i];

			float backgoundTargetPositionX = backgrounds[i].position.x + parallax;

			Vector3 backgroundTargetPos = new Vector3 (backgoundTargetPositionX, backgrounds[i].position.y, backgrounds[i].position.z);

			backgrounds[i].position = Vector3.Lerp (backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);

		}

		previous_cam_pos = cam.position;
	}
}
