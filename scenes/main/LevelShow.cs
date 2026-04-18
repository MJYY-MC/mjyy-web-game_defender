using Godot;
using System;

public class LevelShow : Label
{
    Label levelValue;
    public override void _Ready()
    {
        levelValue=GetNode<Label>("value");

        Level_Changed(DataCore.Instance.gameData.Level);
        DataCore.Instance.gameData.Level_OnChanged += Level_Changed;
	}
    void Level_Changed(ushort lv) => levelValue.Text = lv.ToString();
}
