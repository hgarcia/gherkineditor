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
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.Win32;

namespace GherkinEditor
{
	public partial class Window1
	{
        private readonly Languages _languages;  

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
			HighlightingManager.Instance.RegisterHighlighting("Custom Highlighting", new[] { ".feature" }, customHighlighting);
			
			InitializeComponent();			
			textEditor.SyntaxHighlighting = customHighlighting;

			textEditor.TextArea.TextEntering += Text_editor_text_area_text_entering;
			textEditor.TextArea.TextEntered += Text_editor_text_area_text_entered;
			
			var foldingUpdateTimer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(2)};
		    foldingUpdateTimer.Tick += Folding_update_timer_tick;
			foldingUpdateTimer.Start();
            _languages = new Languages();
		    _completionDataLoader = new CompletionDataLoader();
		}

		string _currentFileName;
		
		void openFileClick(object sender, RoutedEventArgs e)
		{
			var dlg = new OpenFileDialog {CheckFileExists = true};
		    if (!((bool) dlg.ShowDialog())) return;
		    _currentFileName = dlg.FileName;
		    textEditor.Load(_currentFileName);
		    textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(Path.GetExtension(_currentFileName));
		}
		
		void saveFileClick(object sender, EventArgs e)
		{
			if (_currentFileName == null) {
				var dlg = new SaveFileDialog {DefaultExt = ".txt"};
			    if ((bool) dlg.ShowDialog()) {
					_currentFileName = dlg.FileName;
				} else {
					return;
				}
			}
			textEditor.Save(_currentFileName);
		}
				
		CompletionWindow _completionWindow;
		
        protected override void OnKeyDown(KeyEventArgs e)
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

	    void Text_editor_text_area_text_entered(object sender, TextCompositionEventArgs e)
		{
			if (showAutoComplete){
				displayAutoComplete();
			}
		    showAutoComplete = false;
		}

	    private void displayAutoComplete()
	    {
            _completionWindow = new CompletionWindow(textEditor.TextArea);
            var data = _completionWindow.CompletionList.CompletionData;

            if (textEditor.TextArea.Caret.Line == 1 && hasLanguageLine())
            {
                _completionDataLoader.LoadLanguages(data,_languages);
            }else
            {
                var language = getLanguageToLoad();
                _completionDataLoader.LoadDataInto(data, language); 
            }

	        _completionWindow.Show();
	        _completionWindow.Closed += delegate { _completionWindow = null;};
	    }

	    private bool hasLanguageLine()
	    {
            var line = getLineText(1).Trim().ToLower();
            return (line.StartsWith("#")
                    && line.Contains("language")
                    && line.Contains(":"));
	    }

        private string getDocumentIsoCode()
        {
            if (hasLanguageLine())
            {
                var line = getLineText(1).Trim().ToLower();
                return line.Split(':')[1].Trim();
            }
            return "en";
        }

	    private Language getLanguageToLoad()
	    {
	        var isoCode = getDocumentIsoCode();
	        return _languages.Find(l => l.IsoCode == isoCode) 
	                       ?? new Language("en", "English", "English");
	    }

        private string getLineText(int lineNumber)
        {
            var document = textEditor.TextArea.Document;
            var line = document.GetLineByNumber(lineNumber);
            return document.GetText(line);
        }

	    void Text_editor_text_area_text_entering(object sender, TextCompositionEventArgs e)
	    {
	        if (e.Text.Length <= 0 || _completionWindow == null) return;
	        if (!char.IsLetterOrDigit(e.Text[0])) {
	            _completionWindow.CompletionList.RequestInsertion(e);
	        }
	    }

	    #region Folding
		FoldingManager foldingManager;
		AbstractFoldingStrategy foldingStrategy;
	    private Key? prevKey;
	    private bool showAutoComplete;
	    private readonly CompletionDataLoader _completionDataLoader;

	    void Folding_update_timer_tick(object sender, EventArgs e)
		{
			if (foldingStrategy != null) {
				foldingStrategy.UpdateFoldings(foldingManager, textEditor.Document);
			}
		}
		#endregion
	}
}