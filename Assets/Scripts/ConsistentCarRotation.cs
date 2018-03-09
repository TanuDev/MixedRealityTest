using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsistentCarRotation : MonoBehaviour {

    private UserController controller;

    private float defaultYOffset = -0.888f;

    private void Awake()
    {
        controller = GetComponentInParent<UserController>();
    }

    private void Update()
    {
        var targetRotation = Camera.main.transform.localRotation.eulerAngles;
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, targetRotation.y, transform.localRotation.eulerAngles.z);

        var targetPosition = Camera.main.transform.localPosition;

        transform.localPosition = new Vector3(transform.localPosition.x, (targetPosition.y + defaultYOffset), transform.localPosition.z);
    }
}
