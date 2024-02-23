using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPscript : MonoBehaviour
{
    private Vector3 thiscombo;
    private Vector3 thisglobal;

    public float velocidadeprafrente;

    private Renderer rend;

    [SerializeField] private GameObject player;


    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        TP();
    }


    void TP()
    {
        thiscombo = this.transform.localPosition;
        thisglobal = this.transform.position;
        if (Input.GetMouseButton(1))
        {
            this.gameObject.transform.localPosition = new Vector3(thiscombo.x, thiscombo.y, thiscombo.z += 1f * Time.deltaTime * velocidadeprafrente);
            rend.enabled = true;
        }

        this.transform.localPosition = new Vector3(thiscombo.x, thiscombo.y, Mathf.Clamp(this.transform.localPosition.z, 1.46f, 10.46f));

        if (Input.GetMouseButtonUp(1))
        {
            player.transform.position = new Vector3(thisglobal.x, player.transform.position.y, thisglobal.z);
            this.gameObject.transform.localPosition = new Vector3(0f, -0.736f, 1.46f);
            rend.enabled = false;
        }
    }

}
