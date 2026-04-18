using Godot;
using System;

public class Arrow : RigidBody2D {
	private Tween goneAnim;
	Timer doGoneAnimTimer;
	public override void _Ready() {
		goneAnim = GetNode<Tween>("goneAnim");
		doGoneAnimTimer = goneAnim.GetNode<Timer>("doGoneAnimTimer");
	}

#pragma warning disable IDE0051
	private void On_arrow_body_entered(object body) {
#pragma warning restore IDE0051
		if (body is Node nodeObj) {
			DoGone();
		}
	}


	#region goneAnim
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
	#endregion

	public override void _PhysicsProcess(float delta) {
		//if (LinearVelocity.Length()>100f) {//速度大于指定值时保持箭头朝运动方向的前方
		if (!doingGone) {//开始执行消失动画时则表示已碰撞到物体，此时即不再调整箭头朝向
			//Rotation = vel.Angle() + Mathf.Pi / 2;
			Rotation = Mathf.LerpAngle(Rotation, (LinearVelocity.Angle() + Mathf.Pi / 2), .9f);//平滑旋转
		}
	}

	/// <summary>
	/// 根据已有条件，计算命中目标所需的速度大小。
	/// </summary>
	/// <param name="startPos">发射起点</param>
	/// <param name="targetPos">目标落点</param>
	/// <param name="angleRad">发射角度（弧度）</param>
	/// <param name="gravity">重力加速度大小</param>
	/// <returns>满足条件的初速度大小；若无法命中则返回 float.NaN</returns>
	public static float CalculateSpeed(Vector2 startPos, Vector2 targetPos, float angleRad, float gravity = 980f) {
		Vector2 delta = targetPos - startPos;
		float dx = delta.x;
		float dy = delta.y;

		float cosTheta = Mathf.Cos(angleRad);
		float tanTheta = Mathf.Tan(angleRad);

		float denominator = dy - dx * tanTheta;
		if (denominator <= 0f)
			return float.NaN;

		float vSqr = (gravity * dx * dx) / (2f * cosTheta * cosTheta * denominator);
		if (vSqr < 0f)
			return float.NaN;

		return Mathf.Sqrt(vSqr);
	}
}
