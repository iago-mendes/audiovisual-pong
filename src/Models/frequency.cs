using System;
using System.IO;
using CSCore;
using CSCore.DSP;
using CSCore.Codecs;

namespace audiovisual_pong.Models
{
	public class FrequencyModel {

		public FrequencyModel() {
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