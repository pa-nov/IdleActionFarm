using UnityEngine;

public class FarmlandSystem : MonoBehaviour
{
	public static FarmlandSystem Instance;
	public int SizeX, SizeY, SizeSpace;
	public GameObject FarmlandPlace, FarmlandPrefab;

	public int[] FarmlandTimer;
	private GameObject[] FarmlandObject;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		FarmlandTimer = new int[SizeX * SizeY];
		FarmlandObject = new GameObject[SizeX * SizeY];
		for (int x = 0; x < SizeX; x++)
		{
			for (int y = 0; y < SizeY; y++)
			{
				int i = x * SizeY + y;
				FarmlandTimer[i] = 0;
				Vector3 newPosition = FarmlandPlace.transform.position + new Vector3(x + x * SizeSpace, 0, y + y * SizeSpace);
				FarmlandObject[i] = Instantiate(FarmlandPrefab, newPosition, FarmlandPrefab.transform.rotation, FarmlandPlace.transform);
				FarmlandObject[i].GetComponent<Farmland>().Id = i;
				FarmlandObject[i].name = "Farmland-" + i;
			}
		}
	}

	private void FixedUpdate()
	{
		for (int i = 0; i < FarmlandTimer.Length; i++)
		{
			if (FarmlandTimer[i] < 500)
			{
				FarmlandTimer[i] += 1;
			}
			else
			{
				if (!FarmlandObject[i].GetComponent<Farmland>().IsFull)
				{
					FarmlandObject[i].GetComponent<Farmland>().SetFull(true);
				}
			}
		}
	}

	public void Mow(int[] id)
	{
		foreach (int i in id)
		{
			if (FarmlandObject[i].GetComponent<Farmland>().IsPlayer)
			{
				FarmlandTimer[i] = 0;
				FarmlandObject[i].GetComponent<Farmland>().SetFull(false);
			}
		}
	}
}