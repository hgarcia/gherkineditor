﻿// Copyright (c) 2009 Daniel Grunwald
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
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace GherkinEditor
{
	public class GherkinCompletionItem : ICompletionData
	{
	    public GherkinCompletionItem(string text, string description) : this(text)
	    {
	        Description = description;
	    }
		public GherkinCompletionItem(string text)
		{
			Text = text;
		}
		
		public System.Windows.Media.ImageSource Image {
			get { return null; }
		}
		
		public string Text { get; private set; }
		
		// Use this property if you want to show a fancy UIElement in the drop down list.
		public object Content {
			get { return Text; }
		}

	    private object _description;
		public object Description
		{
		    get
		    {
                if(_description == null)
		            return "Description for " + Text;
		        return _description;
		    }
		    private set { _description = value; }
		}

	    public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
		{
			textArea.Document.Replace(completionSegment, Text);
		}
	}
}
