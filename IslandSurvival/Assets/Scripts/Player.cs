using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Transform m_transform;
    CharacterController m_ch;

    float m_movSpeed = 5.0f;
    float m_gravity = 6.0f;

    public int m_life = 5;
    public float m_jumpSpeed = 10.0f;
    private Vector3 m_movDirection = Vector3.zero;

    Transform m_camTransform;
    Vector3 m_camRot;
    float m_camHeight = 1.4f;

    Transform m_muzzlepoint;
    public LayerMask m_layer;
    public Transform m_fx;
    public AudioClip m_audio;
    float m_shootTimeer = 0;

    void Start()
    {
        m_transform = this.transform;

        m_ch = this.GetComponent<CharacterController>();

        m_camTransform = Camera.main.transform;

        Vector3 pos = m_transform.position;

        pos.y += m_camHeight;

        m_camTransform.position = pos;
        m_camTransform.rotation = m_transform.rotation;
        m_camRot = m_camTransform.eulerAngles;

        Screen.lockCursor = true;

        m_muzzlepoint = m_camTransform.FindChild("M16/weapon/muzzlepoint").transform;
    }
    void Update()
    {
        if (m_life <= 0)
            return;
        Control();
        m_shootTimeer -= Time.deltaTime;
        if(Input.GetMouseButton(0)&&m_shootTimeer<=0)
        {
            m_shootTimeer = 0.1f;
            this.GetComponent<AudioSource>().PlayOneShot(m_audio);
            GameManager.Instance.SetAmmo(1);
            RaycastHit info;
            bool hit = Physics.Raycast(m_muzzlepoint.position,
                m_camTransform.TransformDirection(Vector3.forward), out info, 100, m_layer);
            if(hit)
            {
                if(info.transform.tag.CompareTo("enemy")==0)
                {
                    Enemy enemy = info.transform.GetComponent<Enemy>();
                    enemy.OnDamage(1);
                }
                Instantiate(m_fx, info.point, info.transform.rotation);
            }
        }
    }
    void Control()
    {
        float rh = Input.GetAxis("Mouse X");
        float rv = Input.GetAxis("Mouse Y");

        m_camRot.x -= rv;
        m_camRot.y += rh;
        m_camTransform.eulerAngles = m_camRot;

        Vector3 camRot = m_camTransform.eulerAngles;
        camRot.x = 0; camRot.z = 0;
        m_transform.eulerAngles = camRot;
        Vector3 pos = m_transform.position;

        pos.y += m_camHeight;
        m_camTransform.position = pos;

        float xm = 0, ym = 0, zm = 0;
        ym -= m_gravity * Time.deltaTime;

        if (Input.GetKey(KeyCode.W))
        {
            zm += m_movSpeed * Time.deltaTime;
        }

        else if (Input.GetKey(KeyCode.S))
        {
            zm -= m_movSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            xm -= m_movSpeed * Time.deltaTime;
        }

        else if (Input.GetKey(KeyCode.D))
        {
            xm += m_movSpeed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_movDirection.y = m_jumpSpeed;
        }
        m_movDirection.y -= m_gravity * Time.deltaTime;
        m_ch.Move(m_transform.TransformDirection(new Vector3(xm, ym, zm)));
        m_ch.Move(m_movDirection * Time.deltaTime);

    }
    public void OnDamage(int damage)
    {
        m_life -= damage;
        GameManager.Instance.SetLife(m_life);
        if(m_life<=0)
        {
            Screen.lockCursor = false;
        }
    }

}
