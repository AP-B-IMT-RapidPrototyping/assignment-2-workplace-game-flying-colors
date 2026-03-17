using Godot;
using System;

public partial class Checkpoints : Node
{
	private Godot.Collections.Array<Godot.Node> _checkpoints;
	private Godot.Collections.Array<Godot.Node> _valuables;
	private Godot.Collections.Array<Godot.Node> _valuableLights;
	[Export] private Timer updateTimer;
	[Export] private Timer stealTimer;
	private bool stealTimerOn = false;
	private int stolenItemCounter = 0;

	public override void _Ready()
	{
		_checkpoints = GetTree().GetNodesInGroup("checkpoints");
		_valuables = GetTree().GetNodesInGroup("valuables");
		_valuableLights = GetTree().GetNodesInGroup("valuableLight");
		updateTimer.Timeout += Update;
		stealTimer.Timeout += StealItem;
		updateTimer.Start();
	}


	public void Update()
	{
		if (!stealTimerOn)
		{
			if (GetOpenDoors() >= 2)
			{
				GD.Print("More than 1 door open, timer started");
				stealTimer.Start();
				stealTimerOn = true;
			}	
		} else
		{
			if (GetOpenDoors() < 2)
			{
				GD.Print("Less then 2 doors open, timer stopped");
				stealTimer.Stop();
				stealTimerOn = false;
			}
		}
		updateTimer.Start();
	}

	private int GetOpenDoors()
	{
		int teller = 0;
		Checkpoint currentCheckpoint;
		foreach (Checkpoint checkpoint in _checkpoints)
		{
			currentCheckpoint = checkpoint;
			if (currentCheckpoint.hasAnomaly)
			{
				teller++;	
			}
		}
		return teller;
	}

	private void StealItem()
	{
		//still needs more efficient rewrite
		foreach (Valuable valuable in _valuables)
		{
			if (valuable.ID == stolenItemCounter)
			{
				valuable.Visible = false;
			}
		}
		foreach (ValuableLight valuableLight in _valuableLights)
		{
			if (valuableLight.ID == stolenItemCounter)
			{
				valuableLight.TurnRed();
			}
		}
		stolenItemCounter++;
		GD.Print("Item stolen");
		stealTimer.Start();
	}

	private void ResetItems()
	{
		foreach (Valuable valuable in _valuables)
		{
			valuable.Visible = true;
		}
		foreach (ValuableLight valuableLight in _valuableLights)
		{
			valuableLight.TurnGreen();
		}
	}
}
