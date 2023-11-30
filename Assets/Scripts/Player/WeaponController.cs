using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private GameObject bulletObj;
    private Transform fireTransform;    

    private Camera cameraMain;

    [SerializeField]
    private float fireSpeed = 20;

    private void Start()
    {
        cameraMain = Camera.main;

        spriteRenderer = GetComponent<SpriteRenderer>();
        bulletObj = Resources.Load<GameObject>("Prefab/Bullet");
        fireTransform = transform.Find("FirePoint");
    }

    private void Update()
    {
        LookAt2D();
        Fire();
    }

    /// <summary>
    /// 武器跟隨屬標旋轉
    /// </summary>
    void LookAt2D()
    {
        Vector3 dir = cameraMain.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if(transform.rotation.eulerAngles.z >= 90 && transform.rotation.eulerAngles.z <= 270)
        {
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }
    }

    void Fire()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = cameraMain.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            float fireAngle = Vector3.Angle(mousePos - transform.position, Vector2.up);
            if(mousePos.x > transform.position.x)
            {
                fireAngle = -fireAngle;
            }

            GameObject bullet = Instantiate(bulletObj, fireTransform.position, Quaternion.identity);
            bullet.transform.eulerAngles = new Vector3(0, 0, fireAngle + 90);
            Vector2 v2 = (mousePos - transform.position).normalized * fireSpeed;
            bullet.GetComponent<Rigidbody2D>().velocity = v2;

            
        }
    }
}
