using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsistentCarRotation : MonoBehaviour {

    private UserController controller;

    private void Awake()
    {
        controller = GetComponentInParent<UserController>();
    }

    private void Update()
    {
        var isRunned = controller.isRunned;
        if(!isRunned)
        {
            return;
        }

        var targetRotation = Camera.main.transform.localRotation.eulerAngles;
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, targetRotation.y, transform.localRotation.eulerAngles.z);

    }
}
