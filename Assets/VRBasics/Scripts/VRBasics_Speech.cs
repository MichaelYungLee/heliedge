
//========================== VRBasics_Speech =================================
//
// Used for speech recognition with Windows Speech API
//
//=========================== by Zac Zidik ====================================

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

public class VRBasics_Speech : MonoBehaviour {

	//a list of possible speech recognition methods
	public enum recognitionMethods{Dictation, Grammar, Keyword};
	//instance of the list
	public recognitionMethods recognitionMethod;
	//a list of minimum confidence levels for speech to confirm as recognized
	public enum confidenceLevels{High, Medium, Low, Rejected};
	public confidenceLevels confidenceLevel; 


	//DICTION
	//the amount of seconds of silence, while the dictation recognizer is running, before it times out
	public float timeOutSeconds = 2.0f;
	//the amount of intial seconds of silence, after the dictation recognizer has been started, before it times out
	public float initialTimeOutSeconds = 2.0f;
	//a recognizer for speech using the dictation method, must be started
	public DictationRecognizer dictationRecognizer;
	//gives the ability to create a custom dictation results handler
	//call by the DictationRecognizer_DictationResult function
	public delegate void DictationResults(string text, ConfidenceLevel confidence);
	public DictationResults dictionResults;

	public delegate void DictationHypothesis(string text);
	public DictationHypothesis dictionHypothesis;


	//PHRASE RECOGNITION METHODS
	///////////////////////////////////////////////////////////////////////
	//GRAMMAR
	//currently only works on Windows 10
	//a recognizer for speech using a grammar file
	public GrammarRecognizer grammarRecognizer;
	//the name of the grammar xml file stored in the StreamingAssets folder
	public string grammarFileName;
	//gives the ability to create a custom phrase recognized handler
	//call by the GrammarRecognizer_OnPhraseRecognized function
	public delegate void PhraseRecognized(PhraseRecognizedEventArgs args);
	public PhraseRecognized phraseRecognized;


	//KEYWORD
	private KeywordRecognizer keywordRecognizer;
	public string[] keywords;
	public delegate void KeywordRecognized(PhraseRecognizedEventArgs args);
	public KeywordRecognized keywordRecognized;
	///////////////////////////////////////////////////////////////////////



