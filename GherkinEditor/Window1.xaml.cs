// Copyright (c) 2009 Daniel Grunwald
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.Win32;

namespace GherkinEditor
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
        private Languages _languages;  

		public Window1()
		{
			// Load our custom highlighting definition
			IHighlightingDefinition customHighlighting;
			using (Stream s = typeof(Window1).Assembly.GetManifestResourceStream("GherkinEditor.CustomHighlighting.xshd")) {
				if (s == null)
					throw new InvalidOperationException("Could not find embedded resource");
				using (XmlReader reader = new XmlTextReader(s)) {
					customHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.
						HighlightingLoader.Load(reader, HighlightingManager.Instance);
				}
			}
			// and register it in the HighlightingManager
			HighlightingManager.Instance.RegisterHighlighting("Custom Highlighting", new string[] { ".cool" }, customHighlighting);
			
			InitializeComponent();			
			textEditor.SyntaxHighlighting = customHighlighting;

			textEditor.TextArea.TextEntering += textEditor_TextArea_TextEntering;
			textEditor.TextArea.TextEntered += textEditor_TextArea_TextEntered;
			
			DispatcherTimer foldingUpdateTimer = new DispatcherTimer();
			foldingUpdateTimer.Interval = TimeSpan.FromSeconds(2);
			foldingUpdateTimer.Tick += foldingUpdateTimer_Tick;
			foldingUpdateTimer.Start();
            _languages = new Languages();
		    _completionDataLoader = new CompletionDataLoader();
		}

		string currentFileName;
		
		void openFileClick(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.CheckFileExists = true;
			if (dlg.ShowDialog() ?? false) {
				currentFileName = dlg.FileName;
				textEditor.Load(currentFileName);
				textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(Path.GetExtension(currentFileName));
			}
		}
		
		void saveFileClick(object sender, EventArgs e)
		{
			if (currentFileName == null) {
				SaveFileDialog dlg = new SaveFileDialog();
				dlg.DefaultExt = ".txt";
				if (dlg.ShowDialog() ?? false) {
					currentFileName = dlg.FileName;
				} else {
					return;
				}
			}
			textEditor.Save(currentFileName);
		}
				
		CompletionWindow completionWindow;
		
        protected override void  OnKeyDown(KeyEventArgs e)
        {
            if (prevKey == Key.LeftCtrl && e.Key == Key.Space)
            {
                prevKey = null;
                showAutoComplete = true;
            }
            else
            {
                prevKey = e.Key;
                showAutoComplete = false;
                base.OnKeyDown(e);
            }
        }

		void textEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
		{
			if (showAutoComplete){
				displayAutoComplete();
			}
		    showAutoComplete = false;
		}

	    private void displayAutoComplete()
	    {
            completionWindow = new CompletionWindow(textEditor.TextArea);
            var data = completionWindow.CompletionList.CompletionData;

            if (textEditor.TextArea.Caret.Line == 1 && hasLanguageLine())
            {
                _completionDataLoader.LoadLanguages(data,_languages);
            }else
            {
                var language = getLanguageToLoad();
                _completionDataLoader.LoadDataInto(data, language); 
            }

	        completionWindow.Show();
	        completionWindow.Closed += delegate {
	                                                completionWindow = null;
	        };
	    }

	    private bool hasLanguageLine()
	    {
            var line = textEditor.TextArea.Document.GetLineByNumber(1);
            var firstLineText = line.Text.Trim().ToLower();
	        return (firstLineText.StartsWith("#")
	                && firstLineText.Contains("language")
	                && firstLineText.Contains(":"));
	    }

	    private Language getLanguageToLoad()
	    {
            var isoCode = "en";
            if (hasLanguageLine())
                isoCode = textEditor.TextArea.Document.GetLineByNumber(1)
                    .Text.Trim().ToLower().Split(':')[1].Trim();

	        return _languages.Find(l => l.IsoCode == isoCode) 
	                       ?? new Language("en", "English", "English");
	    }

	    void textEditor_TextArea_TextEntering(object sender, TextCompositionEventArgs e)
		{
			if (e.Text.Length > 0 && completionWindow != null) {
				if (!char.IsLetterOrDigit(e.Text[0])) {
					completionWindow.CompletionList.RequestInsertion(e);
				}
			}
		}
		
		#region Folding
		FoldingManager foldingManager;
		AbstractFoldingStrategy foldingStrategy;
	    private Key? prevKey;
	    private bool showAutoComplete;
	    private CompletionDataLoader _completionDataLoader;

	    void foldingUpdateTimer_Tick(object sender, EventArgs e)
		{
			if (foldingStrategy != null) {
				foldingStrategy.UpdateFoldings(foldingManager, textEditor.Document);
			}
		}
		#endregion
	}
}