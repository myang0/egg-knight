using UnityEngine;
using TMPro;

public class WalletText : MonoBehaviour {
  private TextMeshProUGUI _display;

  private void Awake() {
    _display = gameObject.GetComponent<TextMeshProUGUI>();

    GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
    PlayerWallet wallet = playerObject?.GetComponent<PlayerWallet>();

    _display.text = (wallet != null) ? $"{wallet.GetBalance()}" : "0";

    PlayerWallet.OnBalanceChange += HandleBalanceChange;
  }

  private void HandleBalanceChange(object sender, BalanceChangeEventArgs e) {
    _display.text = $"{e.newBalance}";
  }
}
