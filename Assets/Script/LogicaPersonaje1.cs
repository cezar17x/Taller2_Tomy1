using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicaPersonaje1 : MonoBehaviour
{
    public float velocidadMovimiento = 5.0f;
    public float velocidadRotacion = 200.0f;
    private Animator anim;
    public float x, y;

    // Start is called before the first frame update
    void Start()
    {
        // Obtener el componente Animator solo una vez en el inicio
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Corregir el error tipográfico: Input en lugar de Inpud
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        // Rotación y movimiento
        transform.Rotate(0, x * Time.deltaTime * velocidadRotacion, 0);
        transform.Translate(0, 0, y * Time.deltaTime * velocidadMovimiento);

        anim.SetFloat("VelX", x) ;
        anim.SetFloat("VelY", y);

        // Aquí puedes añadir control sobre el Animator si es necesario
        // Por ejemplo: anim.SetFloat("Speed", y);
    }
}