using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

namespace CookingPrototype.Utils
{
	[InitializeOnLoad]
	public static class RightToolbarButtons
	{
		private const float PackProjectProgress = 0.5f;

		private const string IgnoreDirectoryLibrary = "Library";
		private const string IgnoreDirectoryTemp = "Temp";
		private const string IgnoreDirectoryObj = "Obj";
		private const string IgnoreDirectoryobj = "obj";
		private const string IgnoreDirectoryIdea = ".idea";
		private const string IgnoreDirectoryLogs = "Logs";

		private const string SymbolAsterisk = "*";

		static RightToolbarButtons()
		{
			ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
		}

		static void OnToolbarGUI()
		{
			GUILayout.FlexibleSpace();

			if (GUILayout.Button(new GUIContent("Pack Project", "Pack zip archive with the project to send to Matryoshka Games"), EditorStyles.toolbarButton))
				{
				try
				{
					EditorUtility.DisplayProgressBar("Packing project", "Packing project to zip archive...", PackProjectProgress);
					PackArchive();
				}
				finally
				{
					EditorUtility.ClearProgressBar();
				}
			}
		}

		static void PackArchive()
		{
			var projectPath = Directory.GetParent(Application.dataPath)?.FullName ?? string.Empty; 
			var outputPath = Path.Combine(projectPath, "TestDeveloper.zip");

			var ignoreDirectories = new[]
			{
				IgnoreDirectoryLibrary,
				IgnoreDirectoryTemp,
				IgnoreDirectoryObj,
				IgnoreDirectoryobj,
				IgnoreDirectoryIdea,
				IgnoreDirectoryLogs
			};
			
			var fullPathIgnoreDirectories = ignoreDirectories
				.Select(ignoreDirectory => Path.Combine(projectPath, ignoreDirectory))
				.ToArray();

			if (File.Exists(outputPath))
				File.Delete(outputPath);
			
			using var archive = ZipFile.Open(outputPath, ZipArchiveMode.Create);
			
			foreach (var filePath in Directory.GetFiles(projectPath, SymbolAsterisk, SearchOption.AllDirectories))
			{
				var anyIgnoredDirectory = fullPathIgnoreDirectories.Any(ignoredDirectoryPath => filePath.StartsWith(ignoredDirectoryPath));

				if ( anyIgnoredDirectory )
				{
					// ignore file
					continue;
				}
				// Skip the output zip file itself
				if (Path.GetFullPath(filePath) == outputPath)
					continue;
				
				var relativePath = Path.GetRelativePath(projectPath, filePath);
				archive.CreateEntryFromFile(filePath, relativePath);
			}
						
			TryOpenDirectory(projectPath);
		}

		static void TryOpenDirectory(string path)
		{
			try
			{
				if (string.IsNullOrEmpty(path))
					throw new ArgumentException("Path cannot be null or empty", nameof(path));

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
				Process.Start("explorer.exe", path.Replace("/", "\\"));
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
    Process.Start("open", path);
#else
    Debug.LogError("Opening directories is not supported on this platform.");
#endif
			}
			catch (Exception)
			{
				// Do nothing
			}
		}
	}
}