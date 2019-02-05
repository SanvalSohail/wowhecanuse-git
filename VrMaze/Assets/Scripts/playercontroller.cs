using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class playercontroller : MonoBehaviour {
	
    public float speed;
    public float jumpForce;
	public Text countText;
	public Text winText;

    private Rigidbody rb;
    public SphereCollider col;
	private int count;
    private bool gamefinish;
    private static Stopwatch stopWatch;
    private string elapsedTime;
    public LayerMask groundLayers;

    private float ypos;

    void Start ()
    {
        stopWatch = new Stopwatch();
        stopWatch.Start();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();
		count = 0;
        ypos = rb.position.y;
        gamefinish = false;
		setCountText();
		winText.text = "";
    }

    void FixedUpdate ()
    {
        float moveHorizontal = Input.GetAxis ("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");
        ypos = rb.position.y;

        Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

        rb.AddForce (movement * speed);

        if(isGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up*jumpForce, ForceMode.Impulse);
        }

        if (ypos < -0.5)
		{
                        stopWatch.Stop();
            System.TimeSpan ts = stopWatch.Elapsed;
            elapsedTime = System.String.Format("{0:0} {1:00}",
             ts.Minutes + "m", ts.Seconds + "s"
            );
			winText.text = "You Lose \n" + elapsedTime;
		}
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Finish"))
        {
            gamefinish = true;
            setCountText();
        }

        if (other.gameObject.CompareTag ("PickUp"))
        {
            other.gameObject.SetActive (false);
			count++;
			setCountText();
        }
    }

	void setCountText()
	{
		countText.text = "Count: " + count.ToString();
		if (count >= 9 && gamefinish == true)
		{
                        stopWatch.Stop();
            System.TimeSpan ts = stopWatch.Elapsed;
            elapsedTime = System.String.Format("{0:0} {1:00}",
             ts.Minutes + "m", ts.Seconds + "s"
            );
			winText.text = "You Win! \n" + elapsedTime;
		}
	}

    private bool isGrounded()
    {
       return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.center.y), col.radius * 0.9f, groundLayers);
    }
}
