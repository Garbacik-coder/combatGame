using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
public class BattleSystem : MonoBehaviour
{
    public int count = 0;

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

    private double healScore;
    private double nAttackScore;
    private double sAttackScore;
    private double sleepScore;

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
        state = BattleState.ENEMYTURN;
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

    IEnumerator MagicAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.magicDamage);
        //playerUnit.Stamina -= 5;
        playerUnit.Sleep(-7);
        playerHUD.SetStamina(playerUnit.currentStamina);
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "The attack is successful!";
        state = BattleState.ENEMYTURN;
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


        state = BattleState.ENEMYTURN;
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

        yield return new WaitForSeconds(1f);
        if (enemyUnit.currentStamina < 5)
        {
            dialogueText.text = enemyUnit.unitName + " sleeps!";
            StartCoroutine(EnemySleep());
        }
        else
        {
            enemyIntelligentMove();
        }

        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
        }
        else if (state == BattleState.LOST)
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
        playerUnit.Heal(555);

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



    public IEnumerator EnemyAttack()
    {
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "The enemy chose normal attack!";

        enemyUnit.Sleep(-5);
        enemyHUD.SetStamina(enemyUnit.currentStamina);
        state = BattleState.PLAYERTURN;
        yield return new WaitForSeconds(2f);
        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            PlayerTurn();
        }


    }

    public IEnumerator EnemyStrongAttack()
    {
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage * 2);

        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "The enemy chose strong attack!";

        enemyUnit.Sleep(-10);
        enemyHUD.SetStamina(enemyUnit.currentStamina);
        state = BattleState.PLAYERTURN;
        yield return new WaitForSeconds(2f);
        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            PlayerTurn();
        }


    }

    IEnumerator EnemyHeal()
    {
        enemyUnit.Heal(555);

        enemyUnit.Sleep(-5);
        enemyHUD.SetStamina(enemyUnit.currentStamina);
        dialogueText.text = "Enemy feels healed!";
        state = BattleState.PLAYERTURN;
        yield return new WaitForSeconds(2f);


        PlayerTurn();
    }

    IEnumerator EnemySleep()
    {
        enemyUnit.Sleep(10);

        enemyHUD.SetStamina(enemyUnit.currentStamina);
        dialogueText.text = "Enemy feels renewed stamina!";
        state = BattleState.PLAYERTURN;
        yield return new WaitForSeconds(2f);


        PlayerTurn();
    }

    private void enemyRandomMove()
    {
        dialogueText.text = enemyUnit.unitName + " attacks!";
        var move = Random.Range(1, 4);

        if (move == 1)
        {
            StartCoroutine(EnemySleep());
        }
        else if (move == 2)
        {
            StartCoroutine(EnemyAttack());
        }
        else if (move == 3)
        {
            StartCoroutine(EnemyStrongAttack());
        }
        else if (move == 4)
        {
            StartCoroutine(EnemyHeal());
        }
    }

    private void enemyIntelligentMove()
    {
        healScore = enemyUnit.currentHP / enemyUnit.maxHP * -1 + 1;
        sleepScore = enemyUnit.currentStamina / enemyUnit.maxStamina * -1 + 1;
        sAttackScore = playerUnit.currentHP / playerUnit.maxHP * 0.5 + enemyUnit.currentStamina / enemyUnit.maxStamina;
        nAttackScore = 1 - sAttackScore;

        var arr = new[] {healScore, sleepScore, nAttackScore, sAttackScore};
        arr = arr.OrderByDescending(n => n).ToArray();

        if (arr[0] == sleepScore)
        {
            StartCoroutine(EnemySleep());
        }
        else if (arr[0] == nAttackScore)
        {
            StartCoroutine(EnemyAttack());
        }
        else if (arr[0] == sAttackScore)
        {
            StartCoroutine(EnemyStrongAttack());
        }
        else if (arr[0] == healScore)
        {
            StartCoroutine(EnemyHeal());
        }
    }
}