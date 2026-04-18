using Godot;
using System;

public class Level : Timer
{
	/// <summary>
	/// 火焰弹对象资源
	/// </summary>
	[Export]
	public PackedScene FireChargeObject { get; set; }
	/// <summary>
	/// 箭矢对象资源
	/// </summary>
	[Export]
	public PackedScene ArrowObject { get; set; }

	const ushort enemySpawnNum= 5;
	Timer[] enemySpawns=new Timer[enemySpawnNum];
    Position2D[] enemySpawns_pos=new Position2D[enemySpawnNum];

	public override void _Ready()
    {
        for(ushort i = 0; i < enemySpawnNum; i++) {
            enemySpawns[i] = GetNode<Timer>("enemySpawn" + i.ToString());
            enemySpawns_pos[i] = enemySpawns[i].GetNode<Position2D>("pos");
		}
	}

	public void StartLevel() {
		this.Start();
		LevelUpdate();
	}
    
#pragma warning disable IDE0051
	private void On_level_timeout() {
#pragma warning restore IDE0051
        DataCore.Instance.gameData.Level++;
        LevelUpdate();
	}

	void LevelUpdate() {
		switch (DataCore.Instance.gameData.Level) {
            case 0:
				enemySpawns[0].WaitTime = 4;
				enemySpawn0_objBasicSpeed = 100;
				enemySpawns[0].Start();
				break;
			case 1:
				enemySpawns[0].WaitTime = 2.5f;
				break;
			case 2:
				enemySpawns[1].WaitTime = 5;
				enemySpawn1_objBasicSpeed = 150;
				enemySpawns[1].Start();
				break;
			case 3:
				enemySpawn0_objBasicSpeed = 200;
				break;
			case 4:
				enemySpawns[1].WaitTime = 3;
				break;
			case 5:
				enemySpawn1_objBasicSpeed = 250;
				break;
			case 6:
				enemySpawns[0].WaitTime = 1;
				break;
			case 7:
				enemySpawns[1].WaitTime = 2;
				break;
			case 8:
				enemySpawn0_objBasicSpeed = 300;
				break;
			case 9:
				enemySpawn1_objBasicSpeed = 350;
				break;
			case 10:
				enemySpawns[2].WaitTime = 8;
				enemySpawn2_objBasicSpeed = 100;
				enemySpawns[2].Start();
				break;
			case 11:
				enemySpawns[3].WaitTime = 10;
				enemySpawns[3].Start();
				break;
			case 12:
				enemySpawns[4].WaitTime = 8;
				enemySpawn4_objBasicSpeed = 100;
				enemySpawns[4].Start();
				break;
			case 13:
				enemySpawns[0].WaitTime = 0.5f;
				enemySpawn0_objBasicSpeed = 400;
				break;
			case 14:
				enemySpawns[2].WaitTime = 6;
				enemySpawn2_objBasicSpeed = 200;
				break;
			case 15:
				enemySpawns[1].WaitTime = 1;
				enemySpawn1_objBasicSpeed = 450;
				break;
			case 16:
				enemySpawns[4].WaitTime = 6;
				enemySpawn4_objBasicSpeed = 150;
				break;
			case 17:
				enemySpawns[3].WaitTime = 8;
				break;
			case 18:
				enemySpawns[2].WaitTime = 4;
				enemySpawn2_objBasicSpeed = 300;
				break;
			case 19:
				enemySpawns[3].WaitTime = 6;
				break;
			case 20:
				enemySpawns[4].WaitTime = 4;
				enemySpawn4_objBasicSpeed = 250;
				break;
			case 21:
				enemySpawns[3].WaitTime = 4;
				break;
			case 22:
				enemySpawns[2].WaitTime = 2;
				enemySpawn2_objBasicSpeed = 400;
				break;
			case 23:
				enemySpawns[3].WaitTime = 2;
				break;
			case 24:
				enemySpawns[4].WaitTime = 2;
				enemySpawn4_objBasicSpeed = 350;
				break;
			case 25:
				enemySpawns[3].WaitTime = 1;
				break;
			case 26:
				enemySpawns[2].WaitTime = 0.5f;
				break;
			case 27:
				enemySpawns[4].WaitTime = 0.5f;
				break;
			case 28:
				enemySpawns[3].WaitTime = 0.5f;
				break;
		}
	}

	float enemySpawn0_objBasicSpeed = 100;
#pragma warning disable IDE0051
	private void On_enemySpawn0_timeout() {
#pragma warning restore IDE0051
		FireCharge fcObj = (FireCharge)FireChargeObject.Instance();
		fcObj.BasicSpeed = enemySpawn0_objBasicSpeed;
		GetTree().CurrentScene.AddChild(fcObj);
		fcObj.GlobalPosition = enemySpawns_pos[0].GlobalPosition;
	}
	float enemySpawn1_objBasicSpeed = 100;
#pragma warning disable IDE0051
	private void On_enemySpawn1_timeout() {
#pragma warning restore IDE0051
		FireCharge fcObj = (FireCharge)FireChargeObject.Instance();
		fcObj.BasicSpeed = enemySpawn1_objBasicSpeed;
		GetTree().CurrentScene.AddChild(fcObj);
		fcObj.GlobalPosition = enemySpawns_pos[1].GlobalPosition;
	}
	float enemySpawn2_objBasicSpeed = 100;
#pragma warning disable IDE0051
	private void On_enemySpawn2_timeout() {
#pragma warning restore IDE0051
		FireCharge fcObj = (FireCharge)FireChargeObject.Instance();
		fcObj.BasicSpeed = enemySpawn2_objBasicSpeed;
		fcObj.MoveDirection = new Vector2(-1, 0.5f);
		GetTree().CurrentScene.AddChild(fcObj);
		fcObj.GlobalPosition = enemySpawns_pos[2].GlobalPosition;
	}
#pragma warning disable IDE0051
	private void On_enemySpawn3_timeout() {
#pragma warning restore IDE0051
		RigidBody2D arObj = (RigidBody2D)ArrowObject.Instance();

		GetTree().CurrentScene.AddChild(arObj);
		arObj.GlobalPosition = enemySpawns_pos[3].GlobalPosition;

		float degRad = Mathf.Deg2Rad((float)GD.RandRange(180,260));//取一定范围内的随机角度，增加不确定性
		arObj.LinearVelocity =
			new Vector2(Mathf.Cos(degRad), Mathf.Sin(degRad))
			* Arrow.CalculateSpeed(arObj.GlobalPosition, new Vector2((float)GD.RandRange(46,86), (float)GD.RandRange(634,674)/*目标点进行随机浮动，避免射击过于精准*/), degRad);//通过已有条件计算初速度，并指定落点
	}
	float enemySpawn4_objBasicSpeed = 100;
#pragma warning disable IDE0051
	private void On_enemySpawn4_timeout() {
#pragma warning restore IDE0051
		FireCharge fcObj = (FireCharge)FireChargeObject.Instance();
		fcObj.BasicSpeed = enemySpawn4_objBasicSpeed;
		fcObj.MoveDirection = new Vector2(-0.5f, 1);
		GetTree().CurrentScene.AddChild(fcObj);
		fcObj.GlobalPosition = enemySpawns_pos[4].GlobalPosition;
	}
}
