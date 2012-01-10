/*
 * Original author: Brendan MacLean <brendanx .at. uw.edu>,
 *                  MacCoss Lab, Department of Genome Sciences, UW
 *
 * Copyright 2011 University of Washington - Seattle, WA
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using pwiz.Skyline.Alerts;
using pwiz.Skyline.Model.Irt;
using pwiz.Skyline.Model.Lib;
using pwiz.Skyline.Util;

namespace pwiz.Skyline.SettingsUI.Irt
{
    public enum IrtCalculatorSource { settings, file }

    public partial class AddIrtCalculatorDlg : Form
    {
        public AddIrtCalculatorDlg(IEnumerable<RCalcIrt> calculators)
        {
            InitializeComponent();

            comboLibrary.Items.AddRange(calculators.Cast<object>().ToArray());
            ComboHelper.AutoSizeDropDown(comboLibrary);
        }

        public IrtCalculatorSource Source
        {
            get { return radioSettings.Checked ? IrtCalculatorSource.settings : IrtCalculatorSource.file; }

            set
            {
                if (value == IrtCalculatorSource.settings)
                    radioSettings.Checked = true;
                else
                    radioFile.Checked = true;
            }
        }

        public RCalcIrt Calculator
        {
            get
            {
                if (Source == IrtCalculatorSource.settings)
                    return (RCalcIrt)comboLibrary.SelectedItem;
                return new RCalcIrt("", textFilePath.Text);
            }
        }

        private void OkDialog()
        {
            if (Source == IrtCalculatorSource.file)
            {
                string path = textFilePath.Text;
                string message = null;
                if (string.IsNullOrEmpty(path))
                    message = "Please specify a path to an existing iRT database.";
                else if (!path.EndsWith(IrtDb.EXT))
                    message = string.Format("The file {0} is not an iRT database.", path);
                else if (!File.Exists(path))
                    message = string.Format("The file {0} does not exist.\nPlease specify a path to an existing iRT database.", path);
                if (message != null)
                {
                    MessageDlg.Show(this, message);
                    textFilePath.Focus();
                    return;                    
                }
            }
            var calculator = Calculator;
            if (calculator == null)
            {
                MessageDlg.Show(this, "Please choose the iRT calculator you would like to add.");
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            OkDialog();
        }

        private void radioSettings_CheckedChanged(object sender, EventArgs e)
        {
            SourceChanged();
        }

        private void radioFile_CheckedChanged(object sender, EventArgs e)
        {
            SourceChanged();
        }

        private void SourceChanged()
        {
            if (Source == IrtCalculatorSource.settings)
            {
                comboLibrary.Enabled = true;
                textFilePath.Enabled = false;
                textFilePath.Text = "";
                btnBrowseFile.Enabled = false;
            }
            else
            {
                comboLibrary.SelectedIndex = -1;
                comboLibrary.Enabled = false;
                textFilePath.Enabled = true;
                btnBrowseFile.Enabled = true;
            }
        }

        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog
                                 {
                                     CheckPathExists = true,
                                     DefaultExt = BiblioSpecLibSpec.EXT,
                                     Filter = string.Join("|", new[]
                                                                   {
                                                                       "iRT Database Files (*" + IrtDb.EXT + ")|*" + IrtDb.EXT,
                                                                       "All Files (*.*)|*.*"
                                                                   })
                                 })
            {
                if (dlg.ShowDialog(this) != DialogResult.OK)
                    return;

                textFilePath.Text = dlg.FileName;
            }
        }
    }
}