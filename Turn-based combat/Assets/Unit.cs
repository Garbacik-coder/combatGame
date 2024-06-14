using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

	public string unitName;
	public int unitLevel;

	public int damage;
	public int magicDamage;
	public int maxHP;
	public int currentHP;
	public int stamina;
	public int currentStamina;
	public int maxStamina;

	public bool TakeDamage(int dmg)
	{
		currentHP -= dmg;

		if (currentHP <= 0)
			return true;
		else
			return false;
	}

	public void Heal(int amount)
	{
		currentHP += amount;
		if (currentHP > maxHP)
			currentHP = maxHP;
	}

	public void Sleep(int amount)
	{
		currentStamina += amount;
		if (currentStamina > maxStamina)
			currentStamina = maxStamina;
	}

}
