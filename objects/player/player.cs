using Godot;

public class player : Node2D
{
	/// <summary>
	/// 旋转速度（弧度/秒）
	/// </summary>
	[Export]
	public float RotationSpeed = 10.0f;

	public override void _Process(float delta) {
		{
			float rotateInput = 0f;

			if (Input.IsActionPressed("key_left"))
				rotateInput -= 1f;
			if (Input.IsActionPressed("key_right"))
				rotateInput += 1f;

			Rotation += rotateInput * RotationSpeed * delta;
		}
	}

	public override void _Input(InputEvent ie) {
		if (ie.IsActionPressed("key_up")) 
			ShootBullet(1000);
		else if(ie.IsActionPressed("key_down"))
			ShootBullet(500);
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
	private void ShootBullet(int bulletSpeed) {
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
