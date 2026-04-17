using Godot;
using System;

public class FireCharge : RigidBody2D
{
	/// <summary>
	/// 基础速度
	/// </summary>
	[Export] 
	public float BasicSpeed = 100f;

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

	/// <summary>
	/// 在被击中后恢复重力的延迟时间
	/// </summary>
	[Export]
	public float HurtRecoverGravityDelay { get; set; } = 3.0f;

	private Tween goneAnim;
	public override void _Ready() {
		goneAnim = GetNode<Tween>("goneAnim");
		doGoneAnimTimer = goneAnim.GetNode<Timer>("doGoneAnimTimer");
	}

	bool isFlying = true;
	public override void _IntegrateForces(Physics2DDirectBodyState state) {
		if (isFlying) {
			Vector2 currentVelocity = state.LinearVelocity;

			currentVelocity.x = -BasicSpeed;

			state.LinearVelocity = currentVelocity;
		}
	}

#pragma warning disable IDE0051
	private void OnFireCharge_bodyEntered(object body) {
#pragma warning restore IDE0051
		if (body is Node nodeObj) {
			if (nodeObj.IsInGroup("bullet") && !doingGone)
				DataCore.Instance.gameData.Score += 10;
			DoGone();
		}
	}


	Timer doGoneAnimTimer;
	byte doGoneAnimTimer_code = 0;
#pragma warning disable IDE0051
	private void On_doGoneAnimTimer_timeout() {
#pragma warning restore IDE0051
		DoGone(doGoneAnimTimer_code);
	}

	/// <summary>
	/// 是否正在执行消失
	/// </summary>
	private bool doingGone = false;
	private void DoGone(byte code = 0) {
		switch (code) {
			case 0:
				if (!doingGone) {
					doingGone = true;

					isFlying = false;

					doGoneAnimTimer_code = 1;
					doGoneAnimTimer.WaitTime = HurtRecoverGravityDelay;
					doGoneAnimTimer.Start();
				}
				break;
			case 1:
				GravityScale = 1;

				doGoneAnimTimer_code = 2;
				doGoneAnimTimer.WaitTime = GoneAnimationDelay;
				doGoneAnimTimer.Start();
				break;
			case 2:
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
}
