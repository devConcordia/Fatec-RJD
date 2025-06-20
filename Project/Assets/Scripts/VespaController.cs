using UnityEngine;

public class VespaController : Character
{
	
	private Animator animator;
	
	private int direction = 1;
	private float scale = 1.5f;
	
	private float shootCooldownTime = 2f;
	private float shootNextAvailableTime  = 0f;
	
	[SerializeField] public GameObject prision;
	
	[SerializeField] public GameObject target;
	[SerializeField] public GameObject PrefabProjectile;
	[SerializeField] public float projectileSpeed = 5f;
	
	[SerializeField] public float distanceBeginShoot = 6f;
	
	/// audios
	[SerializeField] public AudioClip rangedSound;
	[SerializeField] public AudioClip damageSound;
	
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
		
        animator = GetComponent<Animator>();
	
    }
	
	
	
	public override void takeDamage( int points = 1 ) {
		
		base.takeDamage( points );
		
		SoundManager.Play(damageSound, 2f);
		
	}
	
	public override void onDestroy() {
		
		prision.SetActive(false);
		
	}
	
    void FixedUpdate() {
        
		float delta = Vector3.Distance( target.transform.position, transform.position );
		
		direction = ( transform.position.x < target.transform.position.x )? -1 : 1;
		
		/// inverte a direção do personagem que o personagem está olhando
		transform.localScale = new Vector3( scale*direction, scale, scale );
		
		if( delta < distanceBeginShoot ) {
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
			if( stateInfo.IsName("Idle") ) {
				Shoot();
			}
		}
		
    }
	
	void Shoot() {
		
		if( Time.time > shootNextAvailableTime ) {
		
			animator.SetTrigger("Atk1");
			
			/// AudioClip clip, Vector3 position, float volume
		//	AudioSource.PlayClipAtPoint(rangedSound, transform.position, 1f);
			SoundManager.Play(rangedSound, 2f);
			
			GameObject projectile = Instantiate(PrefabProjectile, transform.position, Quaternion.identity);
			
			/// ignora colisao com o proprio emissor
			Physics2D.IgnoreCollision( projectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
			
			Vector2 dir = (target.transform.position - transform.position).normalized;
			
			projectile.GetComponent<Rigidbody2D>().linearVelocity = dir * projectileSpeed;
		
            // Define o próximo tempo disponível
            shootNextAvailableTime = Time.time + shootCooldownTime;
			
		}
		
	}
	
}
