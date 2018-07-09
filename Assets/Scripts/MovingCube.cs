using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube : MonoBehaviour {
    
    [SerializeField]
    private float movingspeed = 1f;

    public static MovingCube LastCube { get; private set; }
    public static MovingCube CurrentCube { get; private set; }

    private void OnEnable()
    {
        if(LastCube==null){
            LastCube =  GameObject.Find("Start").GetComponent<MovingCube>();
        }
        CurrentCube = this;
        GetComponent<Renderer>().material.color = GetRandomColor();
    }

    private Color GetRandomColor()
    {
        return new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f));
    }

    internal void Stop()
    {
        movingspeed = 0;
        float hangover = transform.position.z - LastCube.transform.position.z;
        float direction = hangover > 0 ? 1f : -1f;
        SplitCubeZ(hangover,direction);
    }

    private void SplitCubeZ(float hangover,float direction)
    {
        float newZsize = LastCube.transform.localScale.z - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.z - newZsize;

        float newZposition = LastCube.transform.position.z + (hangover / 2);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZsize);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZposition);

        float cubeEdge = transform.position.z - newZsize / (2f*direction);
        float fallingCubeZPosition = cubeEdge + fallingBlockSize / (2f*direction);
        SpawnDropCube(fallingCubeZPosition,fallingBlockSize);
    }

    private void SpawnDropCube(float fallingCubeZPosition, float fallingBlockSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
        cube.transform.position = new Vector3(transform.position.x, transform.position.y, fallingCubeZPosition);
        cube.AddComponent<Rigidbody>();
        cube.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
        Destroy(cube.gameObject, 1f);

    }
   
    // Update is called once per frame
    private void Update () {

        transform.position += transform.forward * Time.deltaTime * movingspeed;
		
	}
}
