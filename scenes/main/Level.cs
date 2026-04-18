using Godot;
using System;

public class Level : Timer
{
	/// <summary>
	/// 火焰弹对象资源
	/// </summary>
	[Export]
	public PackedScene FireChargeObject { get; set; }
	
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
    
    ushort level = 0;
#pragma warning disable IDE0051
	private void On_level_timeout() {
#pragma warning restore IDE0051
        level++;
        LevelUpdate();
	}

	void LevelUpdate() {
        switch (level) {
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
}
