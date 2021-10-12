using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTerrain : MonoBehaviour {

    public GameObject pointPrefab;
    public GameObject beamPrefab;
    public GameObject character;
    public pointParent points;
    public HBeamParent hbeams;
    public VBeamParent vbeams;
    private int sizeX, sizeY;
    private float width;
    // Use this for initialization
    void OnEnable()
    {
        sizeX = 8;
        sizeY = 15;
        width = 15.0f;
        for (int j = 0; j < sizeY; j++)
        {
            pointParent[] pointList = points.points[j].points;
            HBeamParent[] hbeamList = hbeams.hbeams[j].hbeams;
            for (int i = 0; i < sizeX; i++)
            {
                GameObject point = pointList[i].gameObject;
                point.transform.position = new Vector3((i * width) - 50.0f, Random.Range(-25.0f, -15.0f), (j * width) - 20.0f);
            }
            for (int i = 0; i < sizeX - 1; i++)
            {
                GameObject beam = hbeamList[i].gameObject;
                beam.transform.position = new Vector3(0, 0, 0);
                beam.GetComponent<LineRenderer>().SetPosition(0, pointList[i].gameObject.transform.position);
                beam.GetComponent<LineRenderer>().SetPosition(1, pointList[i + 1].gameObject.transform.position);
            }
        }
        for (int j = 0; j < sizeY; j++)
        {
            VBeamParent[] vbeamList = vbeams.vbeams[j].vbeams;
            for (int i = 0; i < sizeX; i++)
            {
                GameObject beam = vbeamList[i].gameObject;
                beam.transform.position = new Vector3(0, 0, 0);
                beam.GetComponent<LineRenderer>().SetPosition(0, points.points[j].points[i].transform.position);
                if (sizeY - 1 == j)
                    beam.GetComponent<LineRenderer>().SetPosition(1, points.points[0].points[i].transform.position + new Vector3(0, 0, sizeY * width));
                else
                    beam.GetComponent<LineRenderer>().SetPosition(1, points.points[j + 1].points[i].transform.position);
            }
        }
    }

    void Update()
    {
        if (!character)
            return;
        for (int i = 0; i < sizeY; i++)
        {
            if (character.transform.position.z > points.points[i].points[0].transform.position.z + 20)
            {
                for (int j = 0; j < points.points[i].points.Length; j++)
                    points.points[i].points[j].transform.position = points.points[i].points[j].transform.position
                                                      + new Vector3(0, 0, sizeY * width);
                for (int j = 0; j < hbeams.hbeams[i].hbeams.Length; j++)
                    hbeams.hbeams[i].hbeams[j].transform.position = hbeams.hbeams[i].hbeams[j].transform.position
                                                      + new Vector3(0, 0, sizeY * width);
                for (int j = 0; j < vbeams.vbeams[i].vbeams.Length; j++)
                    vbeams.vbeams[i].vbeams[j].transform.position = vbeams.vbeams[i].vbeams[j].transform.position
                                                      + new Vector3(0, 0, sizeY * width);
            }
        }
    }

    public int getSizeY() { return sizeY; }
}
