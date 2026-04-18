using Godot;
using System;

public class Main : Node2D
{
	private Level level;

	public override void _Ready()
    {
		level = GetNode<Level>("level");

		GetNode<CanvasLayer>("touchLayer").Visible = DataCore.Instance.gameData.Setting.touchButton == true;
		DataCore.Instance.gameData.GameState = GameData.GameStateEnum.ready;
		StartGame();
	}

	internal void StartGame() {
		DataCore.Instance.gameData.GameState = GameData.GameStateEnum.running;

		level.StartLevel();
	}
}
