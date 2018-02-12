using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using HoloToolkit.Unity.InputModule;

public class UserController : MonoBehaviour, IInputClickHandler {

    public bool isRunned = false;

    private GestureRecognizer gestureRecognizer;

    [SerializeField]
    private GameObject cursor;

    private enum Speed
    {
        verySlow, slow, high, veryHigh
    };

    Speed currentSpeed = Speed.verySlow;

    private Dictionary<Speed, float> SpeedMap = new Dictionary<Speed, float>
    {
        {Speed.verySlow, 0.1f},
        {Speed.slow, 0.3f},
        {Speed.high, 0.7f},
        {Speed.veryHigh, 1.0f}
    };

    private void Awake()
    {
        //どこでも呼び出したいOnInputClickedがある場合は以下2つのコードで行う
        //InputManager.Instance.PushFallbackInputHandler(gameObject);
        //InputManager.Instance.AddGlobalListener(gameObject);

        //なぜかInteractionManagerのhandlerはコントローラー入力時に呼ばれない
        //InteractionManager.InteractionSourceDetected += InteractionManager_InteractionSourceDetected;
        //InteractionManager.InteractionSourceUpdated -= InteractionManager_InteractionSourceUpdated;

        //InputClickedEventDataではどちらのコントローラーが押されたかを識別できないのでGestureRecognizerを使う
        gestureRecognizer = new GestureRecognizer();
        gestureRecognizer.Tapped += GestureRecognizer_Tapped;

        gestureRecognizer.StartCapturingGestures();
    }

    private void Update()
    {
        if(!isRunned)
        {
            return;
        }

        //RaycastHit hitInfo;
        //float distance = 30f;
        //Ray myRay = Camera.main.ScreenPointToRay(Camera.main.transform.forward);
        //Physics.Raycast(myRay, out hitInfo, distance);

        //if(hitInfo.collider.gameObject == null)
        //{
        //    return;
        //}

        //var targetPos_x = hitInfo.collider.gameObject.transform.position.x;
        //var targetPos_z = hitInfo.collider.gameObject.transform.position.z;
        var targetPos_x = cursor.transform.position.x;
        var targetPos_z = cursor.transform.position.z;
        var targetPosition = new Vector3(targetPos_x, transform.position.y, targetPos_z);

        float speedRatio = 0;

        var isgetted = SpeedMap.TryGetValue(currentSpeed, out speedRatio);
        if(!isgetted)
        {
            return;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, speedRatio * Time.deltaTime);
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        var type = eventData.PressType;
        if (type == InteractionSourcePressInfo.Select && eventData.TapCount == 1)
        {
            if (!isRunned)
            {
                isRunned = true;
                return;
            }

            if (currentSpeed == Speed.veryHigh)
            {
                isRunned = false;
                currentSpeed = Speed.verySlow;
                return;
            }
            currentSpeed++;
        }
        else if(type == InteractionSourcePressInfo.Select && eventData.TapCount == 2)
        {
            isRunned = false;
            currentSpeed = Speed.verySlow;
            return;
        }
    }

    //今回使うジェスチャーコントロール
    public void GestureRecognizer_Tapped(TappedEventArgs obj)
    {
        if(obj.source.handedness == InteractionSourceHandedness.Left)
        {
            //Debug.Log("left Trigger");
            if (!isRunned)
            {
                isRunned = true;
                return;
            }

            if (currentSpeed == Speed.veryHigh)
            {
                isRunned = false;
                currentSpeed = Speed.verySlow;
                return;
            }
            currentSpeed++;
        }

        else if (obj.source.handedness == InteractionSourceHandedness.Right)
        {
            //Debug.Log("right Trigger");
            isRunned = false;
            currentSpeed = Speed.verySlow;
            return;
        }
    }
}
