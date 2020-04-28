using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Video.DirectShow.Internals;

namespace CameraPrefsApp
{
	class MyCameraConrol
	{
		public VideoCaptureDevice Source;

		private IAMCameraControl cameraControls;
		private IAMVideoProcAmp videoProcAmp;

		private String _name;

		public MyCameraConrol(string name)
		{
			_name = name;

			String moniker = null;

			FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            Encoding e = Encoding.GetEncoding("iso-8859-1");
            Console.OutputEncoding = e;

            Console.WriteLine("Scanning for video device " + name + "\n");
            
            // Match specified camera name to device
            for (int i = 0, n = videoDevices.Count; i < n; i++)
			{
                Console.WriteLine("Detected video device: " + videoDevices[i].Name + " checking: " + (name.Equals(videoDevices[i].Name)));

                if (name == videoDevices[i].Name)
				{
					moniker = videoDevices[i].MonikerString;
					break;
				}
			}

			if (moniker == null)
				return;

			//throw new Exception("Video device with name '" + name + "' not found.");

			Source = new VideoCaptureDevice(moniker);
			Source.DesiredFrameRate = 30;

                
			cameraControls = (IAMCameraControl)Source.SourceObject;
			videoProcAmp = (IAMVideoProcAmp)Source.SourceObject;
        }

		static public MyCameraConrol CreateFromPrefs(CameraPrefs prefs)
		{
			MyCameraConrol newCamera = new MyCameraConrol(prefs.Name);
            int red = 0;
            int green = 0;
            int blue = 0;

            if (newCamera.Source == null)
				throw new Exception("Camera \"" + prefs.Name + "\" not found. Skipping.");

            Console.WriteLine("Current settings are:");

            Console.WriteLine("\t" + newCamera.Name + "\n");

            int focusMin = 0, focusMax = 0, focusStep = 0;
            newCamera.GetFocusRange(ref focusMin, ref focusMax, ref focusStep);
            Console.WriteLine("\tFocus:\t\t" + newCamera.Focus);
            Console.WriteLine("\tAuto Focus:\t" + newCamera.AutoFocus);
            Console.WriteLine("\tFocus Range:\t" + focusMin + " <> " + focusMax + ", step: " + focusStep + "\n");

            int exposureMin = 0, exposureMax = 0, exposureStep = 0, exposureDef = 0;
            newCamera.GetExposureRange(ref exposureMin, ref exposureMax, ref exposureStep, ref exposureDef);
            Console.WriteLine("\tExposure:\t" + newCamera.Exposure);
            Console.WriteLine("\tAuto Exposure:\t" + newCamera.AutoExposure);
            Console.WriteLine("\tExposure Range:\t" + exposureMin + " <> " + exposureMax + ", step: " + exposureStep + ", def: " + exposureDef + "\n");

            Console.WriteLine("\tBacklight compensation: " + newCamera.BacklightCompensation + "\n");

            Console.WriteLine("\tBrightness:\t" + newCamera.Brightness);
            Console.WriteLine("\tContrast:\t" + newCamera.Contrast);
            Console.WriteLine("\tWhite balance:\t" + newCamera.WhiteBalance);
            Console.WriteLine("\tSaturation:\t" + newCamera.Saturation);
            Console.WriteLine("\tSharpness:\t" + newCamera.Sharpness + "\n");

            red = (newCamera.Gain >> 16) & 0x0ff;
            green = (newCamera.Gain >> 8) & 0x0ff;
            blue = (newCamera.Gain) & 0x0ff;
            Console.WriteLine("\tGain:\t\t" + "red: " + red + " green: " + green + " blue: " + blue);

            red = (newCamera.Gamma >> 16) & 0x0ff;
            green = (newCamera.Gamma >> 8) & 0x0ff;
            blue = (newCamera.Gamma) & 0x0ff;
            Console.WriteLine("\tGamma:\t\t" + "red: " + red + " green: " + green + " blue: " + blue);

            red = (newCamera.Hue >> 16) & 0x0ff;
            green = (newCamera.Hue >> 8) & 0x0ff;
            blue = (newCamera.Hue) & 0x0ff;
            Console.WriteLine("\tHue:\t\t" + "red: " + red + " green: " + green + " blue: " + blue);

            red = (newCamera.ColorEnable >> 16) & 0x0ff;
            green = (newCamera.ColorEnable >> 8) & 0x0ff;
            blue = (newCamera.ColorEnable) & 0x0ff;
            Console.WriteLine("\tColor enable:\t" + "red: " + red + " green: " + green + " blue: " + blue);

            red = (newCamera.Iris >> 16) & 0x0ff;
            green = (newCamera.Iris >> 8) & 0x0ff;
            blue = (newCamera.Iris) & 0x0ff;
            Console.WriteLine("\tIris:\t\t" + "red: " + red + " green: " + green + " blue: " + blue + "\n");

            int panMin = 0, panMax = 0, panStep = 0, panDef = 0;
            newCamera.GetPanRange(ref panMin, ref panMax, ref panStep, ref panDef);
            Console.WriteLine("\tPan:\t\t" + newCamera.Pan);
            Console.WriteLine("\tPan Range:\t" + panMin + " <> " + panMax + ", step: " + panStep + ", def: " + panDef + "\n");

            int tiltMin = 0, tiltMax = 0, tiltStep = 0, tiltDef = 0;
            newCamera.GetTiltRange(ref tiltMin, ref tiltMax, ref tiltStep, ref tiltDef);
            Console.WriteLine("\tTilt:\t\t" + newCamera.Tilt);
            Console.WriteLine("\tTilt Range:\t" + tiltMin + " <> " + tiltMax + ", step: " + tiltStep + ", def: " + tiltDef + "\n");

            int zoomMin = 0, zoomMax = 0, zoomStep = 0;
            newCamera.GetZoomRange(out zoomMin, out zoomMax, out zoomStep);
            Console.WriteLine("\tZoom:\t\t" + newCamera.Zoom);
            Console.WriteLine("\tZoom Range:\t" + zoomMin + " <> " + zoomMax + ", step: " + zoomStep + "\n");

            red = (newCamera.Roll >> 16) & 0x0ff;
            green = (newCamera.Roll >> 8) & 0x0ff;
            blue = (newCamera.Roll) & 0x0ff;
            Console.WriteLine("\tRoll:\t\t" + "red: " + red + " green: " + green + " blue: " + blue);

            Console.WriteLine("\nApplying \"" + prefs.Name + "\" settings...\n");

            FieldInfo[] fields = prefs.GetType().GetFields();
            PropertyInfo pi = null;
            foreach (FieldInfo field in fields)
			{
				try
				{
                    pi = newCamera.GetType().GetProperty(field.Name);

                    if (pi == null)
                    {
                        Console.WriteLine("Camera does not support '" + field.Name + "', setting skipped");
                    }
                    else
                    {
                        pi.SetValue(newCamera, field.GetValue(prefs), null);
                        Console.WriteLine("  => " + field.Name + ": " + field.GetValue(prefs));
                    }
				}
				catch (Exception e)
				{
                    Console.WriteLine(" Unexpected error white setting camera property '" + field.Name + "': " + e.Message + "\n");
				}
			}

			return newCamera;
		}

