﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Trifolia.Web.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>Users can requests permissions to implementation guides they don't have access to.</string>
  <string>Newly granted permissions have the option to send out notification emails directly from Trifolia.</string>
  <string>MS Word Export settings allow you to define individual value set member maximums for each value set.</string>
  <string>Implementation guide editors have the option to define default MS Word export settings.</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection WhatsNewItems {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["WhatsNewItems"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3." +
            "org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <s" +
            "tring>Having trouble? Look for the ? bubble or try hovering your mouse over the " +
            "link or header for a more detailed description.</string>\r\n  <string>In most grid" +
            "s, the \"Add\" (or \"New\") button is in the header or footer of the grid.</string>\r" +
            "\n  <string>Changes are not persisted until the \"Save\", \"Update\" or \"OK\" button i" +
            "s pressed. For example, when editing a template any updated constraint is not sa" +
            "ved permanently until you press the \"Save\" button at the bottom-left of the temp" +
            "late editor.</string>\r\n  <string>The template editor has three different constra" +
            "int editor views (selected in the bottom-right of the editor): Analyst, Editor a" +
            "nd Engineer.</string>\r\n  <string>Template authors with the edit-access to an imp" +
            "lementation guide can select \"default settings\" for an MS Word Export.</string>\r" +
            "\n  <string>Once an implementation guide has been published, value sets used by t" +
            "he implementation guide become locked and can only be modified by terminology ad" +
            "mins.</string>\r\n  <string>If you don\'t have access to an implementation guide, y" +
            "ou may be able to \"Request Access\" to the implementation guide view the \"Browse " +
            "Implementation Guides\" screen. Requesting access sends a notification email to t" +
            "he access manager of the implementation guide.</string>\r\n  <string>OIDs are now " +
            "considered \"identifiers\" in Trifolia. This change now treats identifiers more ge" +
            "nerically then OIDs. For templates, identifiers can be a single OID (ex: urn:oid" +
            ":XXX), a versioned OID (ex: urn:hl7ii:OID:DATE) or an HTTP/S address. For value " +
            "sets and code systems, identifiers can be a single OID or an HTTP identifier (to" +
            " support FHIR).</string>\r\n  <string>Trifolia\'s template editor is based on XML S" +
            "chemas. Trifolia is not restricted to editing templates for just CDA, it can def" +
            "ine templates for E-Measures and also define profiles for FHIR. Because Trifolia" +
            " uses XML Schema, in theory, any schema can be loaded into Trifolia and used wit" +
            "h the template editor; after the schema has been properly tested within Trifolia" +
            ".</string>\r\n</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection DidYouKnowItems {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["DidYouKnowItems"]));
            }
        }
    }
}
