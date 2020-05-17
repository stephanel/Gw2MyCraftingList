using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace GW2ExplorerCraftTool
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private MainWindow _win;

        public MainWindowViewModel(MainWindow win)
        {
            this._win = win;

            Languages = new ObservableCollection<Data.Language>();
            Languages.Add(new Data.Language(0, "EN", "en"));
            Languages.Add(new Data.Language(1, "FR", "fr"));
        }

        private IList<Data.Language> _Languages;
        public IList<Data.Language> Languages
        {
            get { return _Languages; }
            set
            {
                if (value == _Languages)
                    return;

                _Languages = value;
                OnPropertyChanged("Languages");
            }
        }

        private CollectionViewSource _cvs;
        private ICollectionView LanguagesCVS
        {
            get
            {
                if (null == _cvs)
                {
                    _cvs = new CollectionViewSource();
                    _cvs.Source = Languages;
                    _cvs.View.Refresh();
                    _cvs.View.CurrentChanging += new CurrentChangingEventHandler(View_CurrentChanging);
                    //_cvs.View.MoveCurrentTo(Languages[Config.Language.Id]);
                }

                return _cvs.View;
            }
        }

        private Data.Language _CurrentLanguage;
        public Data.Language CurrentLanguage
        {
            get
            {
                return _CurrentLanguage;
            }
            set
            {
                _CurrentLanguage = value;
                OnPropertyChanged("CurrentLanguage");
            }
        }

        void View_CurrentChanging(object sender, CurrentChangingEventArgs e)
        {
            if (CurrentLanguage == null)
            {
                CurrentLanguage = Languages[Config.Language.Id]; //LanguagesCVS.CurrentItem as Data.Language;
                return;
            }
            
            ChangeLanguageMessage win = new ChangeLanguageMessage(this._win);
            win.ShowDialog();
            if (win.DialogResult.Equals(true))
            {
                Config.Lang = CurrentLanguage.GlobalizationCode.ToLower();
                Config.Save();
                App.SetCulture();
                App.Restart();
                return;
            }
            e.Cancel = true;
            CurrentLanguage = null;
            CurrentLanguage = Languages[Config.Language.Id]; // LanguagesCVS.CurrentItem as Data.Language;
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
