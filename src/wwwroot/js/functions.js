let audioContext = null
let audioBuffer = null
let audioSource = null
let hasAudioStarted = false

function init() {
	try {
		window.AudioContext = window.AudioContext || window.webkitAudioContext
		audioContext = new AudioContext()
	}
	catch(e) {
		alert('Web Audio API is not supported in this browser!')
	}
}

window.addEventListener('load', init)

window.getWindowDimensions = () => (
	{
		x: window.innerWidth,
		y: window.innerHeight
	}
)

window.focusOnElement = (element) => element.focus()

window.loadAudio = async (audioUrl) => {
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

	audioSource = audioContext.createBufferSource()
	audioSource.buffer = audioBuffer
	audioSource.connect(audioContext.destination)
}

window.playAudio = () => {
	if (hasAudioStarted)
		audioContext.resume()
	else {
		audioSource.start(0)
		hasAudioStarted = true
	}
}

window.pauseAudio = () => {
	audioContext.suspend()
}

window.resetAudio = () => {
	if (audioSource)
		audioSource.stop()

	audioSource = audioContext.createBufferSource()
	audioSource.buffer = audioBuffer
	audioSource.connect(audioContext.destination)

	hasAudioStarted = false
}
