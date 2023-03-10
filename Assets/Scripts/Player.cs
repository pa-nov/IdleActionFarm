using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator PlayerAnimation;
	public GameObject HayBalesObject, HayBalesStart, FarmSeller, Sickle;

	private bool IsSelling;
	private int MowTimer, FullFarmlands, BackpackTemp, SellingTimer;
	private int[] Farmlands = { };
	private GameObject[] HayBales = { };

	private void Update()
    {
        if (Controls.Vector != Vector2.zero)
		{
			transform.position += new Vector3(Controls.Vector.x * Time.deltaTime * 2, 0, Controls.Vector.y * Time.deltaTime * 2);
			transform.LookAt(new Vector3(transform.position.x + Controls.Vector.x * 30, 0, transform.position.y + Controls.Vector.y * 30));
            PlayerAnimation.SetFloat("Speed", Controls.Vector.magnitude);
		}
		if (Controls.Vector == Vector2.zero && (FullFarmlands > 0 || MowTimer > 19))
		{
			Sickle.SetActive(true);
			PlayerAnimation.Play("Mow");
		}
		else
		{
			Sickle.SetActive(false);
			if (Controls.Vector != Vector2.zero)
			{
				PlayerAnimation.Play("Walking");
			}
			else
			{
				PlayerAnimation.Play("Idle");
			}
		}
		if (PlayerUI.Backpack != BackpackTemp)
		{
			int addSize = PlayerUI.Backpack - BackpackTemp;
			if (addSize > 0)
			{
				Array.Resize(ref HayBales, HayBales.Length + addSize);
				for (int i = 0; i < addSize; i++)
				{
					int id = HayBales.Length - addSize + i;

					HayBales[id] = Instantiate(HayBalesObject, HayBalesStart.transform);
					HayBales[id].transform.localPosition = new Vector3(0, id * 0.15f, 0);
				}
			}
			else
			{
				for (int i = 0; i < -addSize; i++)
				{
					HayBales[HayBales.Length - 1].transform.DOMove(FarmSeller.transform.position, 1);
					StartCoroutine(DestroyObject(HayBales[HayBales.Length - 1], 1));
					Array.Resize(ref HayBales, HayBales.Length - 1);
				}
			}

			BackpackTemp = PlayerUI.Backpack;
		}
    }
	private void FixedUpdate()
	{
		if (Farmlands.Length > 0)
		{
			FullFarmlands = 0;
			foreach (int id in Farmlands)
			{
				if (FarmlandSystem.Instance.FarmlandTimer[id] > 499)
				{
					FullFarmlands++;
				}
			}
			if (Controls.Vector == Vector2.zero && (FullFarmlands > 0 || MowTimer > 19))
			{
				MowTimer++;
				if (MowTimer == 20)
				{
					FarmlandSystem.Instance.Mow(Farmlands);
				}
				if (MowTimer > 50)
				{
					MowTimer = 0;
				}
			}
		}

		if (IsSelling)
		{
			SellingTimer--;
			if (SellingTimer < 0)
			{
				SellingTimer = 25;
				if (PlayerUI.Backpack > 0)
				{
					PlayerUI.Backpack--;
					PlayerUI.Instance.AddCoins(15);
				}
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Farm"))
		{
			IsSelling = true;
		}

		if (other.CompareTag("Farmland"))
		{
			int OtherId = other.gameObject.GetComponent<Farmland>().Id;

			Array.Resize(ref Farmlands, Farmlands.Length + 1);
			Farmlands[Farmlands.Length - 1] = OtherId;
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Farm"))
		{
			IsSelling = false;
			SellingTimer = 0;
		}

		if (other.CompareTag("Farmland"))
		{
			int OtherId = other.gameObject.GetComponent<Farmland>().Id;

			Farmlands = Farmlands.Where(val => val != OtherId).ToArray();
			if (Farmlands.Length < 1)
			{
				MowTimer = 0;
				FullFarmlands = 0;
			}
		}
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("HayBale"))
		{
			if (PlayerUI.Backpack < PlayerUI.BackpackMax)
			{
				PlayerUI.Backpack++;
				Destroy(collision.gameObject);
			}
		}
	}

	IEnumerator DestroyObject(GameObject gameObject, int Seconds)
	{
		yield return new WaitForSeconds(Seconds);
		Destroy(gameObject);
	}
}