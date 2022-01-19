using System;
using NAudio;
using NAudio.dsp;

namespace audiovisual_pong.Models
{
	public class FrequencyModel {

		public FrequencyModel() {
			int fftLength = 2048;
			Complex[] buffer = new Complex[fftLength];
			Console.WriteLine($"{buffer.GetType()}");


			/*
			ISampleSource source= CodecFactory.Instance.GetCodec(@"https://dcs.megaphone.fm/VMP8879973005.mp3
").ToSampleSource();
			FftProvider provider = new FftProvider(source.WaveFormat.Channels, FftSize.Fft2048);
			float[] buffer = new float[(int) FftSize.Fft2048];
			if (provider.GetFftData(buffer))
			{
				Console.WriteLine(buffer);
			}
			*/
		}
		
	}

}