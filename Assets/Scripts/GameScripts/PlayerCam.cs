using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform camHolder;
    public Transform characterModel; // Referencia al modelo del personaje

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Obtener entrada del mouse
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        // Aplicar la rotación horizontal al objeto de la cámara
        yRotation += mouseX;

        // Aplicar la rotación vertical al eje X y limitar el ángulo de rotación
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Rotar el holder de la cámara (controla la orientación de la cámara)
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        // Rotar solo en el eje Y el objeto de orientación (controla el modelo del personaje)
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        // Aplicar rotación al modelo del personaje solo en el eje X
        characterModel.localRotation = Quaternion.Euler(0, yRotation, 0);
    }
}
