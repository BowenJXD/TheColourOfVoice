using UnityEngine;

public class FaceTarget : MonoBehaviour
{
    public Transform target;
    
    void Update()
    {
        if (target)
        {
            transform.LookAt(target);
        }
    }
}