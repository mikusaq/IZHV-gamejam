using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBuffer : MonoBehaviour
{
    public Transform playerPosition;
    public CharacterController controller;
    private Vector2[] array;
    private int arraySize;
    private int arrayLength = 200;
    Vector3 currentVelocity;
    public float smoothTime = 0.1f;
    public float moveSpeed = 0.25f;
    private bool zPressed;
    public float step = 0.5f;
    private GameObject cameraControl;

    IEnumerator enterTimeLoop(){
        print("Entering time loop...");
        enabled = false;

        //cameraControl.GetComponent<CameraControl>().enabled = false;
        GetComponent<PlayerControl>().enabled = false;
        

        int i = 0;
        bool reversed = false;
        Vector3 move = new Vector3(0, 0, 0);
        while(!Input.GetKey(KeyCode.Space)){
            move.x = array[i].x;
            move.y = array[i].y;
            Vector3 diff = transform.TransformDirection(move - transform.position);
            controller.Move(diff * moveSpeed);
            //transform.position = Vector3.SmoothDamp(transform.position, move, ref currentVelocity, smoothTime);
            //transform.position = Vector3.MoveTowards(transform.position, move, step);
            //transform.position = Vector3.Slerp(transform.position, move, moveSpeed);
            if(i == arrayLength -1){
                reversed = true;
            }
            else if(i == 0){
                reversed = false;
            }
            if(reversed){
                i--;
            }
            else{
                i++;
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        enabled = true;
        GetComponent<PlayerControl>().enabled = true;
        //cameraControl.GetComponent<CameraControl>().enabled = true;
        print("Exitting time loop.");
    }

    void printArray(Vector2[] array){
        print("Array size: " + arraySize);
        for(int i = 0; i < arraySize; i++){
            print("index: " + i + '\n' + "values: " + array[i].x + " " + array[i].y);
            print("values: " + array[i].x + " " + array[i].y);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        array = new Vector2[arrayLength];
        arraySize = 0;
        zPressed = false;
        cameraControl = GameObject.FindWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetKeyDown(KeyCode.T)){
            printArray(array);
        }
        if(Input.GetKeyDown(KeyCode.Z)){
            StartCoroutine(enterTimeLoop());
            zPressed = true;
        }

        // Shifts values in array
        for(int i = arraySize; i > 0; i--){
            array[i] = array[i-1];
        }
        // Inserts current player position
        array[0].x = playerPosition.position.x;
        array[0].y = playerPosition.position.y;

        if(arraySize < arrayLength-1){
            arraySize++;
        }

    }
    void FixedUpdate(){
        if(zPressed){
            //StartCoroutine(enterTimeLoop());
            zPressed = false;
        }
    }
}
