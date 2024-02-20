using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check if left mouse button was clicked
        {
            RaycastFromCamera();
        }
    }

    void RaycastFromCamera()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Object was hit, extract info
            Vector3 hitPosition = hit.point; // Position where the raycast hits the object
            string objectName = hit.collider.gameObject.name; // Name of the object hit
            string objectTags = hit.collider.gameObject.tag; // Tag of the object hit

            // Print information to the console
            Debug.Log("Hit Position: " + hitPosition.ToString());
            Debug.Log("Object Name: " + objectName);
            Debug.Log("Object Tag: " + objectTags);

            // Leaving space for a grappling hook later
        }
    }
}

