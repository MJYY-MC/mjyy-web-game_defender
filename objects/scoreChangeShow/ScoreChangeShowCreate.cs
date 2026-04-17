using Godot;


public class ScoreChangeShowCreate : Position2D
{
	[Export]
	public PackedScene ScoreChangeShowObject { get; set; }

	Node2D main;
	public override void _Ready() {
		main = GetNode<Node2D>("/root/main");
		DataCore.Instance.gameData.scoreAddon.Score_OnChanged += Score_Changed;
	}

	public void Create(ushort textKey,int score,ushort num) {
		ScoreChangeShow scsObj = (ScoreChangeShow)ScoreChangeShowObject.Instance();
		main.AddChild(scsObj);

		scsObj.Show(
			ScoreAddon.AllText[textKey]+" "+
			(
				(score>=0)
					?"+"
					:""
			)+
			score.ToString()+
			(
				(num>1)
					?"x"+num.ToString()
					:""
			)
			);

		scsObj.GlobalPosition = this.GlobalPosition;
	}

	private void Score_Changed(ScoreAddon.ScoreChangeData scdata) {
		//Create(scdata.TextKey, scdata.ScoreChangeValue, scdata.Mult);
		//可能会影响到引擎遍历碰撞对象，等待到当前物理帧安全时执行
		CallDeferred(nameof(Create), scdata.TextKey, scdata.ScoreChangeValue, scdata.Mult);
	}
}
