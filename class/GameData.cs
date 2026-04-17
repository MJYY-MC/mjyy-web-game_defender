using System;

public class GameData{

	public class GameSetting {
		/// <summary>
		/// 触摸按钮是否启用
		/// </summary>
		public bool? touchButton = null;
	}
	public GameSetting Setting { get; set; } = new GameSetting();

	#region state
	public enum GameStateEnum {
		menu,
		loading,
		ready,
		running,
		over,
	}
	/// <summary>
	/// 当前游戏状态
	/// </summary>
	public GameStateEnum GameState { get; set; } = GameStateEnum.menu;
	#endregion

	#region health
	/// <summary>
	/// 主的最大血量
	/// </summary>
	public float HomeHealthMax { get;private set; } = 100f;

	public Action<float> HomeHealth_OnChanged=null;
	private float homeHealth;
	/// <summary>
	/// 主的血量
	/// </summary>
	public float HomeHealth {
		get => homeHealth;
		set {
			if (value < 0)
				homeHealth = 0;
			else if (value > HomeHealthMax)
				homeHealth = HomeHealthMax;
			else
				homeHealth = value;
			HomeHealth_OnChanged?.Invoke(HomeHealth);
		}
	}

	/// <summary>
	/// 获取当前主的血量百分比
	/// </summary>
	/// <returns>返回血量百分比（小数形式）</returns>
	public float GetHomeHealthPercent() => HomeHealth / HomeHealthMax;
	#endregion

	#region score
	private int score=0;
	/// <summary>
	/// 当得分改变后触发
	/// </summary>
	public Action<int> Score_OnChanged=null;
	/// <summary>
	/// 得分
	/// </summary>
	public int Score {
		get => score;
		set {
			score = value;
			Score_OnChanged?.Invoke(Score);
		}
	}

	#endregion

	public GameData() {
		homeHealth = HomeHealthMax;
	}
}