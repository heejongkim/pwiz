/*
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pwiz.Skyline.Model;
using pwiz.Skyline.Model.DocSettings;
using pwiz.Skyline.Model.DocSettings.Extensions;

namespace pwiz.SkylineTest
{
    /// <summary>
    /// Summary description for ExplicitModTest
    /// </summary>
    [TestClass]
    public class ExplicitModTest
    {
        private static readonly List<StaticMod> ATOMIC_HEAVY_MODS = new List<StaticMod>
            {
                // The original document expressed Heavy K incorrectly
                new StaticMod("13C K", 'K', ModTerminus.C, "C'8 - C8", LabelAtoms.None, null, null),
                new StaticMod("13C R", 'R', ModTerminus.C, null, LabelAtoms.C13, null, null),
                new StaticMod("13C V", 'V', LabelAtoms.C13).ChangeExplicit(true),
                new StaticMod("13C L", 'L', LabelAtoms.C13).ChangeExplicit(true)
            };


        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion

        [TestMethod]
        public void Study7RoundtripTest()
        {
            SrmDocument docStudy7 = CreateStudy7Doc();
            var modifications = docStudy7.Settings.PeptideSettings.Modifications;
            Assert.AreEqual(0, modifications.StaticModifications.Count(mod => mod.IsExplicit));
            Assert.AreEqual(2, modifications.HeavyModifications.Count(mod => mod.IsExplicit));
            Assert.AreEqual(4, modifications.HeavyModifications.Count(mod => mod.Formula != null));
            Assert.AreEqual(3, docStudy7.Peptides.Count(peptide => peptide.HasExplicitMods));
            Assert.AreEqual(2, docStudy7.Peptides.Count(peptide => peptide.HasExplicitMods &&
                peptide.ExplicitMods.StaticModifications.Count > 0 &&
                peptide.ExplicitMods.StaticModifications[0].Modification.AA == 'C'));
            Assert.AreEqual(2, docStudy7.Peptides.Count(peptide => peptide.HasExplicitMods &&
                peptide.ExplicitMods.HeavyModifications[0].Modification.AA == 'V'));
            Assert.AreEqual(1, docStudy7.Peptides.Count(peptide => peptide.HasExplicitMods &&
                peptide.ExplicitMods.HeavyModifications[0].Modification.AA == 'L'));
            AssertEx.Serializable(docStudy7, 3, AssertEx.DocumentCloned);
        }

        [TestMethod]
        public void AddRemoveExplicitModTest()
        {
            SrmDocument docStudy7 = CreateStudy7Doc();
            string transitionList = ExportCsv(docStudy7);
            Assert.AreEqual(69, transitionList.Split('\n').Length);

            var modifications = docStudy7.Settings.PeptideSettings.Modifications;
            var listStaticMods = modifications.StaticModifications;
            var listHeavyMods = modifications.HeavyModifications;

            docStudy7 = docStudy7.ChangeSettings(docStudy7.Settings.ChangeTransitionFilter(filter => filter.ChangeAutoSelect(false)));

            // Remove all modifications
            int i = 0;
            // But save them for later
            var removedMods = new Dictionary<int, ExplicitMods>();
            foreach (var peptide in docStudy7.Peptides)
            {
                if (peptide.HasExplicitMods)
                {
                    removedMods.Add(i, peptide.ExplicitMods);

                    IdentityPath path = docStudy7.GetPathTo((int) SrmDocument.Level.Peptides, i);
                    docStudy7 = docStudy7.ChangePeptideMods(path, null, listStaticMods, listHeavyMods);
                }
                i++;
            }

            // Removes heavy from peptide with c-terminal P
            AssertEx.IsDocumentState(docStudy7, 6, 7, 11, 21, 63);
            modifications = docStudy7.Settings.PeptideSettings.Modifications;
            Assert.AreEqual(2, modifications.HeavyModifications.Count);
            Assert.AreEqual(0, modifications.HeavyModifications.Count(mod => mod.IsExplicit));
            Assert.AreEqual(0, docStudy7.Peptides.Count(peptide => peptide.HasExplicitMods));

            listHeavyMods = ATOMIC_HEAVY_MODS;

            foreach (var pair in removedMods)
            {
                IdentityPath path = docStudy7.GetPathTo((int)SrmDocument.Level.Peptides, pair.Key);
                docStudy7 = docStudy7.ChangePeptideMods(path, pair.Value, listStaticMods, listHeavyMods);
            }

            AssertEx.IsDocumentState(docStudy7, 11, 7, 11, 21, 63);

            // Replace the heavy precursor that was removed
            // TODO: Yuck.  Would be nice to have a way to do this without duplicating
            //       so much of the logic in PeptideDocNode and PeptideTreeNode
            var pepPath = docStudy7.GetPathTo((int) SrmDocument.Level.Peptides, 10);
            var nodePep = (PeptideDocNode) docStudy7.FindNode(pepPath);
            string seq = nodePep.Peptide.Sequence;
            var mods = nodePep.ExplicitMods;
            var nodeGroupLight = (TransitionGroupDocNode) nodePep.Children[0];
            var settings = docStudy7.Settings;
            foreach (var tranGroup in nodePep.Peptide.GetTransitionGroups(settings, mods, false))
            {
                if (tranGroup.PrecursorCharge == nodeGroupLight.TransitionGroup.PrecursorCharge &&
                        !tranGroup.LabelType.IsLight)
                {
                    double massH = settings.GetPrecursorMass(tranGroup.LabelType, seq, mods);
                    RelativeRT relativeRT = settings.GetRelativeRT(tranGroup.LabelType, seq, mods);
                    TransitionDocNode[] transitions = nodePep.GetMatchingTransitions(tranGroup, settings, mods);
                    var nodeGroup = new TransitionGroupDocNode(tranGroup, massH, relativeRT,
                        transitions, false);
                    nodeGroup = nodeGroup.ChangeSettings(settings, mods, SrmSettingsDiff.ALL);
                    docStudy7 = (SrmDocument) docStudy7.Add(pepPath, nodeGroup);
                    break;
                }
            }

            AssertEx.IsDocumentState(docStudy7, 12, 7, 11, 22, 66);

            modifications = docStudy7.Settings.PeptideSettings.Modifications;
            Assert.AreEqual(2, modifications.HeavyModifications.Count(mod => mod.IsExplicit && mod.Label13C));
            Assert.AreEqual(2, modifications.HeavyModifications.Count(mod => mod.Formula != null));
            Assert.AreEqual(3, docStudy7.Peptides.Count(peptide => peptide.HasExplicitMods));
            Assert.AreEqual(2, docStudy7.Peptides.Count(peptide => peptide.HasExplicitMods &&
                peptide.ExplicitMods.StaticModifications.Count > 0 &&
                peptide.ExplicitMods.StaticModifications[0].Modification.AA == 'C'));
            Assert.AreEqual(2, docStudy7.Peptides.Count(peptide => peptide.HasExplicitMods &&
                peptide.ExplicitMods.HeavyModifications[0].Modification.AA == 'V' &&
                peptide.ExplicitMods.HeavyModifications[0].Modification.Label13C));
            Assert.AreEqual(1, docStudy7.Peptides.Count(peptide => peptide.HasExplicitMods &&
                peptide.ExplicitMods.HeavyModifications[0].Modification.AA == 'L' &&
                peptide.ExplicitMods.HeavyModifications[0].Modification.Label13C));

            AssertEx.NoDiff(transitionList, ExportCsv(docStudy7));
        }

        [TestMethod]
        public void ChangeSettingsExplicitModTest()
        {
            SrmDocument docStudy7 = CreateStudy7Doc();
            string transitionList = ExportCsv(docStudy7);
            Assert.AreEqual(69, transitionList.Split('\n').Length);

            var settings = docStudy7.Settings.ChangeTransitionFilter(filter => filter.ChangeAutoSelect(false));
            settings = settings.ChangePeptideModifications(mods => mods.ChangeHeavyModifications(ATOMIC_HEAVY_MODS));
            docStudy7 = docStudy7.ChangeSettings(settings);
            AssertEx.IsDocumentState(docStudy7, 1, 7, 11, 22, 66);

            Assert.AreEqual(3, docStudy7.Peptides.Count(peptide => peptide.HasExplicitMods));
            Assert.AreEqual(2, docStudy7.Peptides.Count(peptide => peptide.HasExplicitMods &&
                peptide.ExplicitMods.StaticModifications.Count > 0 &&
                peptide.ExplicitMods.StaticModifications[0].Modification.AA == 'C'));
            Assert.AreEqual(2, docStudy7.Peptides.Count(peptide => peptide.HasExplicitMods &&
                peptide.ExplicitMods.HeavyModifications[0].Modification.AA == 'V' &&
                peptide.ExplicitMods.HeavyModifications[0].Modification.Label13C));
            Assert.AreEqual(1, docStudy7.Peptides.Count(peptide => peptide.HasExplicitMods &&
                peptide.ExplicitMods.HeavyModifications[0].Modification.AA == 'L' &&
                peptide.ExplicitMods.HeavyModifications[0].Modification.Label13C));

            AssertEx.NoDiff(transitionList, ExportCsv(docStudy7));

            // Correct Heavy K
            var listHeavyMods = new List<StaticMod>(ATOMIC_HEAVY_MODS);
            listHeavyMods[0] = new StaticMod("Heavy K", 'K', ModTerminus.C, null, LabelAtoms.C13|LabelAtoms.N15, null, null);
            settings = settings.ChangePeptideModifications(mods => mods.ChangeHeavyModifications(listHeavyMods));
            var docHeavyK = docStudy7.ChangeSettings(settings);
            var peptidesOld = docStudy7.Peptides.ToArray();
            var peptidesNew = docHeavyK.Peptides.ToArray();

            Assert.AreEqual(peptidesOld.Length, peptidesNew.Length);

            for (int i = 0; i < peptidesOld.Length; i++)
            {
                var peptideOld = peptidesOld[i];
                var peptideNew = peptidesNew[i];
                // Non-explicit cysteines should have changed, but no other peptides
                string seq = peptideOld.Peptide.Sequence;
                if (seq[seq.Length - 1] == 'K' && !peptideOld.HasExplicitMods)
                    Assert.AreNotSame(peptideOld, peptideNew);
                else
                    Assert.AreSame(peptideOld, peptideNew);
            }

            // Change valine explicit only modification
            listHeavyMods[2] = new StaticMod("13C V", 'V', null, null, LabelAtoms.C13 | LabelAtoms.N15, null, null);
            listHeavyMods[2] = listHeavyMods[2].ChangeExplicit(true);
            settings = settings.ChangePeptideModifications(mods => mods.ChangeHeavyModifications(listHeavyMods));
            var docHeavyV = docHeavyK.ChangeSettings(settings);
            peptidesOld = docHeavyK.Peptides.ToArray();
            peptidesNew = docHeavyV.Peptides.ToArray();

            Assert.AreEqual(peptidesOld.Length, peptidesNew.Length);

            for (int i = 0; i < peptidesOld.Length; i++)
            {
                var peptideOld = peptidesOld[i];
                var peptideNew = peptidesNew[i];
                // Explicit valines should have changed, but no other peptides
                if (peptideOld.HasExplicitMods && peptideOld.ExplicitMods.HeavyModifications[0].Modification.AA == 'V')
                    Assert.AreNotSame(peptideOld, peptideNew);
                else
                    Assert.AreSame(peptideOld, peptideNew);
            }

            // Remove implicit cysteine modification
            var staticMods = new[] { settings.PeptideSettings.Modifications.StaticModifications[0].ChangeExplicit(true) };
            settings = settings.ChangePeptideModifications(mods => mods.ChangeStaticModifications(staticMods));
            var docNoStat = docHeavyV.ChangeSettings(settings);

            peptidesOld = docHeavyV.Peptides.ToArray();
            peptidesNew = docNoStat.Peptides.ToArray();

            Assert.AreEqual(peptidesOld.Length, peptidesNew.Length);

            for (int i = 0; i < peptidesOld.Length; i++)
            {
                var peptideOld = peptidesOld[i];
                var peptideNew = peptidesNew[i];
                // Non-explicit cysteines should have changed, but no other peptides
                if (peptideOld.Peptide.Sequence.IndexOf('C') != -1 && !peptideOld.HasExplicitMods)
                    Assert.AreNotSame(peptideOld, peptideNew);
                else
                    Assert.AreSame(peptideOld, peptideNew);
            }
        }

        [TestMethod]
        public void ModifyExplicitModTest()
        {
            SrmDocument docStudy7 = CreateStudy7Doc();
            var settings = docStudy7.Settings.ChangeTransitionFilter(filter => filter.ChangeAutoSelect(false));
            var listStaticMods = settings.PeptideSettings.Modifications.StaticModifications;
            var listHeavyMods = new List<StaticMod>(settings.PeptideSettings.Modifications.HeavyModifications);

            // Change an explicit heavy modification to something new
            var modV = new StaticMod("Heavy V", 'V', LabelAtoms.C13|LabelAtoms.N15);
            listHeavyMods.Add(modV);

            IdentityPath path = docStudy7.GetPathTo((int)SrmDocument.Level.Peptides, 0);
            var peptideMod = (PeptideDocNode) docStudy7.FindNode(path);
            var explicitMods = peptideMod.ExplicitMods;
            explicitMods = explicitMods.ChangeHeavyModifications(new[] {explicitMods.HeavyModifications[0].ChangeModification(modV)});
            var docHeavyV = docStudy7.ChangePeptideMods(path, explicitMods, listStaticMods, listHeavyMods);

            var modSettings = docHeavyV.Settings.PeptideSettings.Modifications;
            Assert.AreEqual(5, modSettings.HeavyModifications.Count);
            Assert.AreEqual(4, modSettings.HeavyModifications.Count(mod => mod.Formula != null));
            Assert.AreEqual(1, modSettings.HeavyModifications.Count(mod => mod.Label13C && mod.Label15N));
            Assert.AreEqual(3, docHeavyV.Peptides.Count(peptide => peptide.HasExplicitMods));
            Assert.AreEqual(1, docHeavyV.Peptides.Count(peptide => peptide.HasExplicitMods &&
                peptide.ExplicitMods.HeavyModifications[0].Modification.AA == 'V' &&
                peptide.ExplicitMods.HeavyModifications[0].Modification.Label13C));
            Assert.AreEqual(1, docHeavyV.Peptides.Count(peptide => peptide.HasExplicitMods &&
                peptide.ExplicitMods.HeavyModifications[0].Modification.AA == 'V' &&
                peptide.ExplicitMods.HeavyModifications[0].Modification.Label13C &&
                peptide.ExplicitMods.HeavyModifications[0].Modification.Label15N));

            // Change an explicit heavy modification to something new
            listHeavyMods = new List<StaticMod>(settings.PeptideSettings.Modifications.HeavyModifications);
            modV = listHeavyMods[2] = ATOMIC_HEAVY_MODS[2];

            explicitMods = peptideMod.ExplicitMods;
            explicitMods = explicitMods.ChangeHeavyModifications(new[] { explicitMods.HeavyModifications[0].ChangeModification(modV) });
            var doc13V = docStudy7.ChangePeptideMods(path, explicitMods, listStaticMods, listHeavyMods);

            modSettings = doc13V.Settings.PeptideSettings.Modifications;
            Assert.AreEqual(4, modSettings.HeavyModifications.Count);
            Assert.AreEqual(3, modSettings.HeavyModifications.Count(mod => mod.Formula != null));
            Assert.AreEqual(1, modSettings.HeavyModifications.Count(mod => mod.Label13C));
            Assert.AreEqual(3, doc13V.Peptides.Count(peptide => peptide.HasExplicitMods));
            Assert.AreEqual(2, doc13V.Peptides.Count(peptide => peptide.HasExplicitMods &&
                peptide.ExplicitMods.HeavyModifications[0].Modification.AA == 'V' &&
                peptide.ExplicitMods.HeavyModifications[0].Modification.Label13C));

            // No change to the peptide, but change an orthoganal modification
            path = docStudy7.GetPathTo((int)SrmDocument.Level.Peptides, docStudy7.Peptides.Count() - 1);
            peptideMod = (PeptideDocNode)docStudy7.FindNode(path);
            doc13V = docStudy7.ChangePeptideMods(path, peptideMod.ExplicitMods, listStaticMods, listHeavyMods);

            modSettings = doc13V.Settings.PeptideSettings.Modifications;
            Assert.AreEqual(4, modSettings.HeavyModifications.Count);
            Assert.AreEqual(3, modSettings.HeavyModifications.Count(mod => mod.Formula != null));
            Assert.AreEqual(1, modSettings.HeavyModifications.Count(mod => mod.Label13C));
            Assert.AreEqual(3, doc13V.Peptides.Count(peptide => peptide.HasExplicitMods));
            Assert.AreEqual(2, doc13V.Peptides.Count(peptide => peptide.HasExplicitMods &&
                peptide.ExplicitMods.HeavyModifications[0].Modification.AA == 'V' &&
                peptide.ExplicitMods.HeavyModifications[0].Modification.Label13C));

            // No change to the peptide, but remove all other modifications from global lists
            var docClear = docStudy7.ChangePeptideMods(path, peptideMod.ExplicitMods,
                new StaticMod[0], new[] {listHeavyMods[3]});
            Assert.AreSame(docClear, docStudy7);
        }

        private static SrmDocument CreateStudy7Doc()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SrmDocument));
            var stream = typeof(ExplicitModTest).Assembly.GetManifestResourceStream("pwiz.SkylineTest.Study7.sky");
            Assert.IsNotNull(stream);
            Debug.Assert(stream != null);   // Keep ReSharper from warning
            SrmDocument docStudy7 = (SrmDocument)xmlSerializer.Deserialize(stream);
            AssertEx.IsDocumentState(docStudy7, 0, 7, 11, 22, 66);
            return docStudy7;
        }

        private static string ExportCsv(SrmDocument document)
        {
            MassListExporter exporter = new ThermoMassListExporter(document);
            exporter.Export(null);
            StringBuilder sb = new StringBuilder();
            foreach (var pair in exporter.MemoryOutput)
                sb.AppendLine(pair.Key).AppendLine(pair.Value.ToString());
            return sb.ToString();
        }
    }
}