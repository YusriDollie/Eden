using UnityEngine;
using System.Collections;

public class VR_movement : MonoBehaviour {
	public GameObject trackingPoint;

	public float speed = 1;
    public float maxSpeed = 1;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

    public float innerDiam = 0.5f;
	public float outerDiam = 1f;

	private Vector3 movement;
	private float speedConversionDivider;
	private float curSpeed = 0;

	public float dashDist = 1f;
	public float dashDuration = 1f;
	private Vector3 startPos;
	private Vector3 dashDirection;
	public bool dash = false;
	private float dashTimeStart;

	public TrailRenderer dashTail;


	// Use this for initialization
	void Start () {
		movement = new Vector2 (trackingPoint.transform.localPosition.x, trackingPoint.transform.localPosition.z);
		speedConversionDivider = outerDiam - innerDiam;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawLine (transform.position, transform.position+(Vector3.forward*innerDiam), Color.white);
		Debug.DrawLine (transform.position, transform.position-(Vector3.forward*outerDiam), Color.red);

		if (Input.GetKeyDown (KeyCode.B)) {
			Debug.Log ("Dash?");
			startPos = transform.position;
			dashDirection = movement.normalized;
			Vector3 temp = dashDirection * dashDist;
			Debug.Log (temp);
			transform.Translate (new Vector3(temp.x,0,temp.y));
		}
	}

	void FixedUpdate(){
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded)
        {
            movement = new Vector3(trackingPoint.transform.localPosition.x,0, trackingPoint.transform.localPosition.z);
            //Debug.Log (movement);
            if (movement.magnitude > innerDiam)
            {
                if (movement.magnitude < outerDiam)
                {
                    curSpeed = ((movement.magnitude - innerDiam) / speedConversionDivider) * maxSpeed;
                }
            }
            else
            {
                curSpeed = 0;
            }
            movement = movement.normalized * curSpeed;
        }
        //Applying gravity to the controller
        movement.y -= gravity * Time.deltaTime;
        //Making the character move
        controller.Move(movement * Time.deltaTime);
               
        //transform.Translate (new Vector3 (movement.x, 0, movement.y));
	}

}
