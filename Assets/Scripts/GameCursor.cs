using UnityEngine;

public class GameCursor : MonoBehaviour {
	public Texture2D cursorTexture;

	// Start is called before the first frame update
	void Start() {
		Cursor.SetCursor(
			cursorTexture,
			new Vector2(
				cursorTexture.width / 2,
				cursorTexture.height / 2
			),
			CursorMode.Auto
		);
	}
}
