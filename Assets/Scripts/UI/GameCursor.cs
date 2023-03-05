using UnityEngine;

public class GameCursor : MonoBehaviour {
	public Texture2D defaultCursorTexture;
	public Texture2D onEnemyCursorTexture;

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
			CursorMode.Auto
		);
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.CompareTag(Tags.ENEMY)) {
			SetCursor(onEnemyCursorTexture);
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.gameObject.CompareTag(Tags.ENEMY)) {
			SetCursor(defaultCursorTexture);
		}
	}
}
