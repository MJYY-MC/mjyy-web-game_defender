using Godot.Collections;
using System;

/// <summary>
/// 分数扩展类
/// </summary>
public class ScoreAddon {
	/// <summary>
	/// 所有分数更改说明文本
	/// </summary>
	public static Dictionary<ushort, string> AllText { get;private set; } = new Dictionary<ushort, string>() {
		{0,"这是一个测试文本" },
		{1,"发射蓄力弹" },
		{2,"发射霰弹" },
		{3,"命中" },
	};
	/// <summary>
	/// 分数更改详细数据
	/// </summary>
	public class ScoreChangeData {
		/// <summary>
		/// 分数更改说明文本对应键
		/// </summary>
		public ushort TextKey {  get; set; }
		/// <summary>
		/// 分数将更改的数值
		/// </summary>
		public int ScoreChangeValue {  get; set; }
		/// <summary>
		/// 分数更改值的倍数，用于一些需要翻倍的地方
		/// </summary>
		public ushort Mult {  get; set; }
	}

	/// <summary>
	/// 当使用分数扩展类中的函数更改分数时将调用此事件
	/// </summary>
	public Action<ScoreChangeData> Score_OnChanged;

	/// <summary>
	/// 执行分数更改
	/// </summary>
	/// <param name="data">分数更改的详细数据</param>
	public void ChangeScore(ScoreChangeData data) {
		DataCore.Instance.gameData.Score += data.ScoreChangeValue * data.Mult;
		Score_OnChanged?.Invoke(data);
	}
}