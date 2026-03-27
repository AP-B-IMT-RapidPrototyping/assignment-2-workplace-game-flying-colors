using Godot;
using System;
using System.Linq;

public partial class CheckpointInterface : Node
{
	[Signal]
	public delegate void UpdateStatsEventHandler();
	private Godot.Collections.Array<Godot.Node> _checkpoints;
	private Godot.Collections.Array<Godot.Node> _valuables;
	private Godot.Collections.Array<Godot.Node> _valuableLights;
	[Export] private Timer updateTimer;
	[Export] private Timer stealTimer;
	[Export] private Timer anomalyTimer;
	[Export] private Timer bufferTimer;
	[Export] private Label gameOverLabel;
	private bool stealTimerOn = false;
	private int stolenItemCounter = 1;
	private bool doorsPretending = false;
	private int anomalyCounter = 0;
	private int maxAnomalies = 3;
	private bool signAlertActive = false;
	private bool signAlertInitialized = false;

	public override void _Ready()
	{
		gameOverLabel.Visible = false;
		_checkpoints = GetTree().GetNodesInGroup("checkpoints");
		_valuables = GetTree().GetNodesInGroup("valuables");
		_valuableLights = GetTree().GetNodesInGroup("valuableLight");
		updateTimer.Timeout += Update;
		stealTimer.Timeout += StealItem;
		updateTimer.Start();

		SetRandomWaitTime();
		anomalyTimer.Start();
		anomalyTimer.Timeout += addAnomaly;

		bufferTimer.Timeout += LoadTitleScreen;
		CallDeferred(nameof(BroadcastSignAlertState));
	}

	public void Update()
	{
		if (!stealTimerOn)
		{
			if (anomalyCounter >= 2)
			{
				GD.Print("2 or more anomalies active");
				stealTimer.Start();
				stealTimerOn = true;
			}
		}
		else
		{
			if (anomalyCounter < 2)
			{
				GD.Print("Less then 2 anomalies active, timer stopped");
				stealTimer.Stop();
				stealTimerOn = false;
			}
		}
		updateTimer.Start();
		BroadcastSignAlertState();
	}

	/* 
		---------------------
		Stealing items system
		---------------------
	*/

	private void StealItem()
	{
		//still needs more efficient rewrite
		foreach (Valuable valuable in _valuables)
		{
			if (valuable.ID == stolenItemCounter)
			{
				valuable.Visible = false;
				GD.Print("Valuable made invisible");
			}
		}
		foreach (ValuableLight valuableLight in _valuableLights)
		{
			if (valuableLight.ID == stolenItemCounter)
			{
				valuableLight.TurnRed();
			}
		}
		if (stolenItemCounter > _valuables.Count())
		{
			GameOver();
			return;
		}
		else
		{
			stolenItemCounter++;
		}
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

	private void stopStealTimer()
	{
		stealTimer.Stop();
		return;
	}

	/* 
		-------------------------
		Creating anomalies system
		-------------------------
	*/

	public void SetRandomWaitTime()
	{
		anomalyTimer.WaitTime = GD.RandRange(20, 60);
	}

	private void addAnomaly()
	{
		if (anomalyCounter < maxAnomalies)
		{
			int ranIndex = GD.RandRange(0, _checkpoints.Count() - 1);
			for (int i = 0; i < _checkpoints.Count(); i++)
			{
				if (ranIndex == _checkpoints.Count())
					ranIndex = 0;

				Checkpoint currentCheckpoint = _checkpoints[ranIndex] as Checkpoint;
				if (!currentCheckpoint.hasAnomaly)
				{
					currentCheckpoint.MakeAnomaly();
					anomalyCounter++;
					break;
				}
				ranIndex++;
			}
			GD.Print("Amount of active anomalies:" + anomalyCounter);
			BroadcastSignAlertState();
		}
	}

	private void anomalyWasFixed()
	{
		anomalyCounter--;
		BroadcastSignAlertState();
	}

	private void BroadcastSignAlertState()
	{
		bool shouldShowAlert = anomalyCounter >= 2;
		if (signAlertInitialized && shouldShowAlert == signAlertActive)
		{
			return;
		}

		signAlertInitialized = true;
		signAlertActive = shouldShowAlert;
		GetTree().CallGroup("sign_managers", "SetAlertState", signAlertActive);
	}

	private void SetRemaingingItems()
	{
		int teller = 0;
		foreach (Valuable valuable in _valuables)
		{
			if (valuable.Visible)
			{
				teller++;
			}
		}
		GameStats.remainingItems = teller;
	}

	private void GameOver()
	{
		gameOverLabel.Visible = true;
		EmitSignal(SignalName.UpdateStats);
		SetRemaingingItems();
		GameStats.gameJustPLayed = true;
		bufferTimer.Start();
		stopStealTimer();
	}

	private void LoadTitleScreen()
	{
		GD.Print("Loading title screen");
		GetTree().ChangeSceneToFile("res://Scenes/Yusuf/startscherm.tscn");
	}

}
