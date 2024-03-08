using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject leftFoot, rightFoot;
    public Rigidbody playerRb;
    public float maxSpeed, maxFootSpeed, footAcceleration, acceleration, decceleration, explosionForce, rotateSpeed, outOfBoundsSpeedMultiplyer, boostSpeed;
    private float speed, leftFootSpeed, rightFootSpeed, speedReset, horizontalInput, stepTime, oobSpeed, boostMaxSpeed;
    private bool deccelBool, maxSpeedReached, left, right, releaseLeft, releaseRight, boost;
    private Animator animator;
    public Text speedText, perfectText;
    public Slider leftSlider, rightSlider;
    public int perfectCounter;
    // hello
    void Start()
    {
        boostMaxSpeed = maxSpeed + boostSpeed;
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
        if (horizontalInput > 0.01f || horizontalInput < -0.01f)
        {
            // playerRb.AddForce(this.transform.forward * speed, ForceMode.Acceleration);
            this.gameObject.transform.Rotate(Vector3.up, Time.deltaTime * 100 * rotateSpeed * horizontalInput, Space.World);

        }
    }
    public void Movement()
    {
        Rotate();
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        animator.SetFloat("Speed", speed);
        if (!boost)
        {
            Controls();
        }
        Speed();
    }
    public void Speed()
    {
        // Deccelrate 
        if (deccelBool && speed > 0)
        {
            speed -= (decceleration / 100) * Time.deltaTime;
        }
        if (speed <= 0)
        {
            deccelBool = false;
            speed = 0;
        }
        // keep speed in range
        if (speed > maxSpeed && !boost)
        {
            speed = maxSpeed;
        }
        // max speed reached / trip
        if (rightFootSpeed >= maxFootSpeed || leftFootSpeed >= maxFootSpeed)
        {
            maxSpeedReached = true;
            animator.SetBool("Trip", true);
        }
        if (maxSpeedReached)
        {
            perfectCounter = 0;
            speedReset = speed / 2;
            speed = speed - (decceleration / 30) * Time.deltaTime;
            if (speed < speedReset)
            {
                maxSpeedReached = false;
                deccelBool = true;
            }
        }
        //boost
        if (perfectCounter >= 3)
        {
            maxSpeed = boostMaxSpeed;
            speed = speed + boostSpeed * Time.deltaTime;
            boost = true;


        }
        if (boost)
        {
            float tempSpeed = speed;
            perfectCounter = 0;

            if(speed > tempSpeed)
            {
                speed -= (decceleration / 1000) * Time.deltaTime;
            }
            else
            {
                maxSpeed = boostMaxSpeed - boostSpeed;
                speed = tempSpeed;
                boost = false;
            }
        }
    }
    public void Controls()
    {
        //ButtonPress("Fire2", "LeftStep", "RightStep", leftFootSpeed, left, releaseLeft);
        //ButtonPress("Fire1", "RightStep", "LeftStep", rightFootSpeed, right, releaseRight);

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
            FootSpeedMod(2f, false, leftFootSpeed);
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
            FootSpeedMod(2f, false, rightFootSpeed);
        }
        // On release buttons
        if (Input.GetAxis("Fire1") == 0 && releaseRight == true)
        {
            right = false;
            left = true;
            releaseRight = false;
            FootSpeedMod(2f, true, rightFootSpeed);
            rightFootSpeed = 0;
        }
        if (Input.GetAxis("Fire2") == 0 && releaseLeft == true)
        {
            right = true;
            left = false;
            releaseLeft = false;
            FootSpeedMod(2f, true, leftFootSpeed);
            leftFootSpeed = 0;
        }
        // no buttons

        if (Input.GetAxis("Fire2") == 0 && Input.GetAxis("Fire1") == 0)
        {
            deccelBool = true;
            stepTime -= 1;
            if (stepTime < 0)
            {
                animator.SetBool("LeftStep", false);
                animator.SetBool("RightStep", false);
            }
        }
    }
    public void ButtonPress(string button, string animatorTrue, string animatorFalse, float footSpeed, bool foot, bool footRelease)
    {

        if (Input.GetAxis(button) == 1 && footSpeed < maxFootSpeed && (foot || speed == 0) && !maxSpeedReached)
        {
            animator.SetBool("Trip", false);
            stepTime = 2;
            animator.SetBool(animatorTrue, true);
            animator.SetBool(animatorFalse, false);
            foot = true;
            footRelease = true;
            deccelBool = false;
            footSpeed += (footAcceleration / 100) * Time.deltaTime;
            FootSpeedMod(2f, false, footSpeed);
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

    public void FootSpeedMod(float mod, bool release, float footSpeed)
    {
        // least amount on foot
        if (footSpeed > maxFootSpeed / 3 && footSpeed < maxFootSpeed / 2)
        {
            FootModFunc(mod, release, 3f);
            if (release) perfectCounter = 0;
        }
        // low amount on foot
        if (footSpeed > maxFootSpeed / 2 && footSpeed < maxFootSpeed / 1.5f)
        {
            FootModFunc(mod, release, 2f);
            if (release) perfectCounter = 0;
        }
        // medium amount on foot
        if (footSpeed > maxFootSpeed / 1.5f && footSpeed < maxFootSpeed / 1.2f)
        {
            FootModFunc(mod, release, 1.5f);
            if (release) perfectCounter = 0;
        }
        // perfect amount of time on foot
        if (footSpeed > maxFootSpeed / 1.2f)
        {
            FootModFunc(mod, release, 1.2f);
            if (release) perfectCounter += 1;
        }
    }
    public void FootModFunc(float mod, bool release, float multiplyer)
    {
        if (!release)
        {
            speed = speed + (acceleration / ((multiplyer * 10) * mod) * Time.deltaTime);
        }
        if (release && speed > (multiplyer * mod) * Time.deltaTime)
        {
            speed = speed - ((multiplyer / 3) * mod) * Time.deltaTime;
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
#region Unused Functions
//public void FootSpeedMod(float mod, bool release)
//{
//    // low amount on foot
//    if ((rightFootSpeed < maxFootSpeed / 3 || leftFootSpeed < maxFootSpeed / 3) && (rightFootSpeed > maxFootSpeed / 2 || leftFootSpeed > maxFootSpeed / 2))
//    {
//        if(!release)
//        {
//            speed = speed + (acceleration / (300 * mod) * Time.deltaTime);
//        }
//        if (release && speed > (1 * mod) * Time.deltaTime)
//        {
//            speed = speed - (1 * mod) * Time.deltaTime;
//            perfectCounter = 0;
//        }
//    }
//    // medium amount on foot
//    if ((rightFootSpeed < maxFootSpeed / 2 || leftFootSpeed < maxFootSpeed / 2) && (rightFootSpeed > maxFootSpeed / 1.5f || leftFootSpeed > maxFootSpeed / 1.5f))
//    {
//        if (!release)
//        {
//            speed = speed + (acceleration / (200 * mod) * Time.deltaTime);
//        }
//        if (release && speed > (0.5f * mod) * Time.deltaTime)
//        {
//            speed = speed - (0.5f * mod) * Time.deltaTime;
//            perfectCounter = 0;
//        }
//    }
//    // max amount on foot
//    if ((rightFootSpeed < maxFootSpeed / 1.5f || leftFootSpeed < maxFootSpeed / 1.5f) && (rightFootSpeed > maxFootSpeed / 1.2f || leftFootSpeed > maxFootSpeed / 1.2f))
//    {
//        if (!release)
//        {
//            speed = speed + (acceleration / (150 * mod) * Time.deltaTime);
//        }
//        if (release && speed > speed - (1 * mod) * Time.deltaTime)
//        {
//            perfectCounter += 1;
//        }
//    }
//    else // least amount of time on foot
//    {
//        if (!release)
//        {
//            speed = speed + (acceleration / (100 * mod) * Time.deltaTime);
//        }
//        if (release && speed > (10 * mod) * Time.deltaTime)
//        {
//            speed = speed - (3 * mod) * Time.deltaTime;
//            perfectCounter = 0;

//        }
//    }
//}
#endregion