using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingCube : MonoBehaviour {
    
    [SerializeField]
    private float movingspeed = 1f;

    public static MovingCube LastCube { get; private set; }
    public static MovingCube CurrentCube { get; private set; }
    public MoveDirection MoveDirection { get; set; }

    private void OnEnable()
    {
        if(LastCube==null){
            LastCube =  GameObject.Find("Start").GetComponent<MovingCube>();
        }
        CurrentCube = this;
        GetComponent<Renderer>().material.color = GetRandomColor();
        transform.localScale = new Vector3(LastCube.transform.localScale.x, transform.localScale.y, LastCube.transform.localScale.z);
    }

    private Color GetRandomColor()
    {
        return new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f));
    }

    internal void Stop()
    {
        movingspeed = 0;
        float hangover = GetHangover();


        float max = MoveDirection == MoveDirection.Z ? LastCube.transform.localScale.z : LastCube.transform.localScale.x;
        if(Mathf.Abs(hangover)>= max){
            LastCube = null;
            CurrentCube = null;
            SceneManager.LoadScene(0);
        }
        float direction = hangover > 0 ? 1f : -1f;
        if(MoveDirection == MoveDirection.Z){
            SplitCubeZ(hangover, direction);
        }
        else{
            SplitCubeX(hangover, direction);
        }

        LastCube = this;
    }

    private float GetHangover()
    {
        if(MoveDirection == MoveDirection.Z){
            return transform.position.z - LastCube.transform.position.z;
        }
        else{
            return transform.position.x - LastCube.transform.position.x;
        }
    }

    private void SplitCubeX(float hangover, float direction)
    {
        float newXsize = LastCube.transform.localScale.x - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.x - newXsize;

        float newXposition = LastCube.transform.position.x + (hangover / 2);
        transform.localScale = new Vector3(newXsize,transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newXposition,transform.position.y, transform.position.z);

        float cubeEdge = transform.position.x + (newXsize / 2f * direction);
        float fallingBlockXPosition = cubeEdge + (fallingBlockSize / 2f * direction);
        SpawnDropCube(fallingBlockXPosition, fallingBlockSize);
    }

    private void SplitCubeZ(float hangover,float direction)
    {
        float newZsize = LastCube.transform.localScale.z - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.z - newZsize;

        float newZposition = LastCube.transform.position.z + (hangover / 2);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZsize);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZposition);

        float cubeEdge = transform.position.z + (newZsize / 2f * direction);
        float fallingBlockZPosition = cubeEdge + (fallingBlockSize / 2f * direction);
        SpawnDropCube(fallingBlockZPosition,fallingBlockSize);
    }

    private void SpawnDropCube(float fallingBlockZPosition, float fallingBlockSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if(MoveDirection==MoveDirection.Z){
            cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
            cube.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockZPosition);
   
        }
        else{
            cube.transform.localScale = new Vector3(fallingBlockSize,transform.localScale.y, transform.localScale.z);
            cube.transform.position = new Vector3(fallingBlockZPosition,transform.position.y, transform.position.z);

        }


        cube.AddComponent<Rigidbody>();
        cube.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
        Destroy(cube.gameObject, 1f);

    }
   
    // Update is called once per frame
    private void Update () {

        if(MoveDirection == MoveDirection.Z){
            transform.position += transform.forward * Time.deltaTime * movingspeed; 
        }
        else{
            transform.position += transform.right * Time.deltaTime * movingspeed;

        }

		
	}
}
