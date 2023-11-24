using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireball : MonoBehaviour
{
    bool canGo = false;
    public int dmg = 5;
    public Transform target;
    public float speed;
    public GameObject fbGraphic;
    public GameObject expl;
    public GameObject trail;
    //public GameObject indicator;
    public Transform end;
    Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        fbGraphic.SetActive(false);
        trail.SetActive(false);
        StartCoroutine(LoadFireball());
        StartCoroutine(LifeSpan());
        direction = (new Vector3(target.position.x, transform.position.y, target.position.z) - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = lookRotation;
        //indicator.SetActive(true);
        //indicator.transform.parent = null;
    }

    IEnumerator LoadFireball()
    {

        yield return new WaitForSeconds(0.2f);
        canGo = true;
        fbGraphic.SetActive(true);
        trail.SetActive(true);
    }

    IEnumerator LifeSpan()
    {

        yield return new WaitForSeconds(3f);
        Explode();
    }

    // Update is called once per frame
    void Update()
    {
        if(canGo)
        {
            MoveTowardsTarget();
        }
        
        if(Mathf.Approximately(end.transform.position.x, transform.position.x) && Mathf.Approximately(end.transform.position.x, transform.position.x))
        {
            Explode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<HealthSystem>().TakeDamage(dmg);
            Explode(true);
        }
        else if(!other.CompareTag("Enemy") && !other.CompareTag("Boss"))
        {
            Explode();
            
        }
    }
    private void MoveTowardsTarget()
    {
        // Calculate the direction towards the target
        

        // Move the projectile in the direction of the target
        transform.position += direction * speed * Time.deltaTime;
    }

    private void Explode(bool hit = false)
    {
        fbGraphic.SetActive(false);
        if (hit)
        {

            CameraShake.Instance.ShakeCamera(2f, 0.2f);
        }
        trail.transform.parent = null;
        Destroy(trail, 1f);
        //Destroy(indicator);
        Destroy(gameObject, 1);
    }
}
