using UnityEngine;
using System.Collections;

public class ProjectileDragging : MonoBehaviour
{

    public float maxStretch = 3.0f;
    public LineRenderer catapultLine;

    private DigController dc;
    private SpringJoint2D spring;
    private Transform catapult;
    private Ray rayToMouse;
    private Ray catapultToProjectile;
    private float maxStretchSqr;
    private bool clickedOn;
    public bool firstTime = true;
    public bool firstClick = true;
    public Vector2 prevVelocity;
    public bool ableToMove;
    public bool collidingInDrag;
    public bool freeFly;
    public bool firstUpdate;


    void Awake()
    {
        spring = GetComponent<SpringJoint2D>();
        catapult = spring.connectedBody.transform;
    }

    void Start()
    {
        //LineRendererSetup();
        freeFly = false;
        rayToMouse = new Ray(catapult.position, Vector3.zero);
        catapultToProjectile = new Ray(catapultLine.transform.position, Vector3.zero);
        maxStretchSqr = maxStretch * maxStretch;
        GetComponent<Rigidbody2D>().isKinematic = true;
        dc = GetComponent<DigController>();
        ableToMove = true;
    }

    void Update()
    {
        if (clickedOn)
            Dragging();

        if (ableToMove)
        {

            if (!GetComponent<Rigidbody2D>().isKinematic && prevVelocity.sqrMagnitude > GetComponent<Rigidbody2D>().velocity.sqrMagnitude )
            {
                //spring.enabled = false;
                Destroy(spring);
                dc.enabled = true;
                dc.firstCol = true;
                //catapultLine.transform.position = GetComponent<Rigidbody2D>().position;
                //catapultLine.SetPosition (1, catapultLine.transform.position);
                ableToMove = false;
                firstClick = true;
                freeFly = true;
                //catapultLine.enabled = false;
                GetComponent<Rigidbody2D>().velocity = prevVelocity;
                print("released");
            }

            if (!clickedOn )
            {
                prevVelocity = GetComponent<Rigidbody2D>().velocity;
            }

            //LineRendererUpdate();
        }
        else
        {
            //catapultLine.enabled = false;
        }
        firstUpdate = false;
    }


    void LineRendererSetup()
    {
        catapultLine.SetPosition(0, catapultLine.transform.position);
        catapultLine.sortingLayerName = "Foreground";
        catapultLine.sortingOrder = 1;
    }

    void OnMouseDown()
    {
        //spring.enabled = false;
        clickedOn = true;
        prevVelocity = Vector2.zero;

        //catapultLine.transform.position = GetComponent<Rigidbody2D>().position;
        //catapultLine.SetPosition(0, transform.position);
        //catapultLine.SetPosition(1, transform.position);
        //rayToMouse = new Ray(catapult.position, Vector3.zero);
        //catapultToProjectile = new Ray(catapultLine.transform.position, Vector3.zero);
        //prevVelocity = GetComponent<Rigidbody2D>().velocity;
        dc.enabled = false;

        GetComponent<CircleCollider2D>().enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Shooting");
        if (!firstTime && firstClick)
        {
            print("new spring");
            //catapult.GetComponent<Rigidbody2D>().position = transform.position;

            gameObject.AddComponent<SpringJoint2D>();
            spring = GetComponent<SpringJoint2D>();
            spring.connectedBody = catapult.GetComponent<Rigidbody2D>();
            spring.distance = 1;
            spring.dampingRatio = 0;
            spring.frequency = 2;
            spring.anchor = new Vector2(0, 0);
            spring.connectedAnchor = new Vector2(0, 0);
            spring.autoConfigureDistance = false;
        }
        firstClick = false;
    }

    public void resetVars()
    {
        GetComponent<Rigidbody2D>().isKinematic = true;
        catapultLine.enabled = true;
        catapult.transform.position = GetComponent<Rigidbody2D>().position + new Vector2(0f,0.5f);

        ableToMove = true;
        freeFly = false;
    }
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    //collidingInDrag = false;  
    //}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (clickedOn)
            return;
        if (dc.currentDigPower > 0)
            return;
        //GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        
        //catapult.GetComponent<Transform>().position = transform.position;
        //catapultLine.transform.position = transform.position;

        //catapultLine.SetPosition(0, transform.position);

        //catapultLine.SetPosition(1, transform.position);

    }

    void OnMouseUp()
    {
        ableToMove = true;
        firstTime = false;
        spring.enabled = true;
        GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<CircleCollider2D>().enabled = true;
        //GetComponent<Rigidbody2D>().position = transform.position;
        firstUpdate = true;
        //prevVelocity = Vector2.zero;

        dc.currentDigPower = dc.maxDigPower;
            clickedOn = false;
        print("btwn " + (transform.position - catapult.position));
    }

    void Dragging()
    {
        Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
        Vector2 catapultToMouse = mouseWorldPoint - catapult.position;
        if (catapultToMouse.sqrMagnitude > maxStretchSqr)
        {
            rayToMouse.direction = catapultToMouse;
            mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
        }

        mouseWorldPoint.z = 0f;
        transform.position = mouseWorldPoint;
    }

    void LineRendererUpdate()
    {
        Vector2 catapultToProjectileVec = transform.position - catapultLine.transform.position;
        //print(catapultLine.GetPosition(0) + " <line player> " + transform.position);
        //print(catapultToProjectileVec);
        catapultToProjectile.direction = catapultToProjectileVec;
        Vector3 holdPoint = catapultToProjectile.GetPoint(catapultToProjectileVec.magnitude);
        catapultLine.SetPosition(1, holdPoint);
    }

}