using Godot;
using System;
using System.Collections.Generic;

public partial class SignManager : Node3D
{
	private static readonly HashSet<string> TargetSignNames = new(StringComparer.OrdinalIgnoreCase)
	{
		"_TitleIntruder",
		"Intruder",
		"Valuables",
		"Valuable",
		"Being",
		"Stolen"
	};

	[Export] private Material redLightMaterial;
	[Export] private Material redDarkMaterial;

	public override void _Ready()
	{
		AddToGroup("sign_managers");

		redLightMaterial ??= ResourceLoader.Load<Material>("res://scenes/Shengyan/RedLightMat.tres");
		redDarkMaterial ??= ResourceLoader.Load<Material>("res://scenes/Shengyan/RedDarkMat.tres");

		foreach (Node child in GetChildren())
		{
			if (child is Node3D signNode)
			{
				SetSignVisualState(signNode, false);
			}
		}

		SetAlertState(false);
	}

	public void SetAlertState(bool isAlertActive)
	{
		Material targetMaterial = isAlertActive ? redLightMaterial : redDarkMaterial;

		foreach (Node child in GetChildren())
		{
			if (!TargetSignNames.Contains(child.Name))
			{
				continue;
			}

			if (child is not Node3D signNode)
			{
				continue;
			}

			SetSignVisualState(signNode, isAlertActive);
		}
	}

	private void SetSignVisualState(Node3D signNode, bool isAlertActive)
	{
		Material targetMaterial = isAlertActive ? redLightMaterial : redDarkMaterial;

		MeshInstance3D meshNode = signNode.GetNodeOrNull<MeshInstance3D>("shape-cube-half");
		if (meshNode != null && targetMaterial != null)
		{
			meshNode.MaterialOverride = targetMaterial;
		}

		OmniLight3D omniLight = signNode.GetNodeOrNull<OmniLight3D>("OmniLight3D");
		if (omniLight != null)
		{
			omniLight.Visible = isAlertActive;
		}

		Label3D label = signNode.GetNodeOrNull<Label3D>("Label3D");
		if (label != null)
		{
			// OFF: receive lighting like non-emissive geometry. ON: restore original unshaded look.
			label.Shaded = !isAlertActive;
		}
	}
}
