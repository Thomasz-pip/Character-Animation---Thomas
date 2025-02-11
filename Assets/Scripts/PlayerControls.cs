using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables
    public CharacterController controller;
    public Animator anim;
    public AudioClip runningSound;
    private AudioSource audioSource;

    public float runningSpeed = 4.0f;
    public float rotationSpeed = 100.0f;
    public float jumpHeight = 6.0f;

    private float jumpInput;
    private float runInput;
    private float rotateInput;

    public Vector3 moveDir;

    // Start function to get the components
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update function where movement and input handling happens
    void Update()
    {
        // Get player input for movement
        runInput = Input.GetAxis("Vertical");
        rotateInput = Input.GetAxis("Horizontal");

        CheckJump();
        
        moveDir = new Vector3(0, jumpInput * jumpHeight, runInput * runningSpeed);

        // Transform moveDir to game world space
        moveDir = transform.TransformDirection(moveDir);

        // Move the character using the controller
        controller.Move(moveDir * Time.deltaTime);

        // Rotate the character based on horizontal input
        transform.Rotate(0f, rotateInput * rotationSpeed * Time.deltaTime, 0f);

        // Call functions to handle jump and effects
        Effects();
    }

    // Function to check for jump input and control jumping behavior
    void CheckJump()
    {
        if (Input.GetKey(KeyCode.Space)) // Check if space key is pressed
        {
            jumpInput = 1; // Start jumping

            // Stop running sound if it's already playing
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        // If grounded, reset jump input to 0
        if (controller.isGrounded)
        {
            jumpInput = 0;
        }
    }

    // Function to handle animations and sound effects
    void Effects()
    {
        // Check if runInput value is NOT 0 AND jumpInput value IS 0
        if (runInput != 0 && jumpInput == 0)
        {
            // Check if jumpInput IS 1
if (jumpInput == 1)
{
    // If true then set Boolean "Jump" parameter to true
    anim.SetBool("Jump", true);
} else {
    // If false then set Boolean "Jump" parameter to false
    anim.SetBool("Jump", false);
}
            // Trigger the "Run Forward" animation
            anim.SetBool("Run Forward", true);

            // Play the running sound if not already playing and grounded
            if (audioSource != null && !audioSource.isPlaying && controller.isGrounded)
            {
                audioSource.clip = runningSound;
                audioSource.Play();
            }
        }
        else
        {
            // Stop the "Run Forward" animation if not running
            anim.SetBool("Run Forward", false);

            // Stop the running sound if it's playing
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}