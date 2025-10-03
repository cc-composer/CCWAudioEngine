// Copyright Epic Games, Inc. All Rights Reserved.

using System.Collections.Generic;
using System.IO;
using UnrealBuildTool;

public class CCWAudioEngine : ModuleRules
{
	public CCWAudioEngine(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;
		
		PublicDependencyModuleNames.AddRange(
			new string[]
			{
				"Core",
			}
		);
			
		PrivateDependencyModuleNames.AddRange(
			new string[]
			{
				"CoreUObject",
				"Engine"
			}
		);

		// Wwise Sound Engine include path
		string ThirdPartyDirectory = Path.Combine(ModuleDirectory, "../../ThirdParty/");
		PublicIncludePaths.Add(Path.Combine(ThirdPartyDirectory, "include"));

		string PlatformName = "";
		if (Target.Platform.IsInGroup(UnrealPlatformGroup.Windows))
		{
			PlatformName = "x64_vc170/";
		}
		
		string Configuration = "";
		switch (Target.Configuration)
		{
			case UnrealTargetConfiguration.Debug:
			case UnrealTargetConfiguration.DebugGame:
			case UnrealTargetConfiguration.Test:
				Configuration = "Debug/";
				break;

			case UnrealTargetConfiguration.Development:
				Configuration = "Profile/";
				break;

			case UnrealTargetConfiguration.Shipping:
				Configuration = "Release/";
				break;

			default:
				break;
		}

		// Wwise libraries
		string LibraryDirectory = Path.Combine([ThirdPartyDirectory, PlatformName, Configuration, "lib"]);
		List<string> Libraries = []; 
		
		foreach(string Library in Directory.GetFiles(LibraryDirectory, "*.lib"))
		{
			if (Target.Configuration == UnrealTargetConfiguration.Shipping && Library == "CommunicationCentral.lib")
			{
				continue;
			}
			
			Libraries.Add(Library);
		}

		PublicAdditionalLibraries.AddRange(Libraries);
		
		// Engine definitions
		if (Target.Configuration == UnrealTargetConfiguration.Shipping)
		{
			PublicDefinitions.Add("CCW_RELEASE");
		}
		else
		{
			PublicDefinitions.Add("CCW_DEBUG");
		}
	}
}