		public String Name
		{
			get { return _name; }
			set { }
		}

		/**
		 * Camera Control
		 */

        // VideoCaptureDevice interaction
		public int Focus
		{
			get { return Source.GetFocus(); }
			set { Source.SetFocus(value); }
		}

		public Boolean AutoFocus
		{
			get { return Source.GetAutoFocus(); }
			set { Source.SetAutoFocus(value); }
		}

        public void GetFocusRange(ref int minimum, ref int maximum, ref int step)
        {
            int defaultValue = 0;
            Source.GetFocusRange(ref minimum, ref maximum, ref step, ref defaultValue);
        }

        

        public int Zoom
		{
			get { return Source.GetZoom(); }
			set { Source.SetZoom(value); }
		}

		public void GetZoomRange(out int minimum, out int maximum, out int step)
		{
			int defaultValue;
			Source.GetZoomRange(out minimum, out maximum, out step, out defaultValue);
		}

		public int Pan
		{
			get { return Source.GetPan(); }
			set { Source.SetPan(value); }
		}

		public void GetPanRange(ref int min, ref int max, ref int step, ref int def)
		{
			Source.GetPanRange(ref min, ref max, ref step, ref def);
		}

		public int Tilt
		{
			get { return Source.GetTilt(); }
			set { Source.SetTilt(value); }
		}

        public void GetTiltRange(ref int min, ref int max, ref int step, ref int def)
		{
			Source.GetTiltRange(ref min, ref max, ref step, ref def);
		}

        /**
		 * Video Settings
		 */

        // Camera control interaction

        // IAMCameraControl cameraControls setters/getters

        // CameraControlFlags.Auto
        // CameraControlFlags.Manual
        // CameraControlFlags.None


        /*
        CameraControlProperty.Pan
        CameraControlProperty.Tilt
        CameraControlProperty.Roll *
        CameraControlProperty.Zoom
        CameraControlProperty.Exposure
        CameraControlProperty.Iris *
        CameraControlProperty.Focus

        */

