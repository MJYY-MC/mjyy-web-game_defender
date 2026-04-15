using Godot;

public class DataCore : Node
{
	public static DataCore Instance { get; private set; }

	public override void _Ready() {
		Instance = this;

		InitData();
	}


	public GameData gameData;
	public void InitData() {
		gameData = new GameData();
	}
}
