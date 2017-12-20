using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Threading;

namespace Aii.Assurance.BindingBaseViewModel
{


    public class BindableBase : INotifyPropertyChanged
    {
        ///
        /// Multicast event for property change notifications.
        ///
		public event PropertyChangedEventHandler PropertyChanged;



		public BindableBase()
		{


		}

        ///
        /// Checks if a property already matches a desired value.  Sets the property and
        /// notifies listeners only when necessary.
        ///
        ///Type of the property.
        ///Reference to a property with both getter and setter.
        ///Desired value for the property.
        ///Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.
        ///True if the value was changed, false if the existing value matched the
        /// desired value.
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (storage != null)
            {
                //if (object.Equals(storage, value)) return false;
				if (storage.Equals(value)) return false;
            }

            storage = value;
            RaisePropertyChange(propertyName);
            return true;
        }

        protected virtual void RaisePropertyChange(string propertyName)
        {            
			
			// we can't use Send here since for example, the splash screen is up during the build 
			// of the main form and the processing is not guarranteed to be running yet.
			// if we send before it is - we hang here for ever....
			//UIctx.Post (_ => OnPropertyChanged(propertyName), null);	

			// the updates MUST come in the UI thread or we get memory leaks with the above..
			OnPropertyChanged(propertyName);
        }

        ///
        /// Notifies listeners that a property value has changed.
        ///
        ///Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support .
        ///         
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            try
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (TargetInvocationException e)
            {
                Console.WriteLine("OnPropertyChanged Error {0}", propertyName);
                Console.WriteLine(e.Message);
            }
        }
    }
}