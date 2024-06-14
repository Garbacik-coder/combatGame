using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
public int count = 0;
public class BattleSystem : MonoBehaviour
{

	public GameObject playerPrefab;
	public GameObject enemyPrefab;

	public Transform playerBattleStation;
	public Transform enemyBattleStation;

	Unit playerUnit;
	Unit enemyUnit;

	public Text dialogueText;

	public BattleHUD playerHUD;
	public BattleHUD enemyHUD;

	public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
		state = BattleState.START;
		StartCoroutine(SetupBattle());
    }

	IEnumerator SetupBattle()
	{
		GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
		playerUnit = playerGO.GetComponent<Unit>();

		GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
		enemyUnit = enemyGO.GetComponent<Unit>();

		dialogueText.text = "A wild " + enemyUnit.unitName + " approaches...";

		playerHUD.SetHUD(playerUnit);
		enemyHUD.SetHUD(enemyUnit);

		yield return new WaitForSeconds(2f);

		state = BattleState.PLAYERTURN;
		PlayerTurn();
	}

	IEnumerator PlayerAttack()
	{
		bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

		enemyHUD.SetHP(enemyUnit.currentHP);
		dialogueText.text = "The attack is successful!";
		//playerUnit.Stamina -= 5;
		playerUnit.Sleep(-5);
		playerHUD.SetStamina(playerUnit.currentStamina);
		state = BattleState.ENEMYTURN
		yield return new WaitForSeconds(2f);

		if(isDead)
		{
			state = BattleState.WON;
			EndBattle();
		} else
		{
			StartCoroutine(EnemyTurn());
		}
	}

	IEnumerator MagicAttack()
	{
		bool isDead = enemyUnit.TakeDamage(playerUnit.magicDamage);
		//playerUnit.Stamina -= 5;
		playerUnit.Sleep(-7);
		playerHUD.SetStamina(playerUnit.currentStamina);
		enemyHUD.SetHP(enemyUnit.currentHP);
		dialogueText.text = "The attack is successful!";
		state = BattleState.ENEMYTURN
		yield return new WaitForSeconds(2f);

		if (isDead)
		{
			state = BattleState.WON;
			EndBattle();
		}
		else
		{
			StartCoroutine(EnemyTurn());
		}
	}
	
	IEnumerator StrongAttack()
	{
		bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
		//playerUnit.Stamina -= 5;
		playerUnit.Sleep(-10);
		playerHUD.SetStamina(playerUnit.currentStamina);

		enemyHUD.SetHP(enemyUnit.currentHP);
		dialogueText.text = "The attack is successful!";
		

		state = BattleState.ENEMYTURN
		yield return new WaitForSeconds(2f);

		if (isDead)
		{
			state = BattleState.WON;
			EndBattle();
		}
		else
		{
			StartCoroutine(EnemyTurn());
		}
	}

	IEnumerator EnemyTurn()
	{
		dialogueText.text = enemyUnit.unitName + " attacks!";

		yield return new WaitForSeconds(1f);

		bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

		playerHUD.SetHP(playerUnit.currentHP);

		yield return new WaitForSeconds(1f);

		if(isDead)
		{
			state = BattleState.LOST;
			EndBattle();
		} else
		{
			state = BattleState.PLAYERTURN;
			PlayerTurn();
		}

	}

	void EndBattle()
	{
		if(state == BattleState.WON)
		{
			dialogueText.text = "You won the battle!";
		} else if (state == BattleState.LOST)
		{
			dialogueText.text = "You were defeated.";
		}
	}

	void PlayerTurn()
	{
		dialogueText.text = "Choose an action:";
	}

	IEnumerator PlayerHeal()
	{
		playerUnit.Heal(5);

		playerHUD.SetHP(playerUnit.currentHP);
		dialogueText.text = "You feel renewed strenght!";
		state = BattleState.ENEMYTURN;
		yield return new WaitForSeconds(2f);

		
		StartCoroutine(EnemyTurn());
	}

	IEnumerator PlayerSleep()
	{
		playerUnit.Sleep(10);

		playerHUD.SetStamina(playerUnit.currentStamina);
		dialogueText.text = "You feel renewed stamina!";
		state = BattleState.ENEMYTURN;
		yield return new WaitForSeconds(2f);


		StartCoroutine(EnemyTurn());
	}

	public void OnAttackButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;

		StartCoroutine(PlayerAttack());
	}

	public void OnHealButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;

		StartCoroutine(PlayerHeal());
	}

	public void OnMagicAttackButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;

		StartCoroutine(MagicAttack());
	}

	public void OnStrongAttackButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;

		StartCoroutine(StrongAttack());
	}

	public void OnSleepButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;

		StartCoroutine(PlayerSleep());
	}

}
