using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HazardousTiles : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8) //Checks to see if it is player layer
        {
            Death();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
    
    void Death()
    {
        Debug.Log("You died!");
        //Death animation goes here
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
