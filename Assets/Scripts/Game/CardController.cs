using System.Threading.Tasks;
using UnityEngine;

public class CardController : MonoBehaviour
{
    private Vector2 startingPos;
    private bool ascending;
    [SerializeField] private float range = 0.5f;
    [SerializeField] private float velocity = 1.5f;

    [SerializeField]private ParticleSystem particles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        startingPos = this.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (ascending) {

            this.transform.position += new Vector3(0, velocity * Time.deltaTime, 0);

            if (this.transform.position.y > (startingPos.y + range)) ascending = !ascending;

        } else {

            this.transform.position -= new Vector3(0, velocity * Time.deltaTime, 0);

            if (this.transform.position.y < (startingPos.y - range)) ascending = !ascending;

        }

        this.transform.Rotate(0, velocity * 100 * Time.deltaTime, 0);

    }

    public void PreDestroy()
    {
        
        particles.transform.SetParent(null);

        particles.Play();


        Destroy(gameObject);

        Destroy(particles.gameObject, particles.main.duration + particles.main.startLifetime.constantMax);

    }

}
