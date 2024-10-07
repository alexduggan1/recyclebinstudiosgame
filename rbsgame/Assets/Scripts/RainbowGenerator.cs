using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class RainbowGenerator : MonoBehaviour
{
    public Player ownerException;

    public float bulletSpeed;

    public Rigidbody rb;

    public Vector3 dir;

    public float startingBulletSpeed;


    public RoadMeshCreator roadMeshCreator;

    public Transform wallItself;
    public Vector3 wallPos;

    public List<Vector3> points;

    float cd;

    public PathCreator pathCreator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //wallPos = wallItself.position;

        wallItself.parent = transform.parent;
        wallItself.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
        cd = 0f;


        rb.velocity = dir * startingBulletSpeed;
        points = new List<Vector3> { };

        
        Physics.IgnoreCollision(this.GetComponent<Collider>(), wallItself.GetComponent<Collider>(), true);
    }

    // Update is called once per frame
    void Update()
    {
        wallItself.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
        if (cd <= 0)
        {
            points.Add(transform.position);
            wallItself.GetComponent<RainbowWall>().path.Add(transform.position);

            if(points.Count > 2)
            {
                roadMeshCreator.pathCreator = pathCreator;
                roadMeshCreator.AssignMeshComponents();
                roadMeshCreator.AssignMaterials();
                roadMeshCreator.path2 = GeneratePath(points);
                roadMeshCreator.CreateRoadMesh2();
                wallItself.GetComponent<MeshCollider>().convex = false;
                wallItself.GetComponent<MeshCollider>().convex = true;
            }
            cd = 0.07f;
        }
        else
        {
            cd -= Time.deltaTime;
        }
    }

    VertexPath GeneratePath(List<Vector3> points)
    {
        Vector3[] points2 = new Vector3[points.Count];
        int i = 0;
        foreach (Vector3 point in points)
        {
            points2[i] = point;
            i++;
        }

        // Create a closed, 2D bezier path from the supplied points array
        // These points are treated as anchors, which the path will pass through
        // The control points for the path will be generated automatically
        BezierPath bezierPath = new BezierPath(points2, false, PathSpace.xyz);
        // Then create a vertex path from the bezier path, to be used for movement etc
        return new VertexPath(bezierPath, transform);
    }



    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 7)
        {
            Debug.Log("rainbow hit killbox or stage");
            // stick
            rb.constraints = RigidbodyConstraints.FreezeAll;
            wallItself.GetComponent<MeshCollider>().convex = true;

            wallItself.gameObject.layer = 7;
            StartCoroutine(VanishSoon());
        }
    }

    IEnumerator VanishSoon()
    {
        yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    }
}
