﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4961
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IDPicker.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0")]
    public sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int MinDistinctPeptides {
            get {
                return ((int)(this["MinDistinctPeptides"]));
            }
            set {
                this["MinDistinctPeptides"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int MaxAmbiguousIds {
            get {
                return ((int)(this["MaxAmbiguousIds"]));
            }
            set {
                this["MaxAmbiguousIds"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int MinPeptideLength {
            get {
                return ((int)(this["MinPeptideLength"]));
            }
            set {
                this["MinPeptideLength"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("r-")]
        public string DecoyPrefix {
            get {
                return ((string)(this["DecoyPrefix"]));
            }
            set {
                this["DecoyPrefix"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("IDPicker.log")]
        public string LogFileName {
            get {
                return ((string)(this["LogFileName"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int DebugLevel {
            get {
                return ((int)(this["DebugLevel"]));
            }
            set {
                this["DebugLevel"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int MinAdditionalPeptides {
            get {
                return ((int)(this["MinAdditionalPeptides"]));
            }
            set {
                this["MinAdditionalPeptides"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("mzML;RAW;mzXML;MGF")]
        public string SourceExtensions {
            get {
                return ((string)(this["SourceExtensions"]));
            }
            set {
                this["SourceExtensions"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3." +
            "org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <s" +
            "tring>&lt;RootInputDirectory&gt;</string>\r\n</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection FastaPaths {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["FastaPaths"]));
            }
            set {
                this["FastaPaths"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3." +
            "org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <s" +
            "tring>&lt;RootInputDirectory&gt;</string>\r\n</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection SourcePaths {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["SourcePaths"]));
            }
            set {
                this["SourcePaths"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3." +
            "org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <s" +
            "tring>&lt;RootInputDirectory&gt;</string>\r\n</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection SearchPaths {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["SearchPaths"]));
            }
            set {
                this["SearchPaths"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int MinSpectraPerProtein {
            get {
                return ((int)(this["MinSpectraPerProtein"]));
            }
            set {
                this["MinSpectraPerProtein"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool TopRankOnly {
            get {
                return ((bool)(this["TopRankOnly"]));
            }
            set {
                this["TopRankOnly"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringCollection QonverterSettings {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["QonverterSettings"]));
            }
            set {
                this["QonverterSettings"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3." +
            "org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" />")]
        public global::System.Collections.Specialized.StringCollection UserLayouts {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["UserLayouts"]));
            }
            set {
                this["UserLayouts"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3." +
            "org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <s" +
            "tring>MyriMatch optimized;SVM;False;Linear;Ignore;Ignore;Partition;Partition;1 A" +
            "scending Off myrimatch:mvh;1 Ascending Off xcorr</string>\r\n  <string>MyriMatch M" +
            "VH;StaticWeighted;False;Linear;Ignore;Ignore;Partition;Partition;1 Ascending Off" +
            " myrimatch:mvh</string>\r\n  <string>MyriMatch XCorr;StaticWeighted;False;Linear;I" +
            "gnore;Ignore;Partition;Partition;1 Ascending Off xcorr</string>\r\n  <string>Seque" +
            "st optimized;SVM;False;Linear;Ignore;Ignore;Partition;Partition;1 Ascending Off " +
            "sequest:xcorr;1 Ascending Off sequest:deltacn</string>\r\n  <string>Sequest XCorr;" +
            "StaticWeighted;False;Linear;Ignore;Ignore;Partition;Partition;1 Ascending Off se" +
            "quest:xcorr</string>\r\n  <string>Mascot ionscore;StaticWeighted;False;Linear;Igno" +
            "re;Ignore;Partition;Partition;1 Ascending Off mascot:score</string>\r\n  <string>M" +
            "ascot ionscore-identityscore;StaticWeighted;False;Linear;Ignore;Ignore;Partition" +
            ";Partition;1 Ascending Off mascot:score;1 Descending Off mascot:identity thresho" +
            "ld</string>\r\n  <string>X!Tandem optimized;SVM;False;Linear;Ignore;Ignore;Partiti" +
            "on;Partition;1 Descending Off x!tandem:expect;1 Ascending Off x!tandem:hyperscor" +
            "e</string>\r\n  <string>X!Tandem expect;StaticWeighted;False;Linear;Ignore;Ignore;" +
            "Partition;Partition;1 Descending Off x!tandem:expect</string>\r\n  <string>X!Tande" +
            "m hyperscore;StaticWeighted;False;Linear;Ignore;Ignore;Partition;Partition;1 Asc" +
            "ending Off x!tandem:hyperscore</string>\r\n  <string>OMSSA expect;StaticWeighted;F" +
            "alse;Linear;Ignore;Ignore;Partition;Partition;1 Descending Off expect</string>\r\n" +
            "  <string>Phenyx zscore;StaticWeighted;False;Linear;Ignore;Ignore;Partition;Part" +
            "ition;1 Ascending Off zscore</string>\r\n</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection DefaultQonverterSettings {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["DefaultQonverterSettings"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int DefaultMaxRank {
            get {
                return ((int)(this["DefaultMaxRank"]));
            }
            set {
                this["DefaultMaxRank"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.25")]
        public double DefaultMaxFDR {
            get {
                return ((double)(this["DefaultMaxFDR"]));
            }
            set {
                this["DefaultMaxFDR"] = value;
            }
        }
    }
}
