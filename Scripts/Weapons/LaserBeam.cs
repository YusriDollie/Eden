//This is free to use and no attribution is required
//No warranty is implied or given
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class LaserBeam : MonoBehaviour{
    Vector2 mouse;
    RaycastHit hit;
    float range = 100.0f;
    LineRenderer line;
    //public Material lineMaterial;
    public GameObject laserDot;

    void Start(){
        line = GetComponent<LineRenderer>();
        line.SetVertexCount(2);
        //line.GetComponent<Renderer>().material = lineMaterial;
        line.SetWidth(0.01f, 0.01f);
    }

    void Update(){
        //LaserDot
        //line.GetComponent<Renderer>().mainTextureOffset = new Vector2(0,Time.time);

        Ray ray = new Ray(transform.position, -transform.right);
        //Ray ray = new Ray (Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        line.SetPosition(0, transform.position);

        if(Physics.Raycast(ray, out hit, 100)){
            line.SetPosition(1, hit.point);
            if(hit.rigidbody){
                hit.rigidbody.AddForceAtPosition(transform.forward * 500, hit.point);
                //GameObject temp = GameObject.Instantiate (hitEffect, hit.point, Quaternion.identity) as GameObject;
                //temp.gameObject.transform.SetParent(hit.transform);
//				laserDot.transform.position = hit.point;
//				laserDot.transform.rotation = Quaternion.Euler(hit.normal);
            }
        } else{
            line.SetPosition(1, ray.GetPoint(100));
        }
    }
}