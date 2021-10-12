using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCollider : MonoBehaviour {
    public LineRenderer line;
    public BoxCollider lineCollider;
    // Use this for initialization
    void OnEnable() {
        Vector3 startPoint = line.GetPosition(0);
        Vector3 endPoint = line.GetPosition(1);
        float lineWidth = line.endWidth;
        float lineLength = Vector3.Distance(startPoint, endPoint);
        lineCollider.size = new Vector3(lineLength, lineWidth, 1f);
        Vector3 midPoint = (startPoint + endPoint) / 2;
        lineCollider.transform.position = midPoint;
        float angle = Mathf.Atan2((endPoint.z - startPoint.z), (endPoint.x - startPoint.x));
        angle *= Mathf.Rad2Deg;
        angle *= -1;
        lineCollider.transform.Rotate(0, angle, 0);
    }
	
	public void updateCollider()
    {
        Vector3 startPoint = line.GetPosition(0);
        Vector3 endPoint = line.GetPosition(1);
        float lineWidth = line.endWidth;
        float lineLength = Vector3.Distance(startPoint, endPoint);
        lineCollider.size = new Vector3(lineLength, 1f, 1f);
        Vector3 midPoint = (startPoint + endPoint) / 2;
        lineCollider.transform.position = midPoint;
        float angle = Mathf.Atan2((endPoint.z - startPoint.z), (endPoint.x - startPoint.x));
        angle *= Mathf.Rad2Deg;
        angle *= -1;
        lineCollider.transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}
