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
			//if (nodeObj.IsInGroup("enemyTarget")) {
				DoGone();
			//}
		}
	}

	/// <summary>
	/// 是否正在执行消失
	/// </summary>
	private bool doingGone = false;
	private async void DoGone() {
		/*
		 * 批注：该函数存在await，在游戏结束对象都被销毁时await结束后仍然会执行，会导致其报错：
		 * E 0:00:05.905   _signal_callback: Resumed after await, but class instance is gone.
			<C++ 错误>      Condition "conn_target_id && !ObjectDB::get_instance(conn_target_id)" is true. Returned: Variant()
			<C++ 源文件>     modules/mono/signal_awaiter_utils.cpp:69 @ _signal_callback()
		 * 后续可选的解决方案：使用Timer节点来实现方法
		 */
		if (!doingGone) {
			doingGone = true;

			isFlying = false;
			await ToSignal(GetTree().CreateTimer(HurtRecoverGravityDelay), "timeout");
			GravityScale = 1;

			await ToSignal(GetTree().CreateTimer(GoneAnimationDelay), "timeout");

			goneAnim.InterpolateProperty(
				this,
				"modulate:a",
				this.Modulate.a,
				0f,
				GoneAnimationDuration,
				Tween.TransitionType.Linear,
				Tween.EaseType.In
				);
			goneAnim.Start();

			await ToSignal(goneAnim, "tween_all_completed");

			QueueFree();
		}
	}
}
