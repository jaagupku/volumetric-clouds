using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/************************ meshzone3d.com
     * This super simple script only moves the emitter together with the player or camera. 
     * Drag Weather FX prefab to your scene. To the PlayerOrCamera variable of EmitterPositionController 
     * script assign the player or camera object. If your camera has the rotation ability you must drag 
     * the camera parent object (not directly camera) to the PlayerOrCamera variable. This will allow the 
     * correctly behavior of wind and snow direction relative to the camera (like in demo scene).
*/

public class EmitterPositionController : MonoBehaviour
{
    

    public Transform PlayerOrCamera;

	// Use this for initialization
	void Start ()
    {
        if (PlayerOrCamera == null)
            Debug.Log("You should attach player object or camera parent object to EmitterPositionController script.");

    }
	
	// Update is called once per frame
	void Update ()
    {
        if(PlayerOrCamera != null)
            transform.position = PlayerOrCamera.position;

    }
}
