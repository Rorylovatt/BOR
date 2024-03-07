using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject leftFoot, rightFoot;
    public Rigidbody playerRb;
    public float maxSpeed, maxFootSpeed, footAcceleration, acceleration, decceleration, explosionForce, rotateSpeed, outOfBoundsSpeedMultiplyer;
    private float speed, leftFootSpeed, rightFootSpeed, speedReset, horizontalInput, stepTime, oobSpeed;
    private bool deccelBool, maxSpeedReached, left, right, releaseLeft, releaseRight;
    private Animator animator;
    public Text speedText, perfectText;
    public Slider leftSlider, rightSlider;
    public int perfectCounter;
    // hello
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        oobSpeed = maxSpeed / outOfBoundsSpeedMultiplyer;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Debugtext();
    }
    public void Rotate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        if(horizontalInput > 0.01f || horizontalInput < -0.01f)
        {
           // playerRb.AddForce(this.transform.forward * speed, ForceMode.Acceleration);
            this.gameObject.transform.Rotate(Vector3.up, Time.deltaTime * 100 * rotateSpeed * horizontalInput, Space.World);

        }
    }
    public void Movement()
    {
        Rotate();
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        animator.SetFloat("Speed", speed) ;

        // Left foot
        if (Input.GetAxis("Fire2") == 1 && leftFootSpeed < maxFootSpeed && (left || speed == 0) && !maxSpeedReached)
        {
            animator.SetBool("Trip", false);
            stepTime = 2;
            animator.SetBool("LeftStep", true);
            animator.SetBool("RightStep", false);
            left = true;
            releaseLeft = true;
            deccelBool = false;
            leftFootSpeed += (footAcceleration / 100) * Time.deltaTime;
            FootSpeedMod(2f, false);
        }
        if (Input.GetAxis("Fire2") == 0)
        {
            deccelBool = true;
            leftFootSpeed = 0;
            if(Input.GetAxis("Fire1") == 0)
            {
                stepTime -= 1;
            }
            if(stepTime < 0)
            {
                animator.SetBool("LeftStep", false);
            }
        }
        // Right Foot
        if (Input.GetAxis("Fire1") == 1 && rightFootSpeed < maxFootSpeed && (right || speed == 0) && !maxSpeedReached)
        {
            animator.SetBool("Trip", false);
            stepTime = 2;
            animator.SetBool("RightStep", true);
            animator.SetBool("LeftStep", false);
            right = true;
            releaseRight = true;
            deccelBool = false;
            rightFootSpeed += (footAcceleration / 100) * Time.deltaTime;
            FootSpeedMod(2f, false);
        }
        if (Input.GetAxis("Fire1") == 0)
        {
            deccelBool = true;
            rightFootSpeed = 0;
            if (Input.GetAxis("Fire2") == 0)
            {
                stepTime -= Time.deltaTime;
            }
            if (stepTime < 0)
            {
                animator.SetBool("RightStep", false);
            }
        }
        if (Input.GetAxis("Fire1") == 0 && releaseRight == true)
        {
            right = false;
            left = true;
            releaseRight = false;
            FootSpeedMod(2f, true);
        }
        if (Input.GetAxis("Fire2") == 0 && releaseLeft == true)
        {
            right = true;
            left = false;
            releaseLeft = false;
            FootSpeedMod(2f, true);

        }
        // Speed sorters 
        if (deccelBool && speed > 0)
        {
            speed = speed - (decceleration / 100) * Time.deltaTime;
        }
        if (speed <= 0)
        {
            deccelBool = false;
            speed = 0;
        }
        if(speed > maxSpeed) 
        {
            speed = maxSpeed;
        }
        if(rightFootSpeed >= maxFootSpeed || leftFootSpeed >= maxFootSpeed)
        {
            maxSpeedReached = true;
            animator.SetBool("Trip", true);
        }
        if (maxSpeedReached)
        {
            speedReset = speed / 2;
            speed = speed - (decceleration / 30) * Time.deltaTime;
            if (speed < speedReset)
            {
                maxSpeedReached = false;
                deccelBool = true;
            }
        }
    }

    public void Debugtext()
    {
        speedText.text = "Speed : " + speed.ToString();
        //speedText.text = Input.GetAxis("Fire2").ToString();
        leftSlider.value = leftFootSpeed / maxFootSpeed;
        rightSlider.value = rightFootSpeed / maxFootSpeed;
        perfectText.text = "Perfects : " + perfectCounter.ToString();
    }
    public void FootSpeedMod(float mod, bool release)
    {
        // low amount on foot
        if ((rightFootSpeed < maxFootSpeed / 3 || leftFootSpeed < maxFootSpeed / 3) && (rightFootSpeed > maxFootSpeed / 2 || leftFootSpeed > maxFootSpeed / 2))
        {
            if(!release)
            {
                speed = speed + (acceleration / (300 * mod) * Time.deltaTime);
            }
            if (release && speed > (1 * mod) * Time.deltaTime)
            {
                speed = speed - (1 * mod) * Time.deltaTime;
                perfectCounter = 0;
            }
        }
        // medium amount on foot
        if ((rightFootSpeed < maxFootSpeed / 2 || leftFootSpeed < maxFootSpeed / 2) && (rightFootSpeed > maxFootSpeed / 1.5f || leftFootSpeed > maxFootSpeed / 1.5f))
        {
            if (!release)
            {
                speed = speed + (acceleration / (200 * mod) * Time.deltaTime);
            }
            if (release && speed > (0.5f * mod) * Time.deltaTime)
            {
                speed = speed - (0.5f * mod) * Time.deltaTime;
                perfectCounter = 0;
            }
        }
        // max amount on foot
        if ((rightFootSpeed < maxFootSpeed / 1.5f || leftFootSpeed < maxFootSpeed / 1.5f) && (rightFootSpeed > maxFootSpeed / 1.2f || leftFootSpeed > maxFootSpeed / 1.2f))
        {
            if (!release)
            {
                speed = speed + (acceleration / (150 * mod) * Time.deltaTime);
            }
            if (release && speed > speed - (1 * mod) * Time.deltaTime)
            {
                perfectCounter += 1;
            }
        }
        else // least amount of time on foot
        {
            if (!release)
            {
                speed = speed + (acceleration / (100 * mod) * Time.deltaTime);
            }
            if (release && speed > (10 * mod) * Time.deltaTime)
            {
                speed = speed - (3 * mod) * Time.deltaTime;
                perfectCounter = 0;

            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            playerRb.AddExplosionForce(100 * Time.deltaTime * explosionForce * speed, collision.contacts[0].point, 10f);
            speed = 0;
        }
        if (collision.gameObject.tag == "OutOfBounds")
        {
            animator.SetBool("Trip", true);
            maxSpeed = oobSpeed;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "OutOfBounds")
        {
            maxSpeed = oobSpeed * outOfBoundsSpeedMultiplyer;
        }
    }


}
