using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	//public float dmgValue = 4;
	public GameObject throwableObject;
	public Transform attackCheck;
	private Rigidbody2D m_Rigidbody2D;
	public Animator animator;
	public bool canAttack = true;
	public bool isTimeToCheck = false;

	public GameObject cam;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.J) && canAttack)
		{
			canAttack = false;
			animator.SetBool("IsAttacking", true);
			StartCoroutine(AttackCooldown());
			DoDashDamage();
		}

		//if (Input.GetKeyDown(KeyCode.V))
		//{
		//	GameObject throwableWeapon = Instantiate(throwableObject, transform.position + new Vector3(transform.localScale.x * 0.5f, 0f), Quaternion.identity) as GameObject; 
		//	Vector2 direction = new Vector2(transform.localScale.x, 0);
		//	throwableWeapon.GetComponent<ThrowableWeapon>().direction = direction; 
		//	throwableWeapon.name = "ThrowableWeapon";
		//}
	}

	IEnumerator AttackCooldown()
	{
		yield return new WaitForSeconds(0.4f);
		canAttack = true;
	}

	public void DoDashDamage()
	{
		//GameManager.Instance.attack = Mathf.Abs(GameManager.Instance.attack);
		Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 1f);
		//print("player.DoDashDamage: collider count=" + collidersEnemies.Length);
		for (int i = 0; i < collidersEnemies.Length; i++)
		{
			if (collidersEnemies[i].gameObject.tag == "Enemy")
			{
				int direction = 1;
				if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
				{
					//GameManager.Instance.attack = -GameManager.Instance.attack;
					direction = -1;
				}
				//collidersEnemies[i].gameObject.SendMessage("ApplyDamage", GameManager.Instance.attack);
				collidersEnemies[i].gameObject.GetComponent<Enemy>().ApplyDamage(GameManager.Instance.attack, direction);
				cam.GetComponent<CameraFollow>().ShakeCamera();

				// show enemy info
				string s = collidersEnemies[i].GetComponent<Enemy>().life + " ( -" + GameManager.Instance.attack + " )";
				GameObject.Find("UI Controller").GetComponent<DisplayMessage>().SendToMessage(s);
			}
		}
	}
}
