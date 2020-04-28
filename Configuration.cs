using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace CameraPrefsApp
{

	/// <summary>
	/// This Configuration class is basically just a set of 
	/// properties with a couple of static methods to manage
	/// the serialization to and deserialization from a
	/// simple XML file.
	/// </summary>
	[Serializable]
	[XmlRoot("config")]
	public class Configuration
	{
		[XmlArray("cameras")]
		[XmlArrayItem("camera")]
		public CameraPrefs[] Cameras;

		[XmlElement("trueColorEnabled")]
		public Boolean TrueColorEnabled = false;

		public static void Serialize(string file, Configuration c)
		{
			XmlSerializer xs = new XmlSerializer(c.GetType());
			StreamWriter writer = File.CreateText(file);
			xs.Serialize(writer, c);
			writer.Flush();
			writer.Close();
		}
		public static Configuration Deserialize(string file)
		{
			XmlSerializer xs = new XmlSerializer(typeof(Configuration));
			StreamReader reader = File.OpenText(file);
			Configuration c = (Configuration)xs.Deserialize(reader);
			reader.Close();
			return c;
		}
	}


	[Serializable]
	[XmlRoot("camera")]
	public class CameraPrefs
	{
		[XmlAttribute("name")]
		public String Name;

        [XmlAttribute("focus")]
        public int Focus;

        [XmlAttribute("autoFocus")]
        public Boolean AutoFocus;

        [XmlAttribute("exposure")]
        public int Exposure;

        [XmlAttribute("autoExposure")]
		public Boolean AutoExposure;

        [XmlAttribute("zoom")]
        public int Zoom;

        [XmlAttribute("pan")]
		public int Pan;

		[XmlAttribute("tilt")]
		public int Tilt;

        [XmlAttribute("iris")]
        public int Iris;

        [XmlAttribute("roll")]
        public int Roll;

        [XmlAttribute("backlightCompensation")]
        public int BacklightCompensation;

        [XmlAttribute("brightness")]
		public int Brightness;

        [XmlAttribute("colorEnable")]
        public Boolean colorEnable;

        [XmlAttribute("contrast")]
		public int Contrast;

        [XmlAttribute("gain")]
        public int Gain;

        [XmlAttribute("gamma")]
        public int Gamma;

        [XmlAttribute("hue")]
        public int Hue;

        [XmlAttribute("saturation")]
		public int Saturation;

        [XmlAttribute("sharpness")]
        public int Sharpness;

        [XmlAttribute("whiteBalance")]
		public int WhiteBalance;
	}
}
