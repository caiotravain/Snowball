//https://learn.unity.com/project/roll-a-ball was used as a reference for this project including most of its code.

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    // Rigidbody of the player.
    private Rigidbody rb;

    // Variable to keep track of collected "PickUp" objects.
    private int count;

    // Variable to keep track of maximum collected "PickUp" objects.
    private int maxCount = 28;

    // Variable to keep track of points
    private int points;

    // Variable to keep track the last time the player collided with a "Tree" object
    private float lastTime;

    //creare a timer that counts up from 0
    float timer = 0.0f;

    // Movement along X and Y axes.
    private float movementX;
    private float movementY;

    // Speed at which the player moves.
    public float speed = 0;

    // UI text component to display count of "PickUp" objects collected.
    public TextMeshProUGUI countText;

    //UI text component to display time
    public TextMeshProUGUI timeText;

    // UI object to display winning text.
    public GameObject winTextObject;

    // UI object to display winning text.
    public GameObject loseTextObject;

    // UI object to restart the game.
    public GameObject restartButton;

    //add sound to the game
    public AudioSource audioSource;

    // Start is called before the first frame update.
    void Start()
    {
        // Get and store the Rigidbody component attached to the player.
        rb = GetComponent<Rigidbody>();

        // Initialize count to zero.
        count = 0;

        points = 0;

        // Update the count display.
        SetCountText();

        // Initially set the win text to be inactive.
        winTextObject.SetActive(false);

        // Initially set the lose text to be inactive.
        loseTextObject.SetActive(false);

        // Initially set the restart button to be inactive.
        restartButton.SetActive(false);

    }

    // This function is called when a move input is detected.
    void OnMove(InputValue movementValue)
    {
        // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();

        // Store the X and Y components of the movement.
        movementX = movementVector.x;
        movementY = movementVector.y;
    }


 

    // FixedUpdate is called once per fixed frame-rate frame.
    private void FixedUpdate()
    {
        // Create a 3D movement vector using the X and Y inputs.
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        // Apply force to the Rigidbody to move the player.
        rb.AddForce(movement * speed);

        // Update the timer and display the time
        SetTimeText();

        if (points < 0)
        {
            endGame();

        }
    }


    void OnTriggerEnter(Collider other)
    {
        // Check if the object the player collided with has the "PickUp" tag.
        if (other.gameObject.CompareTag("PickUp"))
        {
            // Deactivate the collided object (making it disappear).
            other.gameObject.SetActive(false);
            // Play the sound when a "PickUp" object is collected
            audioSource.Play();

            if (points >= 0)
            {
                // Increment the count of "PickUp" objects collected.
                count = count + 1;

                // Increment the points of "PickUp" objects collected.
                points = points + 1;

                // increase size of the player when a "PickUp" object is collected
                transform.localScale += new Vector3(0.07f, 0.07f, 0.07f);

                // Update the count display.
                SetCountText();
            }

           


        }
        // Check if the object the player collided with has the "Tree" tag.
        if (other.gameObject.CompareTag("Tree") & (Time.time - lastTime) > 1 )
        {

            if (count < maxCount & points >= 0)
            {
                // play the sound when a "Tree" object is collected
                audioSource.Play();
                // Decrement the count of "PickUp" objects collected.
                points = points - 1;

                // decrease size of the player when a "Tree" object is collected
                transform.localScale -= new Vector3(0.07f, 0.07f, 0.07f);

                // Update the count display.
                SetCountText();

            }
            
            // stop the player movement when a "Tree" object is collected
            movementX = 0;

            lastTime = Time.time;

        }
        


    }


  

    // Function to update the displayed count of "PickUp" objects collected.
    void SetCountText()
    {
        // Update the count text with the current count.
        countText.text = "Points: " + points.ToString();


        // Check if the count has reached or exceeded the win condition.
        if (count >= maxCount)
        {
            // Display the win text.
            winTextObject.SetActive(true);

            // Display the restart button.
            restartButton.SetActive(true);
            
        }
    }
    void SetTimeText()
    {

        // Update the timer and display the time
        if (count < maxCount)
        {
            // add time to the timer
            timer += Time.deltaTime;

        }

        // display the time in TextMeshPro TimeText
        timeText.text = "Time: " + timer.ToString("F1");
    }   

    void endGame()
    {
        loseTextObject.SetActive(true);
        restartButton.SetActive(true);
    }
    public void Onrestart()
    {
        // Reload the scene to restart the game.
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