        public void setExposure(int value)
        {
            Source.SetExposure(value);
        }

        /*
        public int Exposure
        {
            get { int value; CameraControlFlags flags; return cameraControls.Get(CameraControlProperty.Exposure, out value, out flags); }
            set { cameraControls.Set(CameraControlProperty.Exposure, value, CameraControlFlags.Manual); }
        }
        */
        public int Exposure
        {
            get { return Source.GetExposure(); }
            set { Source.SetExposure(value); }
        }

        public Boolean AutoExposure
        {
            get { return Source.GetAutoExposure(); }
            set { Source.SetAutoExposure(value); }

        }

        public void GetExposureRange(ref int minimum, ref int maximum, ref int step, ref int def)
        {
            int defaultValue = 0;
            Source.GetExposureRange(ref minimum, ref maximum, ref step, ref defaultValue);
        }

        public int Iris
        {
            get { int value; CameraControlFlags flags; return cameraControls.Get(CameraControlProperty.Iris, out value, out flags); }
            set { cameraControls.Set(CameraControlProperty.Iris, value, CameraControlFlags.Manual); }
        }

        public int Roll
        {
            get { int value; CameraControlFlags flags; return cameraControls.Get(CameraControlProperty.Roll, out value, out flags); }
            set { cameraControls.Set(CameraControlProperty.Roll, value, CameraControlFlags.Manual); }
        }


        // Video processing
        // IAMVideoProcAmp videoProcAmp setters/getters

        public int BacklightCompensation
        {
            get { int value; VideoProcAmpFlags flags; return videoProcAmp.Get(VideoProcAmpProperty.BacklightCompensation, out value, out flags); }
            set { videoProcAmp.Set(VideoProcAmpProperty.BacklightCompensation, value, VideoProcAmpFlags.Manual); }
        }

        public int Brightness {
			get { int value; VideoProcAmpFlags flags; return videoProcAmp.Get(VideoProcAmpProperty.Brightness, out value, out flags); }
			set { videoProcAmp.Set(VideoProcAmpProperty.Brightness, value, VideoProcAmpFlags.Manual); }
		}

        public int ColorEnable {
            get { int value; VideoProcAmpFlags flags; return videoProcAmp.Get(VideoProcAmpProperty.ColorEnable, out value, out flags); }
            set { videoProcAmp.Set(VideoProcAmpProperty.ColorEnable, value, VideoProcAmpFlags.Manual); }
        }

        public int Contrast {
			get { int value; VideoProcAmpFlags flags; return videoProcAmp.Get(VideoProcAmpProperty.Contrast, out value, out flags); }
			set { videoProcAmp.Set(VideoProcAmpProperty.Contrast, value, VideoProcAmpFlags.Manual); }
		}

        public int Gain {
            get { int value; VideoProcAmpFlags flags; return videoProcAmp.Get(VideoProcAmpProperty.Gain, out value, out flags); }
            set { videoProcAmp.Set(VideoProcAmpProperty.Gain, value, VideoProcAmpFlags.Manual); }
        }

        public int Gamma
        {
            get { int value; VideoProcAmpFlags flags; return videoProcAmp.Get(VideoProcAmpProperty.Gamma, out value, out flags); }
            set { videoProcAmp.Set(VideoProcAmpProperty.Gamma, value, VideoProcAmpFlags.Manual); }
        }

        public int Hue
        {
            get { int value; VideoProcAmpFlags flags; return videoProcAmp.Get(VideoProcAmpProperty.Hue, out value, out flags); }
            set { videoProcAmp.Set(VideoProcAmpProperty.Hue, value, VideoProcAmpFlags.Manual); }
        }

        public int Saturation
        {
            get { int value; VideoProcAmpFlags flags; return videoProcAmp.Get(VideoProcAmpProperty.Saturation, out value, out flags); }
            set { videoProcAmp.Set(VideoProcAmpProperty.Saturation, value, VideoProcAmpFlags.Manual); }
        }

        public int Sharpness
        {
            get { int value; VideoProcAmpFlags flags; return videoProcAmp.Get(VideoProcAmpProperty.Sharpness, out value, out flags); }
            set { videoProcAmp.Set(VideoProcAmpProperty.Sharpness, value, VideoProcAmpFlags.Manual); }
        }

        public int WhiteBalance
        {
            get { int value; VideoProcAmpFlags flags; return videoProcAmp.Get(VideoProcAmpProperty.WhiteBalance, out value, out flags); }
            set { videoProcAmp.Set(VideoProcAmpProperty.WhiteBalance, value, VideoProcAmpFlags.Manual); }
        }
    }
}
