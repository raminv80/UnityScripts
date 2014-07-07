using UnityEngine;
using System.Collections;

public class parallaxTiling : MonoBehaviour {

	public int offset = 1;
	public bool reverseTiling = false;
	public float smoothing = 1;

	private Transform cam;
	private Vector3 previous_cam_pos;
	public bool hasLeftTile = false;
	public bool hasRightTile = false;
	private float spriteWidth;
	private const int AtLeft = -1;
	private const int AtRight = 1;
	private Transform myTransform;
	private parallaxing prlx;
	private int parallaxBackgroundIndex;
	private float parallaxScale;

	void Awake () {
		Camera.main.orthographic = true;
		cam = Camera.main.transform;
	}

	// Use this for initialization
	void Start () {
		var renderer = gameObject.GetComponent<Renderer>();
		spriteWidth = renderer.bounds.size.x;
		previous_cam_pos = cam.position;
		parallaxScale = transform.position.z * -1;
	}
	
	// Update is called once per frame
	void Update () {
		if(!hasLeftTile || !hasRightTile){
			float tileEdgeLeft = transform.position.x - spriteWidth/2;
			float tileEdgeRight = transform.position.x + spriteWidth/2;
			float camHorizontalExtend = Camera.main.orthographicSize * Screen.width/Screen.height;
			float cameraEdgeLeft = cam.position.x - camHorizontalExtend;
			float cameraEdgeRight = cam.position.x + camHorizontalExtend;

			if(cameraEdgeRight + offset >= tileEdgeRight && !hasRightTile){
				//instantinate right tile
				CreateTile(AtRight);
				hasRightTile = true;
			}

			if(cameraEdgeLeft - offset <= tileEdgeLeft && !hasLeftTile){
				//instantinate left tile
				CreateTile(AtLeft);
				hasLeftTile = true;
			}
		}

		float parallax = (previous_cam_pos.x - cam.position.x) * parallaxScale;
		float tileTargetPositionX = transform.position.x + parallax;
		Vector3 tileTargetPos = new Vector3 (tileTargetPositionX, transform.position.y, transform.position.z);
		transform.position = Vector3.Lerp (transform.position, tileTargetPos, smoothing * Time.deltaTime);
		previous_cam_pos = cam.position;
	}

	void CreateTile(int dir) {
		Vector3 tilePosition = new Vector3(transform.position.x + spriteWidth * dir, transform.position.y, transform.position.z);
		Transform newTile = Instantiate(transform, tilePosition, transform.rotation) as Transform;

		newTile.parent = transform.parent;

		if(reverseTiling){
			newTile.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
		}

		var tilingComponent = newTile.GetComponent<parallaxTiling>();

		tilingComponent.offset = offset;
		tilingComponent.smoothing = smoothing;
		tilingComponent.reverseTiling = reverseTiling;

		if(dir == AtLeft){
			tilingComponent.hasRightTile = true;
		}else{
			tilingComponent.hasLeftTile = true;
		}
	}
}
