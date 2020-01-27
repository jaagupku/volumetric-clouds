using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffController : MonoBehaviour
{

    private DayNightController dayNightController;
    // Start is called before the first frame update
    void Start()
    {
        dayNightController = GetComponent<DayNightController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("3") || Input.GetKeyDown("[3]"))
        {
        dayNightController.enabled = !dayNightController.enabled;
        }
    }
}
