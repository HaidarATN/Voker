using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    Rigidbody rb;

    Transform player;

    float rotX, rotY;

    public float movementSpeed;
    public float sensX = 10f;
    public float sensY = 10f;
    public float maxVertAngle = 45f;
    public float minVertAngle = -45f;
    public GameObject MainCamera;
    public Animator playerAnimator;
    // Start is called before the first frame update
    void Start()
    {
        player = this.transform.GetComponent<Transform>();
        rb = this.transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementControl();
        MouseLookControl();
    }

    void MovementControl()
    {
        float forwardMovement = Input.GetAxis("Vertical");
        float sideMovement = Input.GetAxis("Horizontal");

        Vector3 dirForward = MainCamera.transform.forward * forwardMovement;
        dirForward = dirForward.normalized * movementSpeed * Time.deltaTime;
        dirForward.y = 0;

        Vector3 dirSide = MainCamera.transform.right * sideMovement;
        dirSide = dirSide.normalized * movementSpeed * Time.deltaTime;
        dirSide.y = 0;

        rb.MovePosition(player.position + dirSide + dirForward);

        playerAnimator.SetLayerWeight(1, Mathf.Lerp(0,0.3f, Mathf.Abs((forwardMovement + sideMovement)))); //Mathf.Clamp(Mathf.Abs((forwardMovement + sideMovement)),0,0.1f)
        //print(Mathf.InverseLerp(0, 0.1f, Mathf.Abs((forwardMovement + sideMovement))));
    }

    void MouseLookControl()
    {
        this.transform.Rotate(0, Input.GetAxis("Mouse X") * sensX, 0);

        rotX -= Input.GetAxis("Mouse Y") * sensY;
        rotX = Mathf.Clamp(rotX, minVertAngle, maxVertAngle);

        float rotY = this.transform.localEulerAngles.y;

        transform.localEulerAngles = new Vector3(rotX, rotY, 0);
    }
}
