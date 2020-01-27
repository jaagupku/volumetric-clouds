Weather FX Prefabs User Manual

Drag Weather FX Prefab to your scene. 
Carefully set position of the prefab, relatively to the character controller or camera object.
Assign the character controller or camera object to the PlayerOrCamera's variable of the EmitterPositionController script. 
If your camera has the ability to rotate, you should add the camera's parent object (but not the camera directly)  
to the PlayerOrCamera variable. Weather FX Prefab should not inherit camera's rotation.
This allows you to get the correct wind and snow direction relatively to the rotating camera.



