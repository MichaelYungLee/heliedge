using UnityEngine;
using System.Collections;

public class SpeechInteractions : MonoBehaviour {

	private VRBasics_Speech speech;
	private string speechResult = "";

	// Use this for initialization
	void Start () {

		speech = GetComponent<VRBasics_Speech> ();

		//DICTATION
		if (speech.recognitionMethod == VRBasics_Speech.recognitionMethods.Dictation) {

			//define a custom handler for dictation results
			speech.dictionResults += HandleDictationResults;
			//define a custom handler for diction hypothesis
			speech.dictionHypothesis += HandleDictationHypothesis;
		}


		//GRAMMAR
		if (speech.recognitionMethod == VRBasics_Speech.recognitionMethods.Grammar) {

			//define a custom handler for when a phrase is recognized
			speech.phraseRecognized += HandlePhraseRecognized;
		}

		//KEYWORD
		if (speech.recognitionMethod == VRBasics_Speech.recognitionMethods.Keyword) {

			//define a custom handler for when a keyword is recognized
			speech.keywordRecognized += HandleKeywordRecognized;
		}
	}


	void OnGUI(){

		if (speech.recognitionMethod == VRBasics_Speech.recognitionMethods.Dictation) {

			if (speech.dictationRecognizer.Status == UnityEngine.Windows.Speech.SpeechSystemStatus.Stopped || speech.dictationRecognizer.Status == UnityEngine.Windows.Speech.SpeechSystemStatus.Failed) {

				//create a button to start the diction recognizer
				if (GUI.Button (new Rect (10, 40, 200, 50), "Start Diction")) {
					
					speechResult = "";
					speech.dictationRecognizer.Start ();
				}
			} else {

				//create a button to start the diction recognizer
				if (GUI.Button (new Rect (210, 40, 200, 50), "Stop Diction")) {

					speech.dictationRecognizer.Stop ();
				}
			}
		}

		speechResult = GUI.TextField(new Rect(10, 10, 400, 20), speechResult, 100);
	}

	void HandleDictationResults (string text, UnityEngine.Windows.Speech.ConfidenceLevel confidence)
	{
		speechResult = text;
		//Debug.Log("Dictation Result: " + text + ", Confidence: " + confidence);
	}

	void HandleDictationHypothesis (string text)
	{
		speechResult = text;
	}

	void HandlePhraseRecognized (UnityEngine.Windows.Speech.PhraseRecognizedEventArgs args)
	{
		speechResult = args.text;
	}

	void HandleKeywordRecognized (UnityEngine.Windows.Speech.PhraseRecognizedEventArgs args)
	{
		speechResult = args.text;
	}
}
