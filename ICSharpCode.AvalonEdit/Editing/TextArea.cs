// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <author name="Daniel Grunwald"/>
//     <version>$Revision: 4973 $</version>
// </file>

using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Indentation;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.AvalonEdit.Utils;

namespace ICSharpCode.AvalonEdit.Editing
{
	/// <summary>
	/// Control that wraps a TextView and adds support for user input and the caret.
	/// </summary>
	public class TextArea : Control, IScrollInfo, IWeakEventListener, ITextEditorComponent, IServiceProvider
	{
		#region Constructor
		static TextArea()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(TextArea),
			                                         new FrameworkPropertyMetadata(typeof(TextArea)));
			KeyboardNavigation.IsTabStopProperty.OverrideMetadata(
				typeof(TextArea), new FrameworkPropertyMetadata(Boxes.True));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(
				typeof(TextArea), new FrameworkPropertyMetadata(KeyboardNavigationMode.None));
			FocusableProperty.OverrideMetadata(
				typeof(TextArea), new FrameworkPropertyMetadata(Boxes.True));
		}
		
		/// <summary>
		/// Creates a new TextArea instance.
		/// </summary>
		public TextArea() : this(new TextView())
		{
		}
		
		/// <summary>
		/// Creates a new TextArea instance.
		/// </summary>
		protected TextArea(TextView textView)
		{
			if (textView == null)
				throw new ArgumentNullException("textView");
			this.textView = textView;
			this.Options = textView.Options;
			textView.SetBinding(TextView.DocumentProperty, new Binding(DocumentProperty.Name) { Source = this });
			textView.SetBinding(TextView.OptionsProperty, new Binding(OptionsProperty.Name) { Source = this });
			
			textView.Services.AddService(typeof(TextArea), this);
			
			textView.LineTransformers.Add(new SelectionColorizer(this));
			textView.InsertLayer(new SelectionLayer(this), KnownLayer.Selection, LayerInsertionPosition.Replace);
			
			caret = new Caret(this);
			caret.PositionChanged += (sender, e) => RequestSelectionValidation();
			
			leftMargins.CollectionChanged += leftMargins_CollectionChanged;
			
			this.DefaultInputHandler = new TextAreaDefaultInputHandler(this);
			this.ActiveInputHandler = this.DefaultInputHandler;
		}
		#endregion
		
		#region InputHandler management
		/// <summary>
		/// Gets the default input handler.
		/// </summary>
		public TextAreaDefaultInputHandler DefaultInputHandler { get; private set; }
		
		ITextAreaInputHandler activeInputHandler;
		
		/// <summary>
		/// Gets/Sets the active input handler.
		/// </summary>
		public ITextAreaInputHandler ActiveInputHandler {
			get { return activeInputHandler; }
			set {
				if (value != null && value.TextArea != this)
					throw new ArgumentException("The input handler was created for a different text area than this one.");
				if (activeInputHandler != value) {
					if (activeInputHandler != null)
						activeInputHandler.Detach();
					activeInputHandler = value;
					if (value != null)
						value.Attach();
					if (ActiveInputHandlerChanged != null)
						ActiveInputHandlerChanged(this, EventArgs.Empty);
				}
			}
		}
		
		/// <summary>
		/// Occurs when the ActiveInputHandler property changes.
		/// </summary>
		public event EventHandler ActiveInputHandlerChanged;
		#endregion
		
		#region Document property
		/// <summary>
		/// Document property.
		/// </summary>
		public static readonly DependencyProperty DocumentProperty
			= TextView.DocumentProperty.AddOwner(typeof(TextArea), new FrameworkPropertyMetadata(OnDocumentChanged));
		
		/// <summary>
		/// Gets/Sets the document displayed by the text editor.
		/// </summary>
		public TextDocument Document {
			get { return (TextDocument)GetValue(DocumentProperty); }
			set { SetValue(DocumentProperty, value); }
		}
		
		/// <inheritdoc/>
		public event EventHandler DocumentChanged;
		
		static void OnDocumentChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			((TextArea)dp).OnDocumentChanged((TextDocument)e.OldValue, (TextDocument)e.NewValue);
		}
		
		void OnDocumentChanged(TextDocument oldValue, TextDocument newValue)
		{
			if (oldValue != null) {
				TextDocumentWeakEventManager.Changing.RemoveListener(oldValue, this);
				TextDocumentWeakEventManager.Changed.RemoveListener(oldValue, this);
				TextDocumentWeakEventManager.UpdateStarted.RemoveListener(oldValue, this);
			}
			if (newValue != null) {
				TextDocumentWeakEventManager.Changing.AddListener(newValue, this);
				TextDocumentWeakEventManager.Changed.AddListener(newValue, this);
				TextDocumentWeakEventManager.UpdateStarted.AddListener(newValue, this);
			}
			if (DocumentChanged != null)
				DocumentChanged(this, EventArgs.Empty);
			CommandManager.InvalidateRequerySuggested();
		}
		#endregion
		
		#region Options property
		/// <summary>
		/// Options property.
		/// </summary>
		public static readonly DependencyProperty OptionsProperty
			= TextView.OptionsProperty.AddOwner(typeof(TextArea), new FrameworkPropertyMetadata(OnOptionsChanged));
		
		/// <summary>
		/// Gets/Sets the document displayed by the text editor.
		/// </summary>
		public TextEditorOptions Options {
			get { return (TextEditorOptions)GetValue(OptionsProperty); }
			set { SetValue(OptionsProperty, value); }
		}
		
		/// <summary>
		/// Occurs when a text editor option has changed.
		/// </summary>
		public event PropertyChangedEventHandler OptionChanged;
		
		/// <summary>
		/// Raises the <see cref="OptionChanged"/> event.
		/// </summary>
		protected virtual void OnOptionChanged(PropertyChangedEventArgs e)
		{
			if (OptionChanged != null) {
				OptionChanged(this, e);
			}
		}
		
		static void OnOptionsChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			((TextArea)dp).OnOptionsChanged((TextEditorOptions)e.OldValue, (TextEditorOptions)e.NewValue);
		}
		
		void OnOptionsChanged(TextEditorOptions oldValue, TextEditorOptions newValue)
		{
			if (oldValue != null) {
				PropertyChangedWeakEventManager.RemoveListener(oldValue, this);
			}
			if (newValue != null) {
				PropertyChangedWeakEventManager.AddListener(newValue, this);
			}
			OnOptionChanged(new PropertyChangedEventArgs(null));
		}
		#endregion
		
		#region ReceiveWeakEvent
		/// <inheritdoc/>
		protected virtual bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			if (managerType == typeof(TextDocumentWeakEventManager.Changing)) {
				caret.OnDocumentChanging();
				return true;
			} else if (managerType == typeof(TextDocumentWeakEventManager.Changed)) {
				OnDocumentChanged((DocumentChangeEventArgs)e);
				return true;
			} else if (managerType == typeof(TextDocumentWeakEventManager.UpdateStarted)) {
				OnUpdateStarted();
				return true;
			} else if (managerType == typeof(PropertyChangedWeakEventManager)) {
				OnOptionChanged((PropertyChangedEventArgs)e);
				return true;
			}
			return false;
		}
		
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return ReceiveWeakEvent(managerType, sender, e);
		}
		#endregion
		
		#region Caret handling on document changes
		void OnDocumentChanged(DocumentChangeEventArgs e)
		{
			caret.OnDocumentChanged(e);
			this.Selection = selection.UpdateOnDocumentChange(e);
		}
		
		void OnUpdateStarted()
		{
			Document.UndoStack.PushOptional(new RestoreCaretAndSelectionUndoAction(this));
		}
		
		sealed class RestoreCaretAndSelectionUndoAction : IUndoableOperation
		{
			// keep textarea in weak reference because the IUndoableOperation is stored with the document
			WeakReference textAreaReference;
			TextViewPosition caretPosition;
			Selection selection;
			
			public RestoreCaretAndSelectionUndoAction(TextArea textArea)
			{
				this.textAreaReference = new WeakReference(textArea);
				this.caretPosition = textArea.Caret.Position;
				this.selection = textArea.Selection;
			}
			
			public void Undo()
			{
				TextArea textArea = (TextArea)textAreaReference.Target;
				if (textArea != null) {
					textArea.Caret.Position = caretPosition;
					textArea.Selection = selection;
				}
			}
			
			public void Redo()
			{
				// redo=undo: we just restore the caret/selection state
				Undo();
			}
		}
		#endregion
		
		#region TextView property
		readonly TextView textView;
		IScrollInfo scrollInfo;

		/// <summary>
		/// Gets the text view used to display text in this text area.
		/// </summary>
		public TextView TextView {
			get {
				return textView;
			}
		}
		/// <inheritdoc/>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			scrollInfo = textView;
			ApplyScrollInfo();
		}
		#endregion
		
		#region Selection property
		Selection selection = Selection.Empty;
		
		/// <summary>
		/// Occurs when the selection has changed.
		/// </summary>
		public event EventHandler SelectionChanged;
		
		/// <summary>
		/// Gets/Sets the selection in this text area.
		/// </summary>
		public Selection Selection {
			get { return selection; }
			set {
				if (value == null)
					throw new ArgumentNullException("value");
				if (!object.Equals(selection, value)) {
					Debug.WriteLine("Selection change from " + selection + " to " + value);
					if (textView != null) {
						textView.Redraw(selection.SurroundingSegment, DispatcherPriority.Background);
						textView.Redraw(value.SurroundingSegment, DispatcherPriority.Background);
					}
					selection = value;
					if (SelectionChanged != null)
						SelectionChanged(this, EventArgs.Empty);
					// a selection change causes commands like copy/paste/etc. to change status
					CommandManager.InvalidateRequerySuggested();
				}
			}
		}
		
		/// <summary>
		/// The <see cref="SelectionBrush"/> property.
		/// </summary>
		public static readonly DependencyProperty SelectionBrushProperty =
			DependencyProperty.Register("SelectionBrush", typeof(Brush), typeof(TextArea));
		
		/// <summary>
		/// Gets/Sets the background brush used for the selection.
		/// </summary>
		public Brush SelectionBrush {
			get { return (Brush)GetValue(SelectionBrushProperty); }
			set { SetValue(SelectionBrushProperty, value); }
		}
		
		/// <summary>
		/// The <see cref="SelectionForeground"/> property.
		/// </summary>
		public static readonly DependencyProperty SelectionForegroundProperty =
			DependencyProperty.Register("SelectionForeground", typeof(Brush), typeof(TextArea));
		
		/// <summary>
		/// Gets/Sets the foreground brush used selected text.
		/// </summary>
		public Brush SelectionForeground {
			get { return (Brush)GetValue(SelectionForegroundProperty); }
			set { SetValue(SelectionForegroundProperty, value); }
		}
		
		/// <summary>
		/// The <see cref="SelectionBorder"/> property.
		/// </summary>
		public static readonly DependencyProperty SelectionBorderProperty =
			DependencyProperty.Register("SelectionBorder", typeof(Pen), typeof(TextArea));
		
		/// <summary>
		/// Gets/Sets the background brush used for the selection.
		/// </summary>
		public Pen SelectionBorder {
			get { return (Pen)GetValue(SelectionBorderProperty); }
			set { SetValue(SelectionBorderProperty, value); }
		}
		
		/// <summary>
		/// The <see cref="SelectionCornerRadius"/> property.
		/// </summary>
		public static readonly DependencyProperty SelectionCornerRadiusProperty =
			DependencyProperty.Register("SelectionCornerRadius", typeof(double), typeof(TextArea),
			                            new FrameworkPropertyMetadata(3.0));
		
		/// <summary>
		/// Gets/Sets the corner radius of the selection.
		/// </summary>
		public double SelectionCornerRadius {
			get { return (double)GetValue(SelectionCornerRadiusProperty); }
			set { SetValue(SelectionCornerRadiusProperty, value); }
		}
		#endregion
		
		#region Force caret to stay inside selection
		bool ensureSelectionValidRequested;
		int allowCaretOutsideSelection;
		
		void RequestSelectionValidation()
		{
			if (!ensureSelectionValidRequested && allowCaretOutsideSelection == 0) {
				ensureSelectionValidRequested = true;
				Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(EnsureSelectionValid));
			}
		}
		
		/// <summary>
		/// Code that updates only the caret but not the selection can cause confusion when
		/// keys like 'Delete' delete the (possibly invisible) selected text and not the
		/// text around the caret.
		/// 
		/// So we'll ensure that the caret is inside the selection.
		/// (when the caret is not in the selection, we'll clear the selection)
		/// 
		/// This method is invoked using the Dispatcher so that code may temporarily violate this rule
		/// (e.g. most 'extend selection' methods work by first setting the caret, then the selection),
		/// it's sufficient to fix it after any event handlers have run.
		/// </summary>
		void EnsureSelectionValid()
		{
			ensureSelectionValidRequested = false;
			if (allowCaretOutsideSelection == 0) {
				if (!selection.IsEmpty && !selection.Contains(caret.Offset)) {
					Debug.WriteLine("Resetting selection because caret is outside");
					this.Selection = Selection.Empty;
				}
			}
		}
		
		/// <summary>
		/// Temporarily allows positioning the caret outside the selection.
		/// Dispose the returned IDisposable to revert the allowance.
		/// </summary>
		/// <remarks>
		/// The text area only forces the caret to be inside the selection when other events
		/// have finished running (using the dispatcher), so you don't have to use this method
		/// for temporarily positioning the caret in event handlers.
		/// This method is only necessary if you want to run the WPF dispatcher, e.g. if you
		/// perform a drag'n'drop operation.
		/// </remarks>
		public IDisposable AllowCaretOutsideSelection()
		{
			VerifyAccess();
			allowCaretOutsideSelection++;
			return new CallbackOnDispose(
				delegate {
					VerifyAccess();
					allowCaretOutsideSelection--;
					RequestSelectionValidation();
				});
		}
		#endregion
		
		#region Properties
		readonly Caret caret;
		
		/// <summary>
		/// Gets the Caret used for this text area.
		/// </summary>
		public Caret Caret {
			get { return caret; }
		}
		
		ObservableCollection<UIElement> leftMargins = new ObservableCollection<UIElement>();
		
		/// <summary>
		/// Gets the collection of margins displayed to the left of the text view.
		/// </summary>
		public ObservableCollection<UIElement> LeftMargins {
			get {
				return leftMargins;
			}
		}
		
		void leftMargins_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.OldItems != null) {
				foreach (ITextViewConnect c in e.OldItems.OfType<ITextViewConnect>()) {
					c.RemoveFromTextView(textView);
				}
			}
			if (e.NewItems != null) {
				foreach (ITextViewConnect c in e.NewItems.OfType<ITextViewConnect>()) {
					c.AddToTextView(textView);
				}
			}
		}
		
		IReadOnlySectionProvider readOnlySectionProvider = NoReadOnlySections.Instance;
		
		/// <summary>
		/// Gets/Sets an object that provides read-only sections for the text area.
		/// </summary>
		public IReadOnlySectionProvider ReadOnlySectionProvider {
			get { return readOnlySectionProvider; }
			set {
				if (value == null)
					throw new ArgumentNullException("value");
				readOnlySectionProvider = value;
			}
		}
		#endregion
		
		#region IScrollInfo implementation
		ScrollViewer scrollOwner;
		bool canVerticallyScroll, canHorizontallyScroll;
		
		void ApplyScrollInfo()
		{
			if (scrollInfo != null) {
				scrollInfo.ScrollOwner = scrollOwner;
				scrollInfo.CanVerticallyScroll = canVerticallyScroll;
				scrollInfo.CanHorizontallyScroll = canHorizontallyScroll;
				scrollOwner = null;
			}
		}
		
		bool IScrollInfo.CanVerticallyScroll {
			get { return scrollInfo != null ? scrollInfo.CanVerticallyScroll : false; }
			set {
				canVerticallyScroll = value;
				if (scrollInfo != null)
					scrollInfo.CanVerticallyScroll = value;
			}
		}
		
		bool IScrollInfo.CanHorizontallyScroll {
			get { return scrollInfo != null ? scrollInfo.CanHorizontallyScroll : false; }
			set {
				canHorizontallyScroll = value;
				if (scrollInfo != null)
					scrollInfo.CanHorizontallyScroll = value;
			}
		}
		
		double IScrollInfo.ExtentWidth {
			get { return scrollInfo != null ? scrollInfo.ExtentWidth : 0; }
		}
		
		double IScrollInfo.ExtentHeight {
			get { return scrollInfo != null ? scrollInfo.ExtentHeight : 0; }
		}
		
		double IScrollInfo.ViewportWidth {
			get { return scrollInfo != null ? scrollInfo.ViewportWidth : 0; }
		}
		
		double IScrollInfo.ViewportHeight {
			get { return scrollInfo != null ? scrollInfo.ViewportHeight : 0; }
		}
		
		double IScrollInfo.HorizontalOffset {
			get { return scrollInfo != null ? scrollInfo.HorizontalOffset : 0; }
		}
		
		double IScrollInfo.VerticalOffset {
			get { return scrollInfo != null ? scrollInfo.VerticalOffset : 0; }
		}
		
		ScrollViewer IScrollInfo.ScrollOwner {
			get { return scrollInfo != null ? scrollInfo.ScrollOwner : null; }
			set {
				if (scrollInfo != null)
					scrollInfo.ScrollOwner = value;
				else
					scrollOwner = value;
			}
		}
		
		void IScrollInfo.LineUp()
		{
			if (scrollInfo != null) scrollInfo.LineUp();
		}
		
		void IScrollInfo.LineDown()
		{
			if (scrollInfo != null) scrollInfo.LineDown();
		}
		
		void IScrollInfo.LineLeft()
		{
			if (scrollInfo != null) scrollInfo.LineLeft();
		}
		
		void IScrollInfo.LineRight()
		{
			if (scrollInfo != null) scrollInfo.LineRight();
		}
		
		void IScrollInfo.PageUp()
		{
			if (scrollInfo != null) scrollInfo.PageUp();
		}
		
		void IScrollInfo.PageDown()
		{
			if (scrollInfo != null) scrollInfo.PageDown();
		}
		
		void IScrollInfo.PageLeft()
		{
			if (scrollInfo != null) scrollInfo.PageLeft();
		}
		
		void IScrollInfo.PageRight()
		{
			if (scrollInfo != null) scrollInfo.PageRight();
		}
		
		void IScrollInfo.MouseWheelUp()
		{
			if (scrollInfo != null) scrollInfo.MouseWheelUp();
		}
		
		void IScrollInfo.MouseWheelDown()
		{
			if (scrollInfo != null) scrollInfo.MouseWheelDown();
		}
		
		void IScrollInfo.MouseWheelLeft()
		{
			if (scrollInfo != null) scrollInfo.MouseWheelLeft();
		}
		
		void IScrollInfo.MouseWheelRight()
		{
			if (scrollInfo != null) scrollInfo.MouseWheelRight();
		}
		
		void IScrollInfo.SetHorizontalOffset(double offset)
		{
			if (scrollInfo != null) scrollInfo.SetHorizontalOffset(offset);
		}
		
		void IScrollInfo.SetVerticalOffset(double offset)
		{
			if (scrollInfo != null) scrollInfo.SetVerticalOffset(offset);
		}
		
		Rect IScrollInfo.MakeVisible(System.Windows.Media.Visual visual, Rect rectangle)
		{
			if (scrollInfo != null)
				return scrollInfo.MakeVisible(visual, rectangle);
			else
				return Rect.Empty;
		}
		#endregion
		
		#region Focus Handling (Show/Hide Caret)
		/// <inheritdoc/>
		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			base.OnMouseDown(e);
			Focus();
		}
		
		/// <inheritdoc/>
		protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnGotKeyboardFocus(e);
			caret.Show();
		}
		
		/// <inheritdoc/>
		protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnLostKeyboardFocus(e);
			caret.Hide();
		}
		#endregion
		
		#region OnTextInput / RemoveSelectedText / ReplaceSelectionWithText
		/// <summary>
		/// Occurs when the TextArea receives text input.
		/// This is like the <see cref="UIElement.TextInput"/> event,
		/// but occurs immediately before the TextArea handles the TextInput event.
		/// </summary>
		public event TextCompositionEventHandler TextEntering;
		
		/// <summary>
		/// Occurs when the TextArea receives text input.
		/// This is like the <see cref="UIElement.TextInput"/> event,
		/// but occurs immediately after the TextArea handles the TextInput event.
		/// </summary>
		public event TextCompositionEventHandler TextEntered;
		
		/// <summary>
		/// Raises the TextEntering event.
		/// </summary>
		protected virtual void OnTextEntering(TextCompositionEventArgs e)
		{
			if (TextEntering != null) {
				TextEntering(this, e);
			}
		}
		
		/// <summary>
		/// Raises the TextEntered event.
		/// </summary>
		protected virtual void OnTextEntered(TextCompositionEventArgs e)
		{
			if (TextEntered != null) {
				TextEntered(this, e);
			}
		}
		
		/// <inheritdoc/>
		protected override void OnTextInput(TextCompositionEventArgs e)
		{
			base.OnTextInput(e);
			if (!e.Handled && this.Document != null) {
				if (e.Text == "\x1b") {
					// ASCII 0x1b = ESC.
					// WPF produces a TextInput event with that old ASCII control char
					// when Escape is pressed. We'll just ignore it.
					return;
				}
				PerformTextInput(e);
				e.Handled = true;
			}
		}
		
		/// <summary>
		/// Performs text input.
		/// This raises the <see cref="TextEntering"/> event, replaces the selection with the text,
		/// and then raises the <see cref="TextEntered"/> event.
		/// </summary>
		public void PerformTextInput(string text)
		{
			TextComposition textComposition = new TextComposition(InputManager.Current, this, text);
			TextCompositionEventArgs e = new TextCompositionEventArgs(Keyboard.PrimaryDevice, textComposition);
			e.RoutedEvent = TextInputEvent;
			PerformTextInput(e);
		}
		
		/// <summary>
		/// Performs text input.
		/// This raises the <see cref="TextEntering"/> event, replaces the selection with the text,
		/// and then raises the <see cref="TextEntered"/> event.
		/// </summary>
		public void PerformTextInput(TextCompositionEventArgs e)
		{
			if (e == null)
				throw new ArgumentNullException("e");
			if (this.Document == null)
				throw ThrowUtil.NoDocumentAssigned();
			OnTextEntering(e);
			if (!e.Handled) {
				if (e.Text == "\n" || e.Text == "\r\n")
					ReplaceSelectionWithNewLine();
				else
					ReplaceSelectionWithText(e.Text);
				OnTextEntered(e);
				caret.BringCaretToView();
			}
		}
		
		void ReplaceSelectionWithNewLine()
		{
			string newLine = NewLineFinder.GetNewLineFromDocument(this.Document, this.Caret.Line);
			using (this.Document.RunUpdate()) {
				ReplaceSelectionWithText(newLine);
				if (this.IndentationStrategy != null) {
					DocumentLine line = this.Document.GetLineByNumber(this.Caret.Line);
					this.IndentationStrategy.IndentLine(this.Document, line);
				}
			}
		}
		
		internal void RemoveSelectedText()
		{
			if (this.Document == null)
				throw ThrowUtil.NoDocumentAssigned();
			selection.ReplaceSelectionWithText(this, string.Empty);
			#if DEBUG
			if (!selection.IsEmpty) {
				foreach (ISegment s in selection.Segments) {
					Debug.Assert(this.ReadOnlySectionProvider.GetDeletableSegments(s).Count() == 0);
				}
			}
			#endif
		}
		
		internal void ReplaceSelectionWithText(string newText)
		{
			if (newText == null)
				throw new ArgumentNullException("newText");
			if (this.Document == null)
				throw ThrowUtil.NoDocumentAssigned();
			selection.ReplaceSelectionWithText(this, newText);
		}
		
		internal ISegment[] GetDeletableSegments(ISegment segment)
		{
			var deletableSegments = this.ReadOnlySectionProvider.GetDeletableSegments(segment);
			if (deletableSegments == null)
				throw new InvalidOperationException("ReadOnlySectionProvider.GetDeletableSegments returned null");
			var array = deletableSegments.ToArray();
			int lastIndex = segment.Offset;
			for (int i = 0; i < array.Length; i++) {
				if (array[i].Offset < lastIndex)
					throw new InvalidOperationException("ReadOnlySectionProvider returned incorrect segments (outside of input segment / wrong order)");
				lastIndex = array[i].EndOffset;
			}
			if (lastIndex > segment.EndOffset)
				throw new InvalidOperationException("ReadOnlySectionProvider returned incorrect segments (outside of input segment / wrong order)");
			return array;
		}
		#endregion
		
		#region IndentationStrategy property
		/// <summary>
		/// IndentationStrategy property.
		/// </summary>
		public static readonly DependencyProperty IndentationStrategyProperty =
			DependencyProperty.Register("IndentationStrategy", typeof(IIndentationStrategy), typeof(TextArea),
			                            new FrameworkPropertyMetadata(new DefaultIndentationStrategy()));
		
		/// <summary>
		/// Gets/Sets the indentation strategy used when inserting new lines.
		/// </summary>
		public IIndentationStrategy IndentationStrategy {
			get { return (IIndentationStrategy)GetValue(IndentationStrategyProperty); }
			set { SetValue(IndentationStrategyProperty, value); }
		}
		#endregion
		
		#region OnKeyDown/OnKeyUp
		// Make life easier for text editor extensions that use a different cursor based on the pressed modifier keys.
		/// <inheritdoc/>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			TextView.InvalidateCursor();
		}
		
		/// <inheritdoc/>
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			TextView.InvalidateCursor();
		}
		#endregion
		
		/// <inheritdoc/>
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == SelectionBrushProperty
			    || e.Property == SelectionBorderProperty
			    || e.Property == SelectionForegroundProperty
			    || e.Property == SelectionCornerRadiusProperty)
			{
				textView.Redraw();
			}
		}
		
		/// <summary>
		/// Gets the requested service.
		/// </summary>
		/// <returns>Returns the requested service instance, or null if the service cannot be found.</returns>
		public virtual object GetService(Type serviceType)
		{
			return textView.Services.GetService(serviceType);
		}
		
		/// <summary>
		/// Occurs when text inside the TextArea was copied.
		/// </summary>
		public event EventHandler<TextEventArgs> TextCopied;
		
		internal void OnTextCopied(TextEventArgs e)
		{
			if (TextCopied != null)
				TextCopied(this, e);
		}
	}
	
	/// <summary>
	/// EventArgs with text.
	/// </summary>
	[Serializable]
	public class TextEventArgs : EventArgs
	{
		string text;
		
		/// <summary>
		/// Gets the text.
		/// </summary>
		public string Text {
			get {
				return text;
			}
		}
		
		/// <summary>
		/// Creates a new TextEventArgs instance.
		/// </summary>
		public TextEventArgs(string text)
		{
			if (text == null)
				throw new ArgumentNullException("text");
			this.text = text;
		}
	}
}
