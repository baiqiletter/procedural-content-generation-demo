using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public float life = 10;
	public float attack = 2f;
	public bool isBoss = false;
	private bool isPlat;
	private bool isObstacle;
	private Transform fallCheck;
	private Transform wallCheck;
	public LayerMask turnLayerMask;
	private Rigidbody2D rb;
	private GameObject player;

	private bool facingRight = true;
	private bool isDead = false;
	
	public float speed = 5f;
	public float dropRate = 0.4f;

	public bool isInvincible = false;
	private bool isHitted = false;

	void Awake () {
		fallCheck = transform.Find("FallCheck");
		wallCheck = transform.Find("WallCheck");
		rb = GetComponent<Rigidbody2D>();
		player = GameObject.Find("Player");
		life = Random.Range(life * 0.8f, life * 1.2f) * GameManager.Instance.enemyHealthScale;
		attack *= GameManager.Instance.enemyAttackScale;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (!isDead && life <= 0) {
			transform.GetComponent<Animator>().SetBool("IsDead", true);
			StartCoroutine(DestroyEnemy());
			GameManager.Instance.killNum++;
			isDead = true;
		}

		if (Vector3.Distance(player.transform.position, gameObject.transform.position) > 30f)
		{
			return;
		}

		isPlat = Physics2D.OverlapCircle(fallCheck.position, .2f, 1 << LayerMask.NameToLayer("Default"));
        //isPlat = Physics2D.OverlapCircle(fallCheck.position, .2f, 1 << LayerMask.NameToLayer("Scene"));
        isObstacle = Physics2D.OverlapCircle(wallCheck.position, .2f, turnLayerMask);
        //print("isPlat : " + isPlat);
        //print("isObstacle : " + isObstacle);

        if (!isHitted && life > 0 && Mathf.Abs(rb.velocity.y) < 0.5f)
		{
			if (isPlat && !isObstacle && !isHitted)
			{
				if (facingRight)
				{
					rb.velocity = new Vector2(-speed, rb.velocity.y);
				}
				else
				{
					rb.velocity = new Vector2(speed, rb.velocity.y);
				}
			}
			else
			{
				Flip();
			}
		}
	}

	void Flip (){
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void ApplyDamage(float damage, int direction) {
		//print("enemy.ApplyDamage");
		if (!isInvincible) 
		{
			//float direction = damage / Mathf.Abs(damage);
			damage = Mathf.Abs(damage);
			transform.GetComponent<Animator>().SetBool("Hit", true);
			life -= damage;
			rb.velocity = Vector2.zero;
			rb.AddForce(new Vector2(direction * 500f, 100f));
			StartCoroutine(HitTime());
		}
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player" && life > 0)
		{
			collision.gameObject.GetComponent<CharacterController2D>().ApplyDamage(attack, transform.position);
		}
	}

	IEnumerator HitTime()
	{
		isHitted = true;
		isInvincible = true;
		yield return new WaitForSeconds(0.1f);
		isHitted = false;
		isInvincible = false;
	}

	IEnumerator DestroyEnemy()
	{
		yield return new WaitForSeconds(0.25f);
		rb.velocity = new Vector2(0, rb.velocity.y);
        //yield return new WaitForSeconds(3f);
        yield return new WaitForSeconds(1f);
		DropItem();
		if (isBoss)
        {
			dropRate = .6f;
			DropItem();
		}
		GameManager.Instance.score += GameManager.Instance.killScore;
		Destroy(gameObject);
	}

	void DropItem()
    {
		float dropOrNot = Random.Range(0f, 1f);
		if (dropOrNot <= dropRate)	// drop
        {
			// select a drop item by all items' rare
			int pickupIndex = 0;
			float rareSum = 0;
			for (int i = 0; i < UnitManager.Instance.pickup.Length; i++)
            {
				rareSum += UnitManager.Instance.pickup[i].GetComponent<Pickup>().rare;
            }

			float select = Random.Range(0, rareSum);
			float tempSum = 0;
            for (int i = 0; i < UnitManager.Instance.pickup.Length; i++)
            {
				tempSum += UnitManager.Instance.pickup[i].GetComponent<Pickup>().rare;
				if (select < tempSum)
                {
					pickupIndex = i;
					break;
                }
            }

			Instantiate(UnitManager.Instance.pickup[pickupIndex], gameObject.transform.position, Quaternion.identity);
			
        }
	}
}
