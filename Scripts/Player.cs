using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	public float jumpForce=10f;
	public Rigidbody2D rb;

	public SpriteRenderer sr;

	public Color colorCyan;
	public Color colorYellow;
	public Color colorMagenta;
	public Color colorPink;

	private string currentStringColor;

	private bool gameOver=false;

	public AudioSource coin;
	public AudioSource downer;

	public GameObject circlePrefab;
	public GameObject nestedCirclePrefab;
	public GameObject colorChangerPrefab;

	public Transform lastCircle;

	public ParticleSystem ps;

	private Color currentColor;


	void SetRandomColor(){
		int index = Random.Range (0, 4);
		switch (index) {
		case 0:
			sr.color=colorCyan;
			currentStringColor="Cyan";
			currentColor=colorCyan;
			break;
		case 1:
			sr.color=colorYellow;
			currentStringColor="Yellow";
			currentColor=colorYellow;
			break;
		case 2:
			sr.color=colorMagenta;
			currentStringColor="Magenta";
			currentColor=colorMagenta;
			break;
		case 3:
			sr.color=colorPink;
			currentStringColor="Pink";
			currentColor=colorPink;
			break;
			
		}
		
	}

    void Start()
    {
		SetRandomColor();
		var main=ps.main;
		main.startColor=currentColor;
		//baslangıcta da partıkul renk atamsı ıcın
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown ("Jump") || Input.GetMouseButtonDown (0) && !gameOver) {
			//rb.AddForce(Vector2.up * jumpForce); gravity 0 olmalı
			rb.velocity=Vector2.up* jumpForce;
		}
    }

	void OnTriggerEnter2D(Collider2D col){
		Debug.Log (col.tag);

		if (col.tag == "ColorChanger") {
			Instan();
			SetRandomColor();
			coin.Play();

			var main=ps.main;
			//burada main kullanma nedenim hangi degisken turunun donecegını bilemedigimizden
			main.startColor=currentColor;
			Destroy(col.gameObject);
			return;
			//eger return demezsen diger if e de girer
		}

		if (col.tag != currentStringColor && !gameOver) {
			downer.Play();
			gameOver=true;
			Invoke("Restart",1);
		}
	}

	void Restart(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);

	}

	void Instan(){
		GameObject prefabCircle;
		Transform colorChange;

		int a = Random.Range(0, 4);

		if (a == 0)
			prefabCircle = nestedCirclePrefab;
		else
			prefabCircle = circlePrefab;

		if (lastCircle.gameObject.tag.Equals ("Circle")) {
			colorChange = Instantiate (colorChangerPrefab, new Vector2 (0, lastCircle.position.y + 4), Quaternion.identity).transform;
			if (prefabCircle.tag.Equals ("Circle")) {
				lastCircle = Instantiate (prefabCircle, new Vector2 (0, colorChange.position.y + 4), Quaternion.identity).transform;
			} else {
				lastCircle = Instantiate (prefabCircle, new Vector2 (0, colorChange.position.y + 5), Quaternion.identity).transform;
			}
		
		} 
		else {
			colorChange = Instantiate (colorChangerPrefab, new Vector2 (0, lastCircle.position.y + 5), Quaternion.identity).transform;
			if (prefabCircle.tag.Equals ("Circle")) {
				lastCircle = Instantiate (prefabCircle, new Vector2 (0, colorChange.position.y + 4), Quaternion.identity).transform;
			} else {
				lastCircle = Instantiate (prefabCircle, new Vector2 (0, colorChange.position.y + 5), Quaternion.identity).transform;
			}

		}
	}

}