	void Start(){

		//DICTION
		if (recognitionMethod == recognitionMethods.Dictation) {

			//create the diction recognizer, with the proper minimum confidence
			switch (confidenceLevel) {
			case confidenceLevels.High:
				dictationRecognizer = new DictationRecognizer(UnityEngine.Windows.Speech.ConfidenceLevel.High);
				break;
			case confidenceLevels.Medium:
				dictationRecognizer = new DictationRecognizer(UnityEngine.Windows.Speech.ConfidenceLevel.Medium);
				break;
			case confidenceLevels.Low:
				dictationRecognizer = new DictationRecognizer(UnityEngine.Windows.Speech.ConfidenceLevel.Low);
				break;
			case confidenceLevels.Rejected:
				dictationRecognizer = new DictationRecognizer(UnityEngine.Windows.Speech.ConfidenceLevel.Rejected);
				break;
			}

			//create the function that runs when diction gets a result
			dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
			//create the function that runs while diction is guessing what is being said
			dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;
			//create the function that runs when diction is complete, mainly to indicate if diction has timed out
			dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
			//create the function that checks for diction errors
			dictationRecognizer.DictationError += DictationRecognizer_DictationError;
			//assign the amount of seconds of silence, while the dictation recognizer is running, before it times out
			dictationRecognizer.AutoSilenceTimeoutSeconds = timeOutSeconds;
			//assign the amount of intial seconds of silence, after the dictation recognizer has been started, before it times out
			dictationRecognizer.InitialSilenceTimeoutSeconds = initialTimeOutSeconds;


		//PHRASE RECOGNITION METHODS
		///////////////////////////////////////////////////////////////////////
		//GRAMMAR
		} else if (recognitionMethod == recognitionMethods.Grammar) {

			//create the grammar recognizer, load the grammar file, set minimum confidence
			switch (confidenceLevel) {
			case confidenceLevels.High:
				grammarRecognizer = new GrammarRecognizer (Application.streamingAssetsPath + "/" + grammarFileName + ".xml", UnityEngine.Windows.Speech.ConfidenceLevel.High);
				break;
			case confidenceLevels.Medium:
				grammarRecognizer = new GrammarRecognizer (Application.streamingAssetsPath + "/" + grammarFileName + ".xml", UnityEngine.Windows.Speech.ConfidenceLevel.Medium);
				break;
			case confidenceLevels.Low:
				grammarRecognizer = new GrammarRecognizer (Application.streamingAssetsPath + "/" + grammarFileName + ".xml", UnityEngine.Windows.Speech.ConfidenceLevel.Low);
				break;
			case confidenceLevels.Rejected:
				grammarRecognizer = new GrammarRecognizer (Application.streamingAssetsPath + "/" + grammarFileName + ".xml", UnityEngine.Windows.Speech.ConfidenceLevel.Rejected);
				break;
			}

			//create the function that runs when a phrase from the grammar file is recognized
			grammarRecognizer.OnPhraseRecognized += GrammarRecognizer_OnPhraseRecognized;
			//start the grammar recognizer
			grammarRecognizer.Start ();	

			PhraseRecognitionSystem.OnStatusChanged += SpeechStatusChanged;

		//KEYWORD
		} else if (recognitionMethod == recognitionMethods.Keyword) {

			//create the keyword recognizer, load the keywords, set minimum confidence
			switch (confidenceLevel) {
			case confidenceLevels.High:
				keywordRecognizer = new KeywordRecognizer (keywords, UnityEngine.Windows.Speech.ConfidenceLevel.High);
				break;
			case confidenceLevels.Medium:
				keywordRecognizer = new KeywordRecognizer (keywords, UnityEngine.Windows.Speech.ConfidenceLevel.Medium);
				break;
			case confidenceLevels.Low:
				keywordRecognizer = new KeywordRecognizer (keywords, UnityEngine.Windows.Speech.ConfidenceLevel.Low);
				break;
			case confidenceLevels.Rejected:
				keywordRecognizer = new KeywordRecognizer (keywords, UnityEngine.Windows.Speech.ConfidenceLevel.Rejected);
				break;
			}
				
			//create the function that runs when a keyword from the dictionary is recognized
			keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
			//start the keyword recognizer
			keywordRecognizer.Start ();

			PhraseRecognitionSystem.OnStatusChanged += SpeechStatusChanged;
		}
		///////////////////////////////////////////////////////////////////////
	}


	//DICTION
	void DictationRecognizer_DictationResult (string text, ConfidenceLevel confidence)
	{
		//create your own cutom handler to deal with the results of dictaion
		dictionResults (text, confidence);
	}

	void DictationRecognizer_DictationHypothesis (string text)
	{
		dictionHypothesis (text);
		Debug.Log("Dictation Hypothesis: " + text);
	}

	void DictationRecognizer_DictationComplete (DictationCompletionCause cause)
	{
		if (cause != DictationCompletionCause.Complete){
			Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", cause);
		}
	}

	void DictationRecognizer_DictationError (string error, int hresult)
	{
		Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
	}



	//PHRASE RECOGNITION METHODS
	///////////////////////////////////////////////////////////////////////
	//args.confidence - a measure of correct recognition certainty
	//args.phraseDuration - the time it took for the phrase to be uttered
	//args.phraseStartTime - the moment in time when uttering of the phrase began
	//args.semanticMeanings - a semantic meaning of recognized phrase
	//args.text - the text that was recognized

	//ex. show text of recognized phrase
	//Debug.Log(args.text);

	//ex. show semantic meaning info of recognized phrase
	//Debug.Log (args.semanticMeanings[0].values[0]);

	//ex. check if the text of recognized phrase contains a word or phrase
	/*
	if (args.text.Contains ("Bridget")) {
		Debug.Log ("Yes the phrase contains Bridget");
	}
	*/

	//GRAMMAR
	void GrammarRecognizer_OnPhraseRecognized (PhraseRecognizedEventArgs args){

		//create your own cutom handler to deal with the results of a recognized phrase
		phraseRecognized (args);

	}

	//KEYWORD
	void KeywordRecognizer_OnPhraseRecognized (PhraseRecognizedEventArgs args)
	{
		//create your own cutom handler to deal with the results of a recognized keyword
		keywordRecognized (args);
	}

	void SpeechStatusChanged(SpeechSystemStatus status){

		Debug.Log (status);
	}
	///////////////////////////////////////////////////////////////////////
}

