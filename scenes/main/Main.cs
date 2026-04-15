using Godot;
using System;

public class Main : Node2D
{
	private Timer fireCharge_spawn;

	public override void _Ready()
    {
		fireCharge_spawn = GetNode<Timer>("fireCharge_spawn");

		DataCore.Instance.gameData.GameState = GameData.GameStateEnum.ready;
		StartGame();
	}

	internal void StartGame() {
		DataCore.Instance.gameData.GameState = GameData.GameStateEnum.running;

		fireCharge_spawn.Start();
	}
}
