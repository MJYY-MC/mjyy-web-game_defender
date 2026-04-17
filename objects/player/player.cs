using Godot;

public class player : Node2D {
	/// <summary>
	/// 旋转速度（弧度/秒）
	/// </summary>
	[Export]
	public float RotationSpeed = 5.0f;

	/// <summary>
	/// 可旋转角度最小值
	/// </summary>
	[Export]
	public float RotationDegMin = -90f;

	/// <summary>
	/// 可旋转角度最大值
	/// </summary>
	[Export]
	public float RotationDegMax = 120f;

	Sprite readyShoot;
	public override void _Ready() {
		readyShoot = GetNode<Sprite>("readyShoot");
	}

	/// <summary>
	/// 是否正在蓄力
	/// </summary>
	bool readyShoot_isKeyDown = false;
	/// <summary>
	/// 蓄力射击蓄力时间（秒）
	/// </summary>
	float readyShootTime = 0f;
	/// <summary>
	/// 可蓄力的最大时间（秒）
	/// </summary>
	[Export] public float readyShootTimeMax = 1.5f;
	/// <summary>
	/// 蓄力射击的基础速度，蓄力时间乘以该值即为子弹初速度（像素/秒）
	/// </summary>
	[Export] public float readyShootBasicSpeed = 2000f;
	public override void _Process(float delta) {
		if(DataCore.Instance.gameData.GameState==GameData.GameStateEnum.running){
			{
				float rotateInput = 0f;

				if (Input.IsActionPressed("key_left") && RotationDegrees > RotationDegMin)
					rotateInput -= 1f;
				else {
					if (RotationDegrees < RotationDegMin)
						RotationDegrees = RotationDegMin;
				}
				if (Input.IsActionPressed("key_right") && RotationDegrees < RotationDegMax)
					rotateInput += 1f;
				else {
					if (RotationDegrees > RotationDegMax)
						RotationDegrees = RotationDegMax;
				}

				Rotation += rotateInput * RotationSpeed * delta;
			}
			{
				void updataPlayer() => 
					readyShoot.Modulate = new Color(
						readyShoot.Modulate.r, readyShoot.Modulate.g, readyShoot.Modulate.b,
						(readyShootTime / readyShootTimeMax)
						);

				if (readyShoot_isKeyDown) {
					float rst = readyShootTime + delta;
					if (rst > readyShootTimeMax) {
						readyShootTime = readyShootTimeMax;
						updataPlayer();
					}
					else if (rst == readyShootTimeMax) { }
					else {
						readyShootTime = rst;
						updataPlayer();
					}
				}
				else if (readyShootTime >= 0.1) {//至少蓄力0.1秒才允许射击
					float rst = readyShootTime;
					readyShootTime = 0f;
					ShootBullet(rst * readyShootBasicSpeed);
					updataPlayer();
				}
			}
		}
	}

	public override void _Input(InputEvent ie) {
		if (DataCore.Instance.gameData.GameState == GameData.GameStateEnum.running) {
			if (ie.IsActionPressed("key_up"))
				ShotBullet();
		}
		if (ie.IsActionPressed("key_down")) {
			readyShoot_isKeyDown = true;
		}else if (ie.IsActionReleased("key_down")) {
			readyShoot_isKeyDown = false;
		}
	}

	/// <summary>
	/// 子弹对象资源
	/// </summary>
	[Export]
	public PackedScene BulletObject { get; set; }

	/// <summary>
	/// 发射子弹
	/// </summary>
	/// <param name="bulletSpeed">子弹初速度（像素/秒）</param>
	private void ShootBullet(float bulletSpeed) {
		DataCore.Instance.gameData.scoreAddon.ChangeScore(new ScoreAddon.ScoreChangeData() {
			TextKey=1,
			ScoreChangeValue= -1,
			Many= 1,
		});//DataCore.Instance.gameData.Score -= 1;

		var bulletObj = (RigidBody2D)BulletObject.Instance();

		GetTree().CurrentScene.AddChild(bulletObj);
		bulletObj.GlobalPosition = GetNode<Position2D>("muzzle").GlobalPosition;

		float degRad = Mathf.Deg2Rad(RotationDegrees);
		bulletObj.LinearVelocity = 
			new Vector2(Mathf.Cos(degRad), Mathf.Sin(degRad)) 
			* bulletSpeed;
	}
	/// <summary>
	/// 发射霰弹
	/// </summary>
	private void ShotBullet() {
		float getRd(float rd,byte id) {
			switch (id) {
				case 0:
					return rd - 60;
				case 1:
					return rd - 40;
				case 2:
					return rd - 20;
				case 3:
					return rd + 20;
				case 4:
					return rd + 40;
				case 5:
					return rd + 60;
			}
			throw new System.Exception("错误的id");
		}
		DataCore.Instance.gameData.scoreAddon.ChangeScore(new ScoreAddon.ScoreChangeData() {
			TextKey = 2,
			ScoreChangeValue = -3,
			Many = 6,
		});//DataCore.Instance.gameData.Score -= 18;

		RigidBody2D[] bulletObjs = new RigidBody2D[6];

		for(byte i=0; i < bulletObjs.Length; i++) {
			bulletObjs[i] = (RigidBody2D)BulletObject.Instance();
			GetTree().CurrentScene.AddChild(bulletObjs[i]);
			bulletObjs[i].GlobalPosition = GetNode<Position2D>("muzzle").GlobalPosition;
			float degRad = Mathf.Deg2Rad(getRd(RotationDegrees, i));
			bulletObjs[i].LinearVelocity =
				new Vector2(Mathf.Cos(degRad), Mathf.Sin(degRad))
				* 1500;
		}
	}
}
