using Godot;
using System;

public class Bullet : RigidBody2D
{
	Player player;
	public override void _Ready() {
		goneAnim = GetNode<Tween>("goneAnim");
		doGoneAnimTimer = goneAnim.GetNode<Timer>("doGoneAnimTimer");
		player = GetNode<Player>("/root/main/player");

		PlayShowAnim();
	}

	/// <summary>
	/// 表示该子弹命中的敌方次数，用于计算单发连续击中
	/// </summary>
	private ushort oneShotComboHitNum = 0;

	/// <summary>
	/// 击中得分，得分因子弹类型而异
	/// </summary>
	private int hitScore = 0;

#pragma warning disable IDE0051
	private void OnBulletBody_entered(object body) {
#pragma warning restore IDE0051
		if (body is Node nodeObj) {
			if (!this.IsInGroup("bullet_notFirst")) {
				this.AddToGroup("bullet_notFirst");//打上该组则表示该子弹已经命中过某个目标一次了

				player.BulletFirstHitFeedback(
					nodeObj.IsInGroup("enemy") && !nodeObj.IsInGroup("enemy_byBulletHit") && this.IsInGroup("bullet_aloneMode")
					);
			}
			if (nodeObj.IsInGroup("bulletTarget")) {
				if (nodeObj.IsInGroup("enemy")) {
					if (!nodeObj.IsInGroup("enemy_byBulletHit")) {
						nodeObj.AddToGroup("enemy_byBulletHit");//表示该敌方已被子弹命中过了
						oneShotComboHitNum++;

						if (hitScore == 0) {
							if (this.IsInGroup("bullet_aloneMode"))
								hitScore = 10;
							else if (this.IsInGroup("bullet_shotMode"))
								hitScore = 5;
						}

						DataCore.Instance.gameData.scoreAddon.ChangeScore(new ScoreAddon.ScoreChangeData() {
							TextKey = ((oneShotComboHitNum == 1) ? (ushort)3 : (ushort)5),
							ScoreChangeValue = hitScore,
							Mult = oneShotComboHitNum,
						});
					}
				}
				PlayGoneAnim();
			}
		}
	}

#pragma warning disable IDE0051
	private void OnGoneTimer_timeout() =>
#pragma warning restore IDE0051
		PlayGoneAnim();



	#region playGoneAnim
	/// <summary>
	/// 消失动画持续时间
	/// </summary>
	[Export]
	public float GoneAnimationDuration { get; set; } = 0.5f;
	/// <summary>
	/// 执行消失动画前的等待时间
	/// </summary>
	[Export]
	public float GoneAnimationDelay { get; set; } = 3.0f;


	Tween goneAnim;
	Timer doGoneAnimTimer;
	byte doGoneAnimTimer_code = 0;
#pragma warning disable IDE0051
	private void On_doGoneAnimTimer_timeout() {
#pragma warning restore IDE0051
		PlayGoneAnim(doGoneAnimTimer_code);
	}

	/// <summary>
	/// 是否正在执行消失
	/// </summary>
	private bool doingGone = false;
	public void PlayGoneAnim(byte code = 0) {
		switch (code) {
			case 0:
				if (!doingGone) {
					doingGone = true;
					doGoneAnimTimer_code = 1;
					doGoneAnimTimer.WaitTime = GoneAnimationDelay;
					doGoneAnimTimer.Start();
				}
				break;
			case 1:
				goneAnim.InterpolateProperty(
						this,
						"modulate:a",
						this.Modulate.a,
						0f,
						GoneAnimationDuration,
						Tween.TransitionType.Linear,
						Tween.EaseType.In
						);
				goneAnim_tac_code = 1;
				goneAnim.Start();
				break;
		}
	}
	byte goneAnim_tac_code = 0;
#pragma warning disable IDE0051
	private void On_goneAnim_tween_all_completed() {
#pragma warning restore IDE0051
		switch (goneAnim_tac_code) {
			case 1:
				QueueFree();
				break;
		}
	}

	/// <summary>
	/// 出现动画持续时间
	/// </summary>
	[Export] public float ShowAnimationDuration { get; set; } = 0.2f;

	public void PlayShowAnim() {
		goneAnim.InterpolateProperty(
			this,
			"scale",
			Vector2.Zero,
			this.Scale,
			ShowAnimationDuration,
			Tween.TransitionType.Linear,
			Tween.EaseType.In
			);
		goneAnim.Start();
	}
	#endregion
}
