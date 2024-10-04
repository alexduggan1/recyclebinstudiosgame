using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class RainbowWall : MonoBehaviour
{
    public List<Vector3> path;

    public RoadMeshCreator roadMeshCreator;

    // Start is called before the first frame update
    void Start()
    {
        roadMeshCreator = GetComponent<RoadMeshCreator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0, 0, 0.1f);
        GetComponent<Rigidbody>().MovePosition(new Vector3(0, 0, 0.1f));
        transform.position = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().MovePosition(new Vector3(0, 0, 0f));
        if (path.Count > 1)
        {
            roadMeshCreator.path2 = GeneratePath(path);
            roadMeshCreator.UpdatePath();
        }

        transform.position = new Vector3(0, 0, 0.1f);
        GetComponent<Rigidbody>().MovePosition(new Vector3(0, 0, 0.1f));
        transform.position = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().MovePosition(new Vector3(0, 0, 0f));
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
}
