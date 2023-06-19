using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
	public float sensitivity;
	public float minSensitivity;
	public float maxSensitivity;
	//public float rotationSensitivity;
	public float scroll;
	
	Vector3 startPos;
	Vector2 mouseStart;
	
	bool shaking;

	[SerializeField] private Transform pivot;
	
	void Update(){
		if(shaking)
			return;
		
		Movement();
		Rotation();
	}

	private void Movement()
	{
		Vector2 mousePos = Input.mousePosition;
		
		float moveSensitivity = -Mathf.Clamp(sensitivity * transform.position.y, minSensitivity, maxSensitivity);
		
		if(Input.GetMouseButtonDown(2)){
			startPos = transform.position;
			mouseStart = mousePos;
		}
		else if(Input.GetMouseButton(2)){
			Vector2 difference = mousePos - mouseStart;
			
			Vector3 pos = startPos;
			
			pos += difference.x * moveSensitivity * transform.right;
			
			Vector3 forward = transform.forward;
			forward.y = 0;
			pos += difference.y * moveSensitivity * forward;
			
			transform.position = pos;
		}
		
		transform.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel") * scroll);
	}

	private void Rotation()
	{
		if (Input.GetMouseButton(1))
		{
			float mouseXDelta = Input.GetAxisRaw("Mouse X");
			pivot.Rotate(Vector3.up * mouseXDelta);
		}
	}
	
	
	public void StartShake(){
		StartCoroutine(Shake(0.1f, 0.1f));
	}
	
	IEnumerator Shake(float duration, float amount){
		shaking = true;
		
		float elapsed = 0f;
		
		Vector3 pos = transform.position;
		
		while(elapsed < duration){
			float x = Random.Range(-1f, 1f) * amount;
			float y = Random.Range(-1f, 1f) * amount;
			float z = Random.Range(-1f, 1f) * amount;
			
			transform.position += new Vector3(x, y, z);
			
			elapsed += Time.deltaTime;
			
			yield return 0;
		}
		
		transform.position = pos;
		
		shaking = false;
	}
}
