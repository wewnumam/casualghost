using UnityEngine;

public class Cursor : MonoBehaviour {
	public CharacterController characterPlayer;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		transform.position = characterPlayer.mouseWorldSpace;
	}
}
