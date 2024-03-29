using UnityEngine;

public class MoveWith : MonoBehaviour
{
    public Transform target;
    
    public float speed = 1;
    
    Vector3 targetLastPosition;
    
    private void Update()
    {
        if (!target) return;
        
        Vector3 targetPosition = target.position;
        transform.Translate((targetPosition - targetLastPosition) * speed);
        targetLastPosition = targetPosition;
    }
}