using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Up, Down, Left, Right, Forward, Back
}
[System.Serializable]
public class JointInfo
{
    public Transform jointTrans;
    public Direction direction;
    public Vector3 diflectionAxis;
    public float diflectionAngle;
}
public class ModelAvator : MonoBehaviour
{
    // Start is called before the first frame update
    public UDPReceive udpReceive;
    public GameObject[] bodyPoints;
    void Start()
    {
        bodyOriginCenter = bodyCenter.position;
    }

    public JointInfo headJoint, bodyJoint;
    public JointInfo[] leftHands, leftLegs;
    public JointInfo[] rightHands, rightLegs;
    public Transform bodyCenter;
    Vector3 bodyOriginCenter;

    Vector3 originKuaPos;
    public Vector3 kuaOffset;
    public bool isLerp;
    public float lerpSpeed;
    // Update is called once per frame
    void Update()
    {
        if (udpReceive.isReceive)
        {
            string data = udpReceive.data;

            // data = data.Remove(0, 1);
            // data = data.Remove(data.Length - 1, 1);
            if (data != null && data != "")
            {
                if (originKuaPos == Vector3.zero)
                {
                    originKuaPos = bodyPoints[23].transform.position;
                }
                kuaOffset = bodyPoints[23].transform.position - originKuaPos;

                bodyCenter.position = new Vector3(bodyOriginCenter.x, bodyOriginCenter.y + kuaOffset.y, bodyOriginCenter.z);

                string[] points = data.Split(',');

                //0        1*3      2*3
                //x1,y1,z1,x2,y2,z2,x3,y3,z3

                for (int i = 0; i <= 32; i++)
                {

                    float x = float.Parse(points[0 + (i * 3)]) / 100;
                    float y = float.Parse(points[1 + (i * 3)]) / 100;
                    float z = float.Parse(points[2 + (i * 3)]) / 300;

                    bodyPoints[i].transform.localPosition = new Vector3(x, y, z);

                }
                CalcDirection(headJoint, ((bodyPoints[3].transform.position - bodyPoints[7].transform.position).normalized));

                Vector3 pos1 = bodyPoints[11].transform.position;
                Vector3 pos2 = bodyPoints[12].transform.position;
                pos1.y = 0;
                pos2.y = 0;
                Vector3 bodyDir = Quaternion.AngleAxis(90, Vector3.up) * (pos1 - pos2).normalized;
                CalcDirection(bodyJoint, bodyDir);

                //11,13,15
                //12,14,16
                CalcDirection(leftHands[0], (bodyPoints[13].transform.position - bodyPoints[11].transform.position).normalized);
                CalcDirection(leftHands[1], (bodyPoints[15].transform.position - bodyPoints[13].transform.position).normalized);

                CalcDirection(rightHands[0], (bodyPoints[14].transform.position - bodyPoints[12].transform.position).normalized);
                CalcDirection(rightHands[1],(bodyPoints[16].transform.position - bodyPoints[14].transform.position).normalized);

                ////23,25,27,31
                ////24,26,28,32
                CalcDirection(leftLegs[0], (bodyPoints[25].transform.position - bodyPoints[23].transform.position).normalized);
                CalcDirection(leftLegs[1], (bodyPoints[27].transform.position - bodyPoints[25].transform.position).normalized);
                CalcDirection(leftLegs[2], (bodyPoints[31].transform.position - bodyPoints[27].transform.position).normalized);
                //leftLegs[0].up = (bodyPoints[23].transform.position - bodyPoints[25].transform.position).normalized;
                //leftLegs[1].up = (bodyPoints[25].transform.position - bodyPoints[27].transform.position).normalized;
                //leftLegs[2].forward = -(bodyPoints[27].transform.position - bodyPoints[31].transform.position).normalized;

                CalcDirection(rightLegs[0], (bodyPoints[26].transform.position - bodyPoints[24].transform.position).normalized);
                CalcDirection(rightLegs[1],(bodyPoints[28].transform.position - bodyPoints[26].transform.position).normalized);
                CalcDirection(rightLegs[2], (bodyPoints[32].transform.position - bodyPoints[28].transform.position).normalized);
                //rightLegs[0].up = (bodyPoints[24].transform.position - bodyPoints[26].transform.position).normalized;
                //rightLegs[1].up = (bodyPoints[26].transform.position - bodyPoints[28].transform.position).normalized;
                //rightLegs[2].forward = -(bodyPoints[28].transform.position - bodyPoints[32].transform.position).normalized;
            }
        }
    }
    public void CalcDirection(JointInfo jointInfo,Vector3 direction)
    {
        if(jointInfo.diflectionAxis!=Vector3.zero)
        {
            direction = Quaternion.Euler(jointInfo.diflectionAxis)*direction;
        }
        switch (jointInfo.direction)
        {
            case Direction.Up:
                if (isLerp)
                {
                    jointInfo.jointTrans.rotation = Quaternion.Lerp(jointInfo.jointTrans.rotation, Quaternion.LookRotation(direction) * Quaternion.Euler(90,0, 0), Time.deltaTime * lerpSpeed);
                }
                else
                {
                    jointInfo.jointTrans.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
                }
                break;
            case Direction.Down:
                if (isLerp)
                {
                    jointInfo.jointTrans.rotation = Quaternion.Lerp(jointInfo.jointTrans.rotation, Quaternion.LookRotation(direction) * Quaternion.Euler(-90, 0, 0), Time.deltaTime * lerpSpeed);
                }
                else
                {
                    jointInfo.jointTrans.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(-90, 0, 0);
                }
                break;
            case Direction.Left:
                if (isLerp)
                {
                    jointInfo.jointTrans.rotation = Quaternion.Lerp(jointInfo.jointTrans.rotation, Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0), Time.deltaTime * lerpSpeed);
                }
                else
                {
                    jointInfo.jointTrans.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0);
                }
                break;
            case Direction.Right:
                if (isLerp)
                {
                    jointInfo.jointTrans.rotation = Quaternion.Lerp(jointInfo.jointTrans.rotation, Quaternion.LookRotation(direction) * Quaternion.Euler(0, -90, 0), Time.deltaTime * lerpSpeed);
                }
                else
                {
                    jointInfo.jointTrans.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -90, 0);
                }
                break;
            case Direction.Forward:
                if (isLerp)
                {
                    jointInfo.jointTrans.rotation = Quaternion.Lerp(jointInfo.jointTrans.rotation, Quaternion.LookRotation(direction), Time.deltaTime * lerpSpeed);
                }
                else
                {
                    jointInfo.jointTrans.rotation = Quaternion.LookRotation(direction);
                }
                break;
            case Direction.Back:
                if (isLerp)
                {
                    jointInfo.jointTrans.rotation = Quaternion.Lerp(jointInfo.jointTrans.rotation, Quaternion.LookRotation(-direction), Time.deltaTime * lerpSpeed);
                }
                else
                {
                    jointInfo.jointTrans.rotation = Quaternion.LookRotation(-direction);
                }
                break;
        }
    }
}
