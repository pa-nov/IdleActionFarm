using UnityEngine;

public class Farmland : MonoBehaviour
{
	public GameObject Null, Full, HayBale, FX, Current;
	public int Id;
	public bool IsFull, IsPlayer;

	public void SetFull(bool isFull)
	{
		IsFull = isFull;
		Destroy(Current);

		if (IsFull)
		{
			Current = Instantiate(Full, transform.position, Full.transform.rotation, this.transform);
		}
		else
		{
			Current = Instantiate(Null, transform.position, Null.transform.rotation, this.transform);
			Vector3 RandomPosition = new Vector3(Random.Range(-0.3f, 0.3f), 0.5f, Random.Range(-0.3f, 0.3f));
			Instantiate(HayBale, transform.position + RandomPosition, transform.rotation, this.transform);
			FX.GetComponent<ParticleSystem>().Play();
		}
		Current.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			IsPlayer = true;
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			IsPlayer = false;
		}
	}
}