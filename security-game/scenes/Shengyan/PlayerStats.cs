using Godot;
using System;

//Only handles energy for now

public partial class PlayerStats : Node
{
	[Signal]
	public delegate void EnergyChangedEventHandler(float energy);

	public const float MinEnergy = 0f;
	public const float MaxEnergy = 1f;

	[Export(PropertyHint.Range, "0,1,0.01")]
	private float _energy = MaxEnergy;

	public float Energy => _energy;

	public void SetEnergy(float energy)
	{
		GD.Print("Set Energy triggered.");
		float clampedEnergy = Mathf.Clamp(energy, MinEnergy, MaxEnergy);
		if (_energy == clampedEnergy)
		{
			return;
		}


		_energy = clampedEnergy;
		EmitSignal(SignalName.EnergyChanged, _energy);
	}

	public void ChangeEnergy(float amount)
	{

		SetEnergy(_energy + amount);
	}
}
