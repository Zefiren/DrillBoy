using UnityEngine;
using System.Collections;

public class ProjectileDragging : MonoBehaviour {

	public float maxStretch = 3.0f;
	public LineRenderer catapultLine;
	
	private SpringJoint2D spring;
	private Transform catapult;
	private Ray rayToMouse;
	private Ray catapultToProjectile;
	private float maxStretchSqr;
	private bool clickedOn;
	private Vector2 prevVelocity;
    public bool ableToMove;
    public bool collidingInDrag;

    void Awake () {
		spring = GetComponent <SpringJoint2D> ();
		catapult = spring.connectedBody.transform;
	}
	
	void Start () {
		LineRendererSetup ();
		rayToMouse = new Ray(catapult.position, Vector3.zero);
        catapultToProjectile = new Ray(catapultLine.transform.position, Vector3.zero);
		maxStretchSqr = maxStretch * maxStretch;
        GetComponent<Rigidbody2D>().isKinematic = true;
        ableToMove = true;
    }
	
	void Update () {
		if (clickedOn)
			Dragging ();
		
		if (ableToMove) {
			if (!GetComponent<Rigidbody2D>().isKinematic && prevVelocity.sqrMagnitude > GetComponent<Rigidbody2D>().velocity.sqrMagnitude) {
                spring.enabled = false;
                //Destroy(spring);
                ableToMove = false;
                GetComponent<Rigidbody2D>().velocity = prevVelocity;
			}
			
			if (!clickedOn)
				prevVelocity = GetComponent<Rigidbody2D>().velocity;
			
			LineRendererUpdate ();
		} else {
			catapultLine.enabled = false;
		}
	}
	
	void LineRendererSetup () {
		catapultLine.SetPosition(0, catapultLine.transform.position);
		catapultLine.sortingLayerName = "Foreground";
		catapultLine.sortingOrder = 1;
	}
	
	void OnMouseDown() {
        print("helloooooo?");
		spring.enabled = false;
		clickedOn = true;
        //catapultLine.transform.position = transform.position;
        catapultLine.SetPosition(0, transform.position);
        catapultLine.SetPosition(1, transform.position);

        GetComponent<CapsuleCollider2D>().enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Shooting");
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    //collidingInDrag = false;   
    //}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (clickedOn)
            return;
        print("colllided?");
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().isKinematic = true;
        catapult.GetComponent<Transform>().position = transform.position;
        catapultLine.transform.position = transform.position;

        catapultLine.SetPosition(0, transform.position);

        //catapultLine.SetPosition(1, transform.position);
        ableToMove = true;
        catapultLine.enabled = true;

    }

    void OnMouseUp () {
        spring.enabled = true;
        GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<CapsuleCollider2D>().enabled = true;

        clickedOn = false;
	}
	
	void Dragging () {
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
		Vector2 catapultToMouse = mouseWorldPoint - catapult.position;
		if (catapultToMouse.sqrMagnitude > maxStretchSqr){
			rayToMouse.direction = catapultToMouse;
			mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
		}
		
		mouseWorldPoint.z = 0f;
		transform.position = mouseWorldPoint;
	}
	
	void LineRendererUpdate () {
		Vector2 catapultToProjectileVec = transform.position - catapultLine.transform.position;
        //print(catapultLine.GetPosition(0)+" <line player> "+ transform.position);
        //print(catapultToProjectileVec);
		catapultToProjectile.direction = catapultToProjectileVec;
		Vector3 holdPoint = catapultToProjectile.GetPoint(catapultToProjectileVec.magnitude);
		catapultLine.SetPosition(1, holdPoint);
	}
	
}
	