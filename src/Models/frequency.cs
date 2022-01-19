using System;
using System.IO;
using CSCore;
using CSCore.DSP;
using CSCore.Codecs;

namespace audiovisual_pong.Models
{



	public class FrequencyModel {
		static void DirSearch(string sDir)
{
    try
    {
        foreach (string d in Directory.GetDirectories(sDir))
        {
            foreach (string f in Directory.GetFiles(d))
            {
                Console.WriteLine(f);
            }
            DirSearch(d);
        }
    }
    catch (System.Exception excpt)
    {
        Console.WriteLine(excpt.Message);
    }
}

		public FrequencyModel() {
			DirSearch("/");
			/*
			string[] dirs = Directory.GetDirectories(@"/home");
			foreach(string str in dirs) {
				Console.WriteLine($"dir: {str}");
			}*/
			ISampleSource source= CodecFactory.Instance.GetCodec(@"https://dcs.megaphone.fm/VMP8879973005.mp3
").ToSampleSource();
			FftProvider provider = new FftProvider(source.WaveFormat.Channels, FftSize.Fft2048);
			float[] buffer = new float[(int) FftSize.Fft2048];
			if (provider.GetFftData(buffer))
			{
				Console.WriteLine(buffer);
			}
		}
		
	}

}