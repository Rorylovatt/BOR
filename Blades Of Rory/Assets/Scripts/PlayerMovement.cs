using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerMovement : MonoBehaviour
{
    #region Variables Start Update
    // Start is called before the first frame update
    public GameObject leftFoot, rightFoot;
    public Rigidbody playerRb;
    public float maxSpeed, maxFootSpeed, footAcceleration, acceleration, decceleration, explosionForce, rotateSpeed, outOfBoundsSpeedMultiplyer, boostSpeedMultiplyer, speed;
    public bool boost, maxSpeedReached;
    private float leftFootSpeed, rightFootSpeed, speedReset, horizontalInput, stepTime, oobSpeed, boostSpeed, rubbishSpeed;
    private bool deccelBool, left, right, releaseLeft, releaseRight, boostReady, outOfBounds;
    public Animator animator;
    public Text perfectText;
    public TextMeshProUGUI leftHitText, rightHitText, boostText;
    //public Slider leftSlider, rightSlider;
    public int perfectCounter, rubbishCounter;
    Racemanager racemanager;
    // hello
    void Start()
    {
        racemanager = FindObjectOfType<Racemanager>();
        animator = GetComponentInChildren<Animator>();
        oobSpeed = maxSpeed / outOfBoundsSpeedMultiplyer;
        rubbishSpeed = maxSpeed / 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (racemanager.raceStart)
        {
            Movement();

        }
        GUIMain();
        //Debugtext();
    }
    #endregion

    #region Main Movement Functions
    public void Movement()
    {
        if (!racemanager.raceFinish)
        {
            Rotate();
        }
        else
        {
            animator.SetBool("Win", true);
            animator.SetBool("LeftStep", false);
            animator.SetBool("RightStep", false);
            animator.SetBool("Trip", false);
        }
        animator.SetFloat("Speed", speed);
        Speed();
        if (!boost)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            if (!racemanager.raceFinish)
            {
                Controls();

            }
        }
        if (boost)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * boostSpeed);
        }
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
    public void Speed()
    {
        // Deccelrate 
        if ((deccelBool && speed > 0 && !boost) || racemanager.raceFinish)
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
        if (rubbishCounter >= 2)
        {
            if (speed > rubbishSpeed)
            {
                speed = rubbishSpeed;
            }
        }
        // max speed reached / trip
        if (rightFootSpeed >= maxFootSpeed || leftFootSpeed >= maxFootSpeed)
        {
            maxSpeedReached = true;
            animator.SetBool("Trip", true);
            leftHitText.text = "";
            rightHitText.text = "";
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
        if (outOfBounds)
        {
            perfectCounter = 0;
        }
        if (perfectCounter >= 3)
        {
            boostReady = true;
        }
        if(perfectCounter < 3)
        {
            boostReady = false;
        }
        if (boostReady && Input.GetButtonDown("Jump") && !maxSpeedReached)
        {
            boostSpeed = speed + boostSpeedMultiplyer;
            animator.SetTrigger("Boost");
            boost = true;
            boostReady = false;
        }
        if (boost)
        {
            animator.SetBool("LeftStep", false);
            animator.SetBool("RightStep", false);
            perfectCounter = 0;
            if (boostSpeed > speed)
            {
                boostSpeed -= (decceleration / 40) * Time.deltaTime;
            }
            else
            {
                boost = false;
                right = true;
                left = true;
            }
        }
    }
    #endregion

    #region Controls
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
            FootSpeedMod(2f, false, leftFootSpeed, leftHitText);
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
            FootSpeedMod(2f, false, rightFootSpeed, rightHitText);
        }
        // On release buttons
        if (Input.GetAxis("Fire1") == 0 && releaseRight == true)
        {
            right = false;
            left = true;
            releaseRight = false;
            FootSpeedMod(2f, true, rightFootSpeed, rightHitText);
            rightFootSpeed = 0;
        }
        if (Input.GetAxis("Fire2") == 0 && releaseLeft == true)
        {
            right = true;
            left = false;
            releaseLeft = false;
            FootSpeedMod(2f, true, leftFootSpeed, leftHitText);
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
            //FootSpeedMod(2f, false, footSpeed);
        }

    }
    public void FootSpeedMod(float mod, bool release, float footSpeed, TextMeshProUGUI footText)
    {
        // least amount on foot
        if (footSpeed > 0 && footSpeed < maxFootSpeed / 2)
        {
            FootModFunc(mod, release, 5f);
            if (release)
            {
                rubbishCounter ++;
                perfectCounter = 0;
                footText.text = "Rubbish!";
                footText.fontSize = 36;

            }
        }
        // low amount on foot
        if (footSpeed > maxFootSpeed / 2 && footSpeed < maxFootSpeed / 1.5f)
        {
            FootModFunc(mod, release, 2f);
            if (release)
            {
                rubbishCounter = 0;
                perfectCounter = 0;
                footText.text = "OK!";
                footText.fontSize = 36;

            }
        }
        // medium amount on foot
        if (footSpeed > maxFootSpeed / 1.5f && footSpeed < maxFootSpeed / 1.2f)
        {
            FootModFunc(mod, release, 1.5f);
            if (release)
            {
                rubbishCounter = 0;
                perfectCounter = 0;
                footText.text = "Good!";
                footText.fontSize = 36;
            }
        }
        // perfect amount of time on foot
        if (footSpeed > maxFootSpeed / 1.2f && footSpeed <= maxFootSpeed)
        {
            FootModFunc(mod, release, 1.2f);
            if (release)
            {
                perfectCounter += 1;
                footText.text = "Perfect!";
                footText.fontSize = 36;

            }
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
    #endregion

    #region GUI
    public void GUIMain()
    {
        TextSize(leftHitText, 0.2f);
        TextSize(rightHitText, 0.2f);
        BoostText();
    }
    public void TextSize(TextMeshProUGUI text, float decrease)
    {
        if(text.fontSize > 0)
        {
            text.fontSize -= decrease;
        }
    }
    public void BoostText()
    {
        if(perfectCounter >= 3 && !racemanager.raceFinish)
        {
            boostText.text = "Boost Ready!";
        }
        if(perfectCounter < 3)
        {
            boostText.text = "";
        }
    }
    #endregion

    #region Collisions

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            playerRb.AddExplosionForce(100 * Time.deltaTime * explosionForce * speed, collision.contacts[0].point, 10f);
            speed = 0;
            //if(!boost)
            //{
            //    playerRb.AddExplosionForce(100 * Time.deltaTime * explosionForce * speed, collision.contacts[0].point, 10f);
            //    speed = 0;
            //}
            //else
            //{
            //    playerRb.AddForce(-transform.forward * explosionForce * Time.deltaTime * 100f);
            //}
        }
        if (collision.gameObject.tag == "OutOfBounds")
        {
            animator.SetBool("Trip", true);
            maxSpeed = oobSpeed;
            outOfBounds = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "OutOfBounds")
        {
            maxSpeed = oobSpeed * outOfBoundsSpeedMultiplyer;
            outOfBounds = false;
        }
    }
    #endregion

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


//if (boost)
//{
//    maxSpeed = boostMaxSpeed;
//    speed = speed + boostSpeed * Time.deltaTime;
//    float tempSpeed = speed;
//    perfectCounter = 0;

//    if(speed > tempSpeed)
//    {
//        speed -= (decceleration / 1000) * Time.deltaTime;
//    }
//    else
//    {
//        maxSpeed = boostMaxSpeed - boostSpeed;
//        speed = tempSpeed;
//        boost = false;
//    }
//}
//public void Debugtext()
//{
//    //if (!boost)
//    //{
//    //    speedText.text = "Speed : " + speed.ToString();

//    //}
//    //else
//    //{
//    //    speedText.text = "Boost! : " + boostSpeed.ToString();

//    //}
//    //speedText.text = Input.GetAxis("Fire2").ToString();
//    leftSlider.value = leftFootSpeed / maxFootSpeed;
//    rightSlider.value = rightFootSpeed / maxFootSpeed;
//    perfectText.text = "Perfects : " + perfectCounter.ToString();
//}

#endregion