using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace GW2ExplorerCraftTool.Data
{
    public class Language : INotifyPropertyChanged
    {
        public const string DE = "de";
        public const string EN = "en";
        public const string ES = "es";
        public const string FR = "fr";

        private int _id;

        public int Id
        {
          get { return _id; }
          set {
              _id = value;
              OnPropertyChanged("Id");
          }
        }

        private string _label;

        public string Label
        {
          get { return _label; }
          set {
              _label = value;
              OnPropertyChanged("Label");
          }
        }

        private string _globalizationCode;

        public string GlobalizationCode
        {
          get { return _globalizationCode; }
          set { 
              _globalizationCode = value;
              OnPropertyChanged("GlobalizationCode");
          }
        }

        public Language(int id, string label, string globalizationCode)
        {
            this._id = id;
            this._label = label;
            this._globalizationCode = globalizationCode;
        }

        private static List<Language> _languages;

        public static List<Language> Languages
        {
            get {
                return _languages;
            }
        }
        static Language()
        {
            _languages = new List<Language>();
            _languages.Add(new Language(0, DE.ToUpper(), DE));
            _languages.Add(new Language(1, EN.ToUpper(), EN));
            _languages.Add(new Language(2, ES.ToUpper(), ES));
            _languages.Add(new Language(3, FR.ToUpper(), FR));
        }

        public static Data.Language GetLanguage(string key)
        {
            return Data.Language.Languages
                    .Where((p) => { return p.Label.ToLower() == key.ToLower(); }).Single();
        }
        
        public override string ToString()
        {
            return this._globalizationCode;
        }

        #region INotifyPropertyChanged

        /// <summary>
        /// The PropertyChanged event is used by consuming code
        /// (like WPF's binding infrastructure) to detect when
        /// a value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise the PropertyChanged event for the 
        /// specified property.
        /// </summary>
        /// <param name="propertyName">
        /// A string representing the name of 
        /// the property that changed.</param>
        /// <remarks>
        /// Only raise the event if the value of the property 
        /// has changed from its previous value</remarks>
        protected void OnPropertyChanged(string propertyName)
        {
            // Validate the property name in debug builds
            VerifyProperty(propertyName);

            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Verifies whether the current class provides a property with a given
        /// name. This method is only invoked in debug builds, and results in
        /// a runtime exception if the <see cref="OnPropertyChanged"/> method
        /// is being invoked with an invalid property name. This may happen if
        /// a property's name was changed but not the parameter of the property's
        /// invocation of <see cref="OnPropertyChanged"/>.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        [System.Diagnostics.Conditional("DEBUG")]
        private void VerifyProperty(string propertyName)
        {
            Type type = this.GetType();

            // Look for a *public* property with the specified name
            System.Reflection.PropertyInfo pi = type.GetProperty(propertyName);
            if (pi == null)
            {
                // There is no matching property - notify the developer
                string msg = "OnPropertyChanged was invoked with invalid " +
                                "property name {0}. {0} is not a public " +
                                "property of {1}.";
                msg = String.Format(msg, propertyName, type.FullName);
                System.Diagnostics.Debug.Fail(msg);
            }
        }

        #endregion

    }
}
