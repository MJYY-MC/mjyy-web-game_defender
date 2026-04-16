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
	/// <summary>
	/// 当前按键松开的时间，用于容错判断
	/// </summary>
	float readyShootTime_shootTolerance_pressTime = 0f;
	/// <summary>
	/// 按键松开后的容错时间，避免帧数不稳定时判断按键松开
	/// </summary>
	float readyShootTime_shootTolerance_pressTimeMax; //= 0.05f;
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

				if (Input.IsActionPressed("key_down")) {
					readyShootTime_shootTolerance_pressTime = 0f;

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
				else {
					if (readyShootTime != 0) {
						readyShootTime_shootTolerance_pressTime += delta;
						readyShootTime_shootTolerance_pressTimeMax = delta*4;//根据帧时间动态调整容错时间
						if (readyShootTime_shootTolerance_pressTime > readyShootTime_shootTolerance_pressTimeMax) {
							float rst = readyShootTime;
							readyShootTime = 0f;
							ShootBullet(rst * readyShootBasicSpeed);
							updataPlayer();
						}
					}
				}
			}
		}
	}

	public override void _Input(InputEvent ie) {
		if (DataCore.Instance.gameData.GameState == GameData.GameStateEnum.running) {
			if (ie.IsActionPressed("key_up"))
				ShootBullet(1000);
			/*else if (ie.IsActionPressed("key_down"))
				ShootBullet(500);*/
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
		if (BulletObject == null && OS.IsDebugBuild()) {
			GD.PrintErr("错误: BulletObject为null");
			return;
		}

		var bulletObj = (RigidBody2D)BulletObject.Instance();

		GetTree().CurrentScene.AddChild(bulletObj);
		bulletObj.GlobalPosition = GetNode<Position2D>("muzzle").GlobalPosition;

		float degRad = Mathf.Deg2Rad(RotationDegrees);
		bulletObj.LinearVelocity = 
			new Vector2(Mathf.Cos(degRad), Mathf.Sin(degRad)) 
			* bulletSpeed;
	}
}
