using UnityEngine;
using System.Collections;

public class CubeScript : MonoBehaviour
{

	public Texture2D[] runImgs;
	
	public Texture2D[] jumpImgs;

	public MeshRenderer mesh;

	private static int STATE_MOVE = 1;

	private static int STATE_JUMP = 2;

	private static int STATE_STOP = 3;

	private static int DIR_LEFT = 1;

	private static int DIR_RIGHT = 2;

	//人物朝向(左右)
	private int dir;

	//人物状态（行走、跳跃）
	private int state;
		
	private int frames;

	private Quaternion rotation;

	private Vector3 position;
	
	private Vector3 cameraPos;
	
	void Start ()
	{
		state = STATE_STOP;
		dir = DIR_RIGHT;
		rotation = new Quaternion (0, 0, 0, 0);
		Vector3 cp = new Vector3(-3.6f,0f,0f);
	}


	void Update ()
	{
		
		//修正人物位置（防止翻转、滚动）
		position = transform.position;
		position.z = -0.43f;
		transform.position = position;
		transform.rotation = rotation;
		//出界处理
		if(transform.position.y<-3)
		{
			position.x=-5f;
			position.y=1f;
			position.z=-0.43f;
			transform.position=position;
		}
		
		//人物状态处理
		//静止处理
		if (state == STATE_STOP) {
			if (dir == DIR_LEFT) {
				mesh.material.mainTexture = runImgs[5];
			} else if (dir == DIR_RIGHT) {
				mesh.material.mainTexture = runImgs[0];
			}
		//跳跃处理
		} else if (state == STATE_JUMP) {
			frames++;
			if (frames < 18) {
				transform.Translate (0, 0.12f, 0f);
				if (dir == DIR_LEFT) {
					transform.Translate (-0.05f, 0f, 0f);
					mesh.material.mainTexture = jumpImgs[9 + 4];
				} else if (dir == DIR_RIGHT) {
					transform.Translate (0.05f, 0f, 0f);
					mesh.material.mainTexture = jumpImgs[4];
				}
			}
			else{
				if (dir == DIR_LEFT) {
					transform.Translate (-0.05f, 0f, 0f);
				} else if (dir == DIR_RIGHT) {
					transform.Translate (0.05f, 0f, 0f);
				}	
			}
			
			if (frames>36) {
				state = STATE_STOP;
			}
		//移动处理
		} else if (state == STATE_MOVE) {
			if (dir == DIR_LEFT) {
				transform.Translate (-0.1f, 0f, 0f);
				mesh.material.mainTexture = runImgs[5 + frames % 5];
			} else if (dir == DIR_RIGHT) {
				transform.Translate (0.1f, 0f, 0f);
				mesh.material.mainTexture = runImgs[frames % 5];
			}
			if (Time.frameCount % 4 == 0) {
				frames++;
			}
			if (frames >= 5) {
				state = STATE_STOP;
			}
		}
		
		//按键控制
		if (state == STATE_STOP) {
			if (Input.GetKey (KeyCode.Space)) {
				state = STATE_JUMP;
				frames = 0;
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				state = STATE_MOVE;
				frames = 0;
				dir = DIR_LEFT;
			} else if (Input.GetKey (KeyCode.RightArrow)) {
				frames = 0;
				state = STATE_MOVE;
				dir = DIR_RIGHT;
			}
		}
	}
	
}
