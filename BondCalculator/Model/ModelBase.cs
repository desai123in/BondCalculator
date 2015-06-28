using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BondCalculator.Model
{
    //base class for models that needs to bind to UI
    public class ModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
{
    #region Property changed
    public event PropertyChangedEventHandler PropertyChanged;
    public Action NotifyVM; // to notify any consumer who wants to be notified when Error status changes.
 
    protected void NotifyPropertyChanged(string propertyName)
    {
        if (this.PropertyChanged != null)
        { 
            // property changed
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));           
        }
    }
    #endregion
 
    #region Notify data error
    private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
 
    // get errors by property
    public IEnumerable GetErrors(string propertyName)
    {
        if (this._errors.ContainsKey(propertyName))
            return this._errors[propertyName];
        return null;
    }
         
    // has errors
    public bool HasErrors
    {
        get { return (this._errors.Count > 0); }
    }
 
    // object is valid
    public bool IsValid
    {
        get { return !this.HasErrors; }
 
    }
 
    public void AddError(string propertyName, string error)
    {
        // Add error to list
        this._errors[propertyName] = new List<string>() { error };
        this.NotifyErrorsChanged(propertyName);
    }
 
    public void RemoveError(string propertyName)
    {
        // remove error
        if (this._errors.ContainsKey(propertyName))
            this._errors.Remove(propertyName);
        this.NotifyErrorsChanged(propertyName);
    }
 
    public void NotifyErrorsChanged(string propertyName)
    {
        // Notify
        if (this.ErrorsChanged != null)
            this.ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        if(NotifyVM != null)
        {
            NotifyVM();
        }
    }
    #endregion
}
}
