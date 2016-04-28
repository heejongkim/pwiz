﻿/*
 * Original author: Nicholas Shulman <nicksh .at. u.washington.edu>,
 *                  MacCoss Lab, Department of Genome Sciences, UW
 *
 * Copyright 2016 University of Washington - Seattle, WA
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
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pwiz.Common.SystemUtil;
using pwiz.Skyline.Alerts;
using pwiz.SkylineTestUtil;

namespace pwiz.SkylineTestA
{
    [TestClass]
    public class FormUtilTest : AbstractUnitTest
    {
        /// <summary>
        /// Creates and destroys forms on multiple threads, and ensures that <see cref="FormUtil.OpenForms"/>
        /// performs correctly.
        /// </summary>
        [TestMethod]
        public void TestFormUtilOpenForms()
        {
            const int numberOfFormsToCreate = 25;
            int numberOfFormsCreated = 0;
            var threads = new List<Thread>();
            while (threads.Count < 3)
            {
                var thread = new Thread(() =>
                {
                    while (true)
                    {
                        int formNumber = Interlocked.Increment(ref numberOfFormsCreated);
                        if (formNumber > numberOfFormsToCreate)
                        {
                            return;
                        }
                        var alertDlg = new AlertDlg("Form number " + formNumber);
                        alertDlg.Shown += (sender, args) => alertDlg.BeginInvoke(new Action(() => alertDlg.Close()));
                        alertDlg.ShowDialog();
                    }
                });
                threads.Add(thread);
            }
            foreach (var thread in threads)
            {
                thread.Start();
            }
            while (true)
            {
                var openForms = FormUtil.OpenForms;
                CollectionAssert.DoesNotContain(openForms, null);
                if (numberOfFormsCreated >= numberOfFormsToCreate)
                {
                    break;
                }
            }
        }
    }
}