using Godot;
using System;

public partial class color_rect : ColorRect
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.GetChild<Node2D>(0).Position = new Vector2( this.Size.X / 2, this.Size.Y / 2 );
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
 
}
