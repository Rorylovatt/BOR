using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject leftFoot, rightFoot;
    public Rigidbody playerRb;
    public float maxSpeed, maxFootSpeed, footAcceleration, acceleration, decceleration, explosionForce, rotateSpeed;
    private float speed, leftFootSpeed, rightFootSpeed, speedReset, horizontalInput, stepTime;
    private bool deccelBool, maxSpeedReached, left;
    private Animator animator;
    public Text speedText;
    public Slider leftSlider, rightSlider;
    // hello
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
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
        animator.SetFloat("Speed", speed);

        // Left foot
        if (Input.GetAxis("Fire2") == 1 && leftFootSpeed < maxFootSpeed && (left || speed == 0) && !maxSpeedReached)
        {
            animator.SetBool("Trip", false);
            stepTime = 2;
            animator.SetBool("LeftStep", true);
            animator.SetBool("RightStep", false);
            left = true;
            deccelBool = false;
            leftFootSpeed += (footAcceleration / 100);
            FootSpeedMod(0.8f);
        }
        if (Input.GetAxis("Fire2") == 0)
        {
            left = false; 
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
        if (Input.GetAxis("Fire1") == 1 && rightFootSpeed < maxFootSpeed && (!left || speed == 0) && !maxSpeedReached)
        {
            animator.SetBool("Trip", false);
            stepTime = 2;
            animator.SetBool("RightStep", true);
            animator.SetBool("LeftStep", false);
            left = false;
            deccelBool = false;
            rightFootSpeed += (footAcceleration / 100);
            FootSpeedMod(2f);
        }
        if (Input.GetAxis("Fire1") == 0)
        {
            left = true;
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
        // Speed sorters 
        if (deccelBool && speed > 0)
        {
            speed = speed - (decceleration / 100);
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
            speed = speed - (decceleration / 30);
            if(speed < speedReset)
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
    }
    public void FootSpeedMod(float mod)
    {
        if((rightFootSpeed < maxFootSpeed / 3 || leftFootSpeed < maxFootSpeed / 3) && (rightFootSpeed > maxFootSpeed / 2 || leftFootSpeed > maxFootSpeed / 2))
        {
            speed = speed + (acceleration / (300 * mod));
        }
        if ((rightFootSpeed < maxFootSpeed / 2 || leftFootSpeed < maxFootSpeed / 2) && (rightFootSpeed > maxFootSpeed / 1.5f || leftFootSpeed > maxFootSpeed / 1.5f))
        {
            speed = speed + (acceleration / (200 * mod));
        }
        if ((rightFootSpeed < maxFootSpeed / 1.5f || leftFootSpeed < maxFootSpeed / 1.5f) && (rightFootSpeed > maxFootSpeed / 1.2f || leftFootSpeed > maxFootSpeed / 1.2f))
        {
            speed = speed + (acceleration / (150 * mod));
        }
        else
        {
            speed = speed + (acceleration / (100 * mod));
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            playerRb.AddExplosionForce(100 * Time.deltaTime * explosionForce * speed, collision.contacts[0].point, 10f);
            speed = 0;
        }
    }


}
