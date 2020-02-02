/*
 * Text Cleaner - A utility to cleanup text
 * Copyright (C) 2020 Peter Stuifzand <peter@p83.nl>
 *
 * This file is part of Text Cleaner.
 *
 * Text Cleaner is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Text Cleaner is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Text Cleaner.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;
using TextCleaner;
using Application = Gtk.Application;
using UI = Gtk.Builder.ObjectAttribute;

namespace Gui
{
    class TextCleaner : Window
    {
        [UI] private TextView textMain = null;

        [UI] private TextView textResult = null;

        [UI] private Entry textArgument1 = null;
        [UI] private Entry textArgument2 = null;

        [UI] private Label labelArgument1 = null;
        [UI] private Label labelArgument2 = null;

        [UI] private RadioButton radioFullText = null;
        [UI] private RadioButton radioLines = null;
        [UI] private RadioButton radioTesting = null;

        [UI] private ComboBoxText operation = null;

        [UI] private ListStore listOperations = null;
        private ListStore listWorkspace;

        [UI] private TreeView treeOperations = null;

        [UI] private TreeViewColumn columnName = null;
        [UI] private CellRendererText cellRendererName = null;

        [UI] private Button btnCopy = null;

        private Processor processor;
        private Operation currentOp;

        private OperationFactory operationFactory;

        public static Operation[] Operations = new Operation[] {
            new Nop(),
            new Uppercase(),
            new Lowercase(),
            new Titlecase(),
            new MatchText(),
            new ReplaceText(),
            new ReplaceFull(),
            new Trim(),
            new AddPrefix(),
            new AddSuffix(),
            new SurroundText(),
            new KeepMatchLines(),
            new RemoveMatchLines(),
            new HtmlDecode(),
            new HtmlEncode(),
            new StripTags(),
            new SelectHtml(),
            new FindHtmlLinks(),
            new LeftCharacters(),
            new MidCharacters(),
            new RightCharacters(),
            new SplitFormat(),
            new SelectJson(),
            new Calculate(),
        };


        public TextCleaner() : this(new Builder("TextCleaner.glade"))
        {
        }

        private TextCleaner(Builder builder) : base(builder.GetObject("TextCleaner").Handle)
        {
            builder.Autoconnect(this);

            operationFactory = new OperationFactory(Operations);

            processor = new FullTextProcessor();
            currentOp = new Nop();

            DeleteEvent += Window_DeleteEvent;

            var fira = Pango.FontDescription.FromString("Fira Code Regular 11");

            textMain.ModifyFont(fira);
            textResult.ModifyFont(fira);

            textMain.Buffer.Changed += (object sender, EventArgs e) => ProcessText();

            operation.Changed += (object sender, EventArgs e) => {
                if (operation.Active < 0) {
                    return;
                }

                currentOp = Operations[operation.Active];
                string[] argNames = currentOp.ArgNames;

                if (argNames.Length >= 1) {
                    textArgument1.IsEditable = true;
                    labelArgument1.Text = argNames[0];
                    textArgument1.Show();
                    labelArgument1.Show();
                } else {
                    textArgument1.IsEditable = false;
                    textArgument1.Hide();
                    labelArgument1.Hide();
                }

                if (argNames.Length >= 2) {
                    textArgument2.IsEditable = true;
                    labelArgument2.Text = argNames[1];
                    textArgument2.Show();
                    labelArgument2.Show();
                } else {
                    textArgument2.IsEditable = false;
                    textArgument2.Hide();
                    labelArgument2.Hide();
                }

                ProcessText();
            };

            textArgument1.Changed += (object sender, EventArgs e) => ProcessText();
            textArgument2.Changed += (object sender, EventArgs e) => ProcessText();

            radioLines.Clicked += (object sender, EventArgs e) => {
                btnCopy.Sensitive = true;
                processor = new LineProcessor();
                ProcessText();
            };

            radioFullText.Clicked += (object sender, EventArgs e) => {
                btnCopy.Sensitive = true;
                processor = new FullTextProcessor();
                ProcessText();
            };

            radioTesting.Clicked += (object sender, EventArgs e) => {
                btnCopy.Sensitive = false;
                TreeIter iter;
                if (!listWorkspace.GetIterFirst(out iter)) {
                    return;
                }

                var list = new List<CommandData>();
                do {
                    if (treeOperations.Selection.IterIsSelected(iter)) {
                        CommandData data = (CommandData) listWorkspace.GetValue(iter, 0);
                        list.Add(data);
                    }
                } while (listWorkspace.IterNext(ref iter));

                processor = new ProcessorList(operationFactory, list.ToArray());

                ProcessText();
            };

            btnCopy.Clicked += (sender, args) => {
                string processorName = "";
                if (radioFullText.Active) {
                    processorName = "full";
                } else if (radioLines.Active) {
                    processorName = "line";
                }

                string opName = currentOp.Name;
                string arg1 = textArgument1.Text;
                string arg2 = textArgument2.Text;

                listWorkspace.AppendValues(
                    CommandData.CreateInstance(
                        processorName,
                        opName,
                        operation.Active,
                        arg1,
                        arg2
                    )
                );
            };

            treeOperations.Selection.Changed += (sender, args) => {
                if (radioTesting.Active) {
                    btnCopy.Sensitive = false;
                    TreeIter iter;
                    if (!listWorkspace.GetIterFirst(out iter)) {
                        return;
                    }

                    var list = new List<CommandData>();
                    do {
                        if (treeOperations.Selection.IterIsSelected(iter)) {
                            CommandData data = (CommandData) listWorkspace.GetValue(iter, 0);
                            list.Add(data);
                        }
                    } while (listWorkspace.IterNext(ref iter));

                    processor = new ProcessorList(operationFactory, list.ToArray());
                    ProcessText();
                }
            };

            for (int i = 0; i < Operations.Length; i++) {
                var op = Operations[i];
                listOperations.AppendValues(i, op.Name);
            }

            operation.IdColumn = 0;
            operation.EntryTextColumn = 1;
            operation.Model = listOperations;

            var em = new TextTag("highlight") {Background = "#ff0"};
            textMain.Buffer.TagTable.Add(em);

            cellRendererName.Editable = false;
            columnName.SetCellDataFunc(cellRendererName, RenderCommandName);

            listWorkspace = new ListStore(typeof(CommandData));
            treeOperations.Model = listWorkspace;
            treeOperations.Reorderable = true;

            treeOperations.Selection.Mode = SelectionMode.Multiple;

            cellRendererName.Sensitive = true;
            treeOperations.Sensitive = true;

            FillFromClipboard();
        }

        private void RenderCommandName(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.ITreeModel model,
            Gtk.TreeIter iter)
        {
            CommandData data = (CommandData) model.GetValue(iter, 0);
            var cellRendererText = (CellRendererText) cell;
            var text = data.OperationText();

            cellRendererText.Text = text;
        }

        protected void ProcessText()
        {
            string[] args = {textArgument1.Text, textArgument2.Text};

            string lastResult = textResult.Buffer.Text;

            try {
                var input = textMain.Buffer;
                textMain.Buffer.RemoveTag("highlight", input.StartIter, input.EndIter);

                var result = processor.Process(input.Text, currentOp, args);

                textResult.Buffer.Text = result.Text;

                foreach (var hl in result.InputHighlights) {
                    if (hl.Line >= 0) {
                        input.ApplyTag(
                            "highlight",
                            input.GetIterAtLineOffset(hl.Line, hl.Start),
                            input.GetIterAtLineOffset(hl.Line, hl.End)
                        );
                    } else {
                        input.ApplyTag(
                            "highlight",
                            input.GetIterAtOffset(hl.Start),
                            input.GetIterAtOffset(hl.End)
                        );
                    }
                }
            } catch (Exception e) {
                textResult.Buffer.Text = lastResult;
            }
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        private void Toolbar_FillFromClipboard(object sender, EventArgs a)
        {
            FillFromClipboard();
        }

        private void Toolbar_CopyToClipboard(object sender, EventArgs a)
        {
            CopyToClipboard();
        }

        private void Toolbar_CopyToLeft(object sender, EventArgs a)
        {
            CopyToLeft();
        }

        private void CopyToClipboard()
        {
            var clipboard = Clipboard.Get(Gdk.Atom.Intern("CLIPBOARD", false));
            clipboard.SetWithData(
                new[] {TargetEntry.New("UTF8_STRING", 0, 0)},
                (clipboard1, selectionData, info) => {
                    selectionData.Text = textResult.Buffer.Text;
                    clipboard1.Store();
                },
                clipboard1 => { }
            );
        }

        private void FillFromClipboard()
        {
            textMain.Buffer.Text = Clipboard.Get(Gdk.Atom.Intern("CLIPBOARD", false)).WaitForText();
        }

        private void CopyToLeft()
        {
            textMain.Buffer.Text = textResult.Buffer.Text;

            textArgument1.Text = "";
            textArgument2.Text = "";

            operation.GrabFocus();
        }

        private void workspace_row_activated(object o, RowActivatedArgs args)
        {
            TreeIter iter;
            if (treeOperations.Model.GetIter(out iter, args.Path)) {
                if (!treeOperations.Selection.IterIsSelected(iter)) {
                    return;
                }

                CommandData data = (CommandData) treeOperations.Model.GetValue(iter, 0);
                if (data.Processor == "full") {
                    radioFullText.Active = true;
                } else if (data.Processor == "line") {
                    radioLines.Active = true;
                }

                operation.Active = data.OperationId;

                textArgument1.Text = data.Arg1;
                textArgument2.Text = data.Arg2;
            }
        }

        private void WorkspaceDeleteRow(object o, KeyPressEventArgs args)
        {
            if (args.Event.Key != Gdk.Key.Delete) {
                return;
            }

            var rows = treeOperations.Selection.GetSelectedRows();
            foreach (var row in rows.Reverse()) {
                TreeIter iter;
                if (listWorkspace.GetIter(out iter, row)) {
                    listWorkspace.Remove(ref iter);
                }
            }
        }
    }
}
