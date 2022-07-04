using UnityEngine;
using UnityEngine.InputSystem;

public class GameClear : MonoBehaviour
{
	public GameClearController Controls { get; private set; }

	private void Awake()
	{
		Controls = new GameClearController();

		Controls.GameClear.Enter.performed += TitleSceneTransfer;
	}

    void TitleSceneTransfer(InputAction.CallbackContext context)
	{
		SceneController.Instance.SceneChange("scene_Title");
	}

	private void OnEnable() => Controls.Enable();
	private void OnDisable() => Controls.Disable();
}
