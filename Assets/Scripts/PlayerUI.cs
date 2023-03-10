using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI Instance;

	public static int BackpackMax, Backpack, Coins;
    public int MaxBackpack;
    public GameObject Coin, StartPosition, CoinsImage;

    public Text BackpackText, CoinsText;
    public Image BackpackImage;

    private float CoinsTemp;

	private void Awake()
	{
        Instance = this;
		BackpackMax = MaxBackpack;
	}

	private void Start()
    {
		Coins = PlayerPrefs.GetInt("Coins");
		CoinsTemp = Coins;
	}

    private void Update()
    {
        BackpackText.text = Backpack + "/" + BackpackMax;
        BackpackImage.fillAmount = Mathf.Lerp(BackpackImage.fillAmount, (float)Backpack / BackpackMax, 10f * Time.deltaTime);

		CoinsTemp = Mathf.Lerp(CoinsTemp, Coins, 10f * Time.deltaTime);
		CoinsText.text = Mathf.RoundToInt(CoinsTemp).ToString();
		if (Mathf.RoundToInt(CoinsTemp) != Coins)
		{
			CoinsText.gameObject.transform.localPosition = new Vector3(Random.Range(-346, -326), Random.Range(-10, 10), 0);
		}
		else
		{
			CoinsText.gameObject.transform.localPosition = new Vector3(-336, 0, 0);
		}
    }

    public void AddCoins(int amount)
    {
        StartCoroutine(AddCoinsAnim(1, amount));
	}

	private IEnumerator AddCoinsAnim(int Seconds, int amount)
	{
        yield return new WaitForSeconds(0.75f);
		GameObject newCoin = Instantiate(Coin, CoinsImage.transform.parent);
		newCoin.transform.position = Camera.main.WorldToScreenPoint(StartPosition.transform.position);
		newCoin.transform.DOMove(CoinsImage.transform.position, Seconds);
		yield return new WaitForSeconds(Seconds);
		Destroy(newCoin);
		Coins += amount;
        PlayerPrefs.SetInt("Coins", Coins);
	}
}