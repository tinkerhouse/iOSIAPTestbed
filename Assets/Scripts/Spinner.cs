using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour
{
	public enum State
	{
		Default = 0,
		Spinning
	}

	private Texture2D _image;
	private int _angle;
	private Vector2 _center;
	private int _speed = 4;
	private State _state = State.Default;
	private GUISkin _guiSkin;

	void Awake()
	{
		this._image = (Texture2D)Resources.Load("Textures/war-room-spinner");
		this._guiSkin = (GUISkin)ScriptableObject.CreateInstance("GUISkin");
		this._center = new Vector2(Screen.width / 2, Screen.height / 2);
	}

	void OnGUI()
	{
		GUI.skin = this._guiSkin;
		switch (this._state)
		{
			case State.Spinning:
				GUIUtility.RotateAroundPivot(this._angle, this._center);
				GUI.Label(new Rect(Screen.width / 2 - this._image.width / 2, Screen.height / 2 - this._image.height / 2, this._image.width, this._image.height), this._image);
				this._angle += this._speed;
				break;

			default:
				break;
		}
	}

	public void Show()
	{
		this._angle = 0;
		this._state = State.Spinning;
	}

	public void Hide()
	{
		this._state = State.Default;
	}

}
