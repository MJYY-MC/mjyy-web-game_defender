using Godot;
using System;

public class FireCharge_spawn : Timer
{
	/// <summary>
	/// 火焰弹对象资源
	/// </summary>
	[Export]
	public PackedScene FireChargeObject { get; set; }


	private Position2D fireCharge_spawnPos;

	public override void _Ready()
    {
        fireCharge_spawnPos = GetNode<Position2D>("fireCharge_spawnPos");
    }

#pragma warning disable IDE0051
	private void OnFireChargeSpawn_timeout() {
#pragma warning restore IDE0051
		if (FireChargeObject == null && OS.IsDebugBuild()) {
			GD.PrintErr("错误: FireChargeObject为null");
			return;
		}

		var fcObj = (RigidBody2D)FireChargeObject.Instance();
		GetTree().CurrentScene.AddChild(fcObj);
		fcObj.GlobalPosition = fireCharge_spawnPos.GlobalPosition;
	}
}
