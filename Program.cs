using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using Microsoft.Win32;

namespace CameraPrefsApp
{
	class Program
	{
		private static Configuration config;

		static void Main(string[] args)
		{
			String configPath = "CameraPrefs.xml";
			if (args.Length >= 1)
				configPath = args[0];

            Console.WriteLine("\nReading " + configPath + "\n");

            config = loadAndParseConfig(configPath);

			applyCameraSettings(config.Cameras);

            // There is no LifeCam software for Windows 10
            if (!isWindows10())
            {
                applyTrueColorSetting(config.TrueColorEnabled);
            }
		}

		private static Configuration loadAndParseConfig(String pPath)
		{
			try
			{
				return Configuration.Deserialize(pPath);
			}
			catch (IOException pException)
			{
				TextWriter errorWriter = Console.Error;
				errorWriter.WriteLine("\n  ERROR: Config not found at \"" + pPath + "\"" + " : " + pException.Message);
				return null;
			}
		}

		private static void applyTrueColorSetting(Boolean pValue)
		{
			Console.WriteLine("\nApplying TrueColor setting to registry...");

			RegistryKey hkcu = Registry.CurrentUser;

			hkcu = hkcu.OpenSubKey("Software\\Microsoft\\LifeCam", true);

			try
			{
				hkcu.SetValue("TrueColorOff", pValue ? 0 : 1);
			}
			catch (Exception pException)
			{
                Console.WriteLine("\n  ERROR: LifeCam entry not found. Is the LifeCam software installed?\n\tError message: " + pException.Message);
			}
		}

        static bool isWindows10()
        {
            var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");

            string productName = (string)reg.GetValue("ProductName");

            return productName.StartsWith("Windows 10");
        }

        private static void applyCameraSettings(CameraPrefs[] pCameraPrefsList)
		{
			foreach (CameraPrefs cameraPrefs in pCameraPrefsList)
			{
				try
				{
					MyCameraControl camera = MyCameraControl.CreateFromPrefs(cameraPrefs);
				}
				catch (Exception pException)
				{
					Console.WriteLine("\n  ERROR: " + pException.Message);
					continue;
				}

			}
		}

	}
}
