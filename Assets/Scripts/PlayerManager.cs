using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    Rigidbody rb, ropeRb;

    [Header("Character Settings")]
    public float speed = 500;

    public float gravity = -32;

    public float jumpForce = 11;

    public float dashForce = 2500;
    public float swingForce = 15;
    public static bool invertedAxis;
    public Transform mainCamera;
    public StaminaManager staminaManager;
    [Header("Wallrun Settings")]
    public float maxWallSpeed; 
    public float wallrunForce;
    public float maxWallRunCameraValue;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 10;

    private float dashTimer = 0.3f, headInclination = 0, bodyInclination = 0, lastTapTime, wallJumpTimer = 0.3f, climbTimer = 0.5f, wallRunCameraValue;

    private const float DASH_TIME = 0.2f;

    Vector3 velocity, dashDirection, spawnPoint, footPosition;

    bool forward, back, left, right, jump, dash, sprint, glide, isRope, isLadder,
        isWallRight, isWallLeft, isWallRunning, canWallRun = true, canClimb = true, wallJump, wallJumpRight, wallJumpLeft;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        spawnPoint = transform.position;
    }

    void Update()
    {
        MyInput();
        Rotation();
    }

    void FixedUpdate()
    {
        Movement();
        footPosition = new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z);
    }

    void MyInput()
    {
        // Respawn test intended
        if(Input.GetKeyDown(KeyCode.R))
        {
            transform.position = spawnPoint;
        }

        // Movement input
        forward = Input.GetKey(KeyCode.W);
        back = Input.GetKey(KeyCode.S);
        left = Input.GetKey(KeyCode.A);
        right = Input.GetKey(KeyCode.D);

        if(canWallRun)
        {
            isWallRight = Physics.Raycast(transform.position, transform.right, 1f, 1 << 11);
            isWallLeft = Physics.Raycast(transform.position, -transform.right, 1f, 1 << 11);
        }

        if (!isWallLeft && !isWallRight) isWallRunning = false;

        // Interaction
        Vector3 armPosition = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
        RaycastHit hitInteraction;
        if (Physics.Raycast(mainCamera.position, transform.TransformDirection(Vector3.forward), out hitInteraction, 2, 1 << 14))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(hitInteraction.collider.gameObject.tag == "Glyph")
                {
                    hitInteraction.collider.GetComponent<InteractionManager>().checkInteractionGlyph();
                }
                else 
                {
                    hitInteraction.collider.GetComponent<InteractionManager>().checkInteractionGlyphUp();
                }
            }
        }

        if (Physics.Raycast(transform.position, Vector3.down, 1.1f))
        {
            staminaManager.canRecoverOn();
            canWallRun = true;
            if (glide)
            {
                glide = false;
                staminaManager.staminaOff();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jump = true;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (staminaManager.stamina >= 1)
                {
                    sprint = true;
                }
                else
                {
                    sprint = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                float timeSinceLastTap = Time.time - lastTapTime;
                if (timeSinceLastTap <= DASH_TIME && !dash)
                {
                    if (staminaManager.stamina >= 25)
                    {
                        dash = true;
                        dashDirection = transform.forward;
                        staminaManager.dashStamina();
                    }
                }
                lastTapTime = Time.time;
            }
        }
        else
        {
            sprint = false;
            staminaManager.canRecoverOff();
            // Check if there is a wall
            if (Input.GetKey(KeyCode.D) && isWallRight)
            {
                if(staminaManager.stamina >= 1)
                {
                    Wallrun();
                    staminaManager.staminaOn();
                }
                else
                {
                    isWallRunning = false;
                    canWallRun = false;
                    isWallLeft = false;
                    isWallRight = false;
                }
            }
            if (Input.GetKeyUp(KeyCode.D)) isWallRunning = false;
            if (Input.GetKey(KeyCode.A) && isWallLeft)
            {
                if(staminaManager.stamina >= 1)
                {
                    Wallrun();
                    staminaManager.staminaOn();
                }
                else
                {
                    isWallRunning = false;
                    canWallRun = false;
                    isWallLeft = false;
                    isWallRight = false;
                }
            }

            if (Input.GetKeyUp(KeyCode.A)) isWallRunning = false;

            if(!isWallRunning) staminaManager.staminaOff();
            if (velocity.y < 0)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    if (staminaManager.stamina >= 1)
                    {
                        glide = true;
                        staminaManager.staminaOn();
                    }
                    else
                    {
                        glide = false;
                        staminaManager.staminaOff();
                    }
                }
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    glide = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (!glide)
                {
                    float timeSinceLastTap = Time.time - lastTapTime;
                    if (timeSinceLastTap <= DASH_TIME && !dash)
                    {
                        if (staminaManager.stamina >= 25)
                        {
                            dash = true;
                            dashDirection = transform.forward;
                            staminaManager.dashStamina();
                        }
                    }
                    lastTapTime = Time.time;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            sprint = false;
            staminaManager.staminaOff();
        }
    }

    void Wallrun()
    {
        isWallRunning = true;
        
        if (rb.velocity.magnitude <= maxWallSpeed)
        {
            rb.AddForce(transform.forward * wallrunForce * Time.deltaTime);

            if (isWallRight) rb.AddForce(transform.right * wallrunForce / 5 * Time.deltaTime);
            else rb.AddForce(-transform.right * wallrunForce / 5 * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isWallRight)
            {
                isWallRight = false;
                canWallRun = false;
                wallJump = true;
                wallJumpLeft = true;
            }
            else if(isWallLeft)
            {
                isWallLeft = false;
                canWallRun = false;
                wallJump = true;
                wallJumpRight = true;
            }
        }
    }

    void Rotation()
    {
        // body rotation
        if(!isRope)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0);
        }

        // head rotation
        if(!isRope)
        {
            if(invertedAxis)
            {
                headInclination = headInclination + Input.GetAxis("Mouse Y") * mouseSensitivity;
            }
            else
            {
                headInclination = headInclination + -Input.GetAxis("Mouse Y") * mouseSensitivity;
            }
            headInclination = Mathf.Clamp(headInclination, -60, 60);
        }
        else
        {
            if(invertedAxis)
            {
                headInclination = headInclination + Input.GetAxis("Mouse Y") * mouseSensitivity;
                bodyInclination = bodyInclination + -Input.GetAxis("Mouse X") * mouseSensitivity;
            }
            else
            {
                headInclination = headInclination + -Input.GetAxis("Mouse Y") * mouseSensitivity;
                bodyInclination = bodyInclination + Input.GetAxis("Mouse X") * mouseSensitivity;
            }
            headInclination = Mathf.Clamp(headInclination, -60, 60);
            bodyInclination = Mathf.Clamp(bodyInclination, -40, 40);
        }

        // wallrun camera
        if (Mathf.Abs(wallRunCameraValue) < maxWallRunCameraValue && isWallRunning && isWallRight)
        {
            wallRunCameraValue += Time.deltaTime * maxWallRunCameraValue * 2;
        }
        if (Mathf.Abs(wallRunCameraValue) < maxWallRunCameraValue && isWallRunning && isWallLeft)
        {
            wallRunCameraValue -= Time.deltaTime * maxWallRunCameraValue * 2;
        }

        if (wallRunCameraValue > 0 && !isWallRight && !isWallLeft)
        {
            wallRunCameraValue -= Time.deltaTime * maxWallRunCameraValue * 2;
        }
        if (wallRunCameraValue < 0 && !isWallRight && !isWallLeft)
        {
            wallRunCameraValue += Time.deltaTime * maxWallRunCameraValue * 2;
        }

        if(!isRope)
        {
            mainCamera.localEulerAngles = new Vector3(headInclination, 0, wallRunCameraValue);
        }
        else
        {
            mainCamera.localEulerAngles = new Vector3(headInclination, bodyInclination, wallRunCameraValue);
        }
    }

    void Movement()
    {
        if(!isRope)
        {
            transform.parent = null;
        }
        if (!isRope && !isLadder)
        {
            RaycastHit hitGround;
            if (Physics.Raycast(transform.position,Vector3.down, out hitGround, 1.1f, 1 << 13))
            {
                transform.position = new Vector3(transform.position.x, hitGround.point.y + 1f, transform.position.z);
                transform.parent = hitGround.transform;
                velocity.y = 0;
            }
            if (Physics.Raycast(transform.position,Vector3.down, out hitGround, 1.1f, 1 << 10))
            {
                transform.position = new Vector3(transform.position.x, hitGround.point.y + 1f, transform.position.z);
                velocity.y = 0;
            }
            else
            {
                if (glide)
                {
                    velocity.y += (gravity * 0.065f) * Time.deltaTime;
                }
                else if (isWallRunning)
                {
                    velocity.y = 0;
                }
                else
                {
                    velocity.y += gravity * Time.deltaTime;
                }
            }
        }

        isLadder = false;
        velocity = new Vector3(0, velocity.y, 0);

        if(!isWallRunning && !isRope)
        {
            if (forward)
            {
                if (sprint)
                {
                    velocity += transform.TransformDirection(Vector3.forward) * (speed * 1.1f) * Time.deltaTime;
                    staminaManager.staminaOn();
                }
                velocity += transform.TransformDirection(Vector3.forward) * speed * Time.deltaTime;
            }

            if (back)
            {
                velocity += transform.TransformDirection(Vector3.back) * speed * Time.deltaTime;
            }

            if (left)
            {
                velocity += transform.TransformDirection(Vector3.left) * speed * Time.deltaTime;
            }

            if (right)
            {
                velocity += transform.TransformDirection(Vector3.right) * speed * Time.deltaTime;
            }
        }

        //Jump
        if (jump)
        {
            velocity.y = jumpForce;
            jump = false;
        }

        //Dash
        if (dash)
        {
            sprint = false;
            velocity += dashDirection * dashForce * Time.deltaTime;
            dashTimer -= Time.deltaTime;
        }

        if (dashTimer <= 0)
        {
            dash = false;
            dashTimer = 0.3f;
        }

        //Walljump
        if (wallJump)
        {
            if(wallJumpRight)
            {
                rb.AddForce(transform.right * wallrunForce * Time.deltaTime);
            }
            else if(wallJumpLeft)
            {
                rb.AddForce(-transform.right * wallrunForce * Time.deltaTime);
            }
            rb.AddForce(Vector2.up * jumpForce * 50f);
            velocity.y = 0;
            wallJumpTimer -= Time.deltaTime;
        }

        if(wallJumpTimer <= 0)
        {
            wallJump = false;
            wallJumpTimer = 0.3f;
            wallJumpLeft = false;
            wallJumpRight = false;
            velocity = Vector3.zero;
        }

        //Rope climbing
        if(!canClimb)
        {
            climbTimer -= Time.deltaTime;
        }

        if(climbTimer <= 0)
        {
            canClimb = true;
            climbTimer = 0.5f;
            rb.detectCollisions = true;
        }

/*         // Ladder interaction
        Vector3 footPosition = new Vector3(transform.position.x, transform.position.y - 0.7f, transform.position.z);

        if (Physics.Raycast(footPosition, transform.forward, 1,1 << 12) &&forward)
        {
            velocity.y = 600 * Time.deltaTime;
            isLadder = true;
        } */

        //Rope interaction
        
        if(canClimb)
        {
            Vector3 armPosition = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
            RaycastHit hitRope;
            
            if(Physics.Raycast(armPosition, transform.forward, out hitRope, 1f, 1 << 8))
            {
                transform.parent = hitRope.transform;
                isRope = true;
                velocity = Vector3.zero;
                rb.isKinematic = true;
                ropeRb = hitRope.rigidbody;
                staminaManager.canRecoverOff();
                if(glide) glide = false;
            }

            if(isRope)
            {
                float ropeVelocity;

                ropeVelocity = ropeRb.velocity.magnitude;
                if(ropeVelocity < 1.5f) ropeVelocity = 1.5f;
                if(ropeVelocity > 4.5f) ropeVelocity = 4.5f;

                if(forward)
                {
                    ropeRb.AddForce(transform.forward * swingForce, ForceMode.Acceleration);
                }

                if(back)
                {
                    ropeRb.AddForce(-transform.forward * swingForce, ForceMode.Acceleration);
                }

                if(Input.GetKeyDown(KeyCode.Space))
                {
                    staminaManager.canRecoverOn();
                    rb.isKinematic = false;
                    transform.parent = null;
                    transform.localEulerAngles = new Vector3(0, mainCamera.eulerAngles.y, 0);
                    isRope = false;
                    canClimb = false;
                    rb.detectCollisions = false;
                    velocity.y = (jumpForce * 0.7f * ropeVelocity) * 0.9f;
                }
            }
        }
        rb.velocity = velocity;
    }

    public void resize()
    {
        transform.localScale = new Vector3(1,0.6f,1);
    }

    public void backToNormal()
    {
        transform.localScale = new Vector3(1,1,1);
    }

    void OnDrawGizmosSelected()
    {
        // Ladder
        /* Gizmos.color = Color.green;
        Vector3 footPosition = new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z);
        Gizmos.DrawRay(footPosition, transform.forward * 1); */

        // On ground
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(footPosition, 0.4f);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, Vector3.down * 1.1f);

        //Rope
        Gizmos.color = Color.red;
        Vector3 armPosition = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
        Gizmos.DrawRay(armPosition, transform.forward * 1f);
    }
}
