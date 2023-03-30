using UnityEngine;

public class GameCursor : MonoBehaviour {
	public static GameCursor Instance { get; private set; }

	[SerializeField] private Texture2D defaultCursorTexture;
	[SerializeField] private Texture2D onEnemyCursorTexture;
	[SerializeField] private Texture2D onBuildCursorTexture;

	void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

	void Start() {
		SetCursor(defaultCursorTexture);
	}

	void Update() {
		transform.position = UtilsClass.GetMouseWorldPosition();	
	}

	void SetCursor(Texture2D cursorTexture) {
		Cursor.SetCursor(
			cursorTexture,
			new Vector2(
				cursorTexture.width / 2,
				cursorTexture.height / 2
			),
			CursorMode.ForceSoftware
		);
	}

	public void SetDefaultCursor() => SetCursor(defaultCursorTexture);
	public void SetEnemyCursor() => SetCursor(onEnemyCursorTexture);
	public void SetBuildCursor() => SetCursor(onBuildCursorTexture);
}
