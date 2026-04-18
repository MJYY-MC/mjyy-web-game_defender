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
				enemySpawns[0].Start();
				break;
			case 1:
				enemySpawns[0].WaitTime = 2.5f;
				break;
			case 2:
				enemySpawns[1].WaitTime = 5;
				enemySpawns[1].Start();
				break;
			case 3:
				enemySpawns[0].WaitTime = 1f;
				break;
			case 4:
				enemySpawns[1].WaitTime = 3;
				break;
			case 5:
				enemySpawns[1].WaitTime = 1.5f;
				break;
		}
	}

#pragma warning disable IDE0051
	private void On_enemySpawn0_timeout() {
#pragma warning restore IDE0051
		var fcObj = (RigidBody2D)FireChargeObject.Instance();
		GetTree().CurrentScene.AddChild(fcObj);
		fcObj.GlobalPosition = enemySpawns_pos[0].GlobalPosition;
	}
#pragma warning disable IDE0051
	private void On_enemySpawn1_timeout() {
#pragma warning restore IDE0051
		var fcObj = (RigidBody2D)FireChargeObject.Instance();
		GetTree().CurrentScene.AddChild(fcObj);
		fcObj.GlobalPosition = enemySpawns_pos[1].GlobalPosition;
	}
}
