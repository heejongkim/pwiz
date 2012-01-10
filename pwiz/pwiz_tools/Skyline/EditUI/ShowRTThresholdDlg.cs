﻿/*
 * Original author: Brendan MacLean <brendanx .at. u.washington.edu>,
 *                  MacCoss Lab, Department of Genome Sciences, UW
 *
 * Copyright 2009 University of Washington - Seattle, WA
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
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using pwiz.Skyline.Controls;

namespace pwiz.Skyline.EditUI
{
    public partial class ShowRTThresholdDlg : Form
    {
        private double _threshold;

        public ShowRTThresholdDlg()
        {
            InitializeComponent();
        }

        public double Threshold
        {
            get { return _threshold; }
            set
            {
                _threshold = value;
                textThreshold.Text = _threshold.ToString(CultureInfo.CurrentCulture);
            }
        }

        public void OkDialog()
        {
            var e = new CancelEventArgs();
            var helper = new MessageBoxHelper(this);
            if (!helper.ValidateDecimalTextBox(e, textThreshold, 0, double.MaxValue, out _threshold))
                return;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            OkDialog();
        }
    }
}
