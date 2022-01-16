using System;

namespace audiovisual_pong.Models
{
	public class FrequencyModel {

		public FrequencyModel() {
			ISampleSource source= CodecFactory.Instance.GetCodec("/audio/bensound-scifi.mp3").ToSampleSource();
			FftProvider provider = new FftProvider(source.WaveFormat.Channels, FftSize.Fft2048);
			float[] buffer = new float[(int) FftSize.Fft2048];
			if (provider.GetFftData(buffer))
			{
				Console.WriteLine(buffer);
			}
		}
		
	}

}