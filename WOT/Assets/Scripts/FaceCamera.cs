using UnityEngine;

public class FaceCamera : MonoBehaviour
{

    [SerializeField] Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (cam)
        {
            Vector3 direction = transform.position - cam.transform.position;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

    public void StartLookAt()
    {
        cam = Camera.main;
    }
}
