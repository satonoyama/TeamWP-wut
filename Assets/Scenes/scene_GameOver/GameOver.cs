using UnityEngine;
using UnityEngine.InputSystem;

public class GameOver : MonoBehaviour
{
	public GameOverController Controls { get; private set; }

	void Awake()
	{
		Controls = new GameOverController();

		Controls.GameOver.Enter.performed += TitleSceneTransfer;
	}

	void TitleSceneTransfer(InputAction.CallbackContext context)
	{
		SceneController.Instance.SceneChange("scene_Title");
	}

	private void OnEnable() => Controls.Enable();
	private void OnDisable() => Controls.Disable();
}
