let audioContext = null
let audioBuffer = null
let audioSource = null
let audioAnalyser = null
let hasAudioStarted = false

function initWebAudioApi() {
	try {
		window.AudioContext = window.AudioContext || window.webkitAudioContext
		audioContext = new AudioContext()

		audioAnalyser = audioContext.createAnalyser()
		audioAnalyser.connect(audioContext.destination)
	}
	catch(e) {
		alert('Web Audio API is not supported in this browser!')
	}
}

function configSource() {
	audioSource = audioContext.createBufferSource()
	audioSource.buffer = audioBuffer
	audioSource.connect(audioAnalyser)
}

window.getWindowDimensions = () => (
	{
		x: window.innerWidth,
		y: window.innerHeight
	}
)

window.focusOnElement = (element) => element.focus()

window.loadAudio = async (audioUrl) => {
	initWebAudioApi()
	
	await new Promise((resolve, reject) => {
		const request = new XMLHttpRequest()

		request.open('GET', audioUrl, true)
		request.responseType = 'arraybuffer'
		request.onload = async () => {
			const buffer = await audioContext.decodeAudioData(request.response)
			audioBuffer = buffer
			resolve()
		}

		request.send()
	})

	configSource()
}

window.playAudio = () => {
	if (!hasAudioStarted) {
		audioSource.start(0)
		hasAudioStarted = true
	}
	else
		audioContext.resume()
}

window.pauseAudio = () => {
	audioContext.suspend()
}

window.resetAudio = () => {
	if (audioSource)
		audioSource.stop()

	configSource()

	hasAudioStarted = false
}

window.getAudioFrequencyData = () => {
	let audioFrequencyData = new Uint8Array(audioAnalyser.frequencyBinCount)

	audioAnalyser.getByteFrequencyData(audioFrequencyData)
	const audioFrequencyDataString = audioFrequencyData.join(';')

	return audioFrequencyDataString
}
