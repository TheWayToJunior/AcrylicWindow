﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AcrylicWindow.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private bool _disposed = false;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (field == null && value == null)
                return false;

            if (field?.Equals(value) ?? false)
                return false;

            field = value;

            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// All transient IDisposable instances, as long as this area is alive,
        /// will accumulate, and we will get a form of memory leak when services
        /// remain alive far beyond their need.
        /// </summary>
        public virtual void Dispose(bool collect)
        {
            if (_disposed)
            {
                return;
            }

            if (collect)
            {
                /// Trying to clear resources. Does not guarantee deletion
                GC.Collect();
            }

            _disposed = true;
        }
    }
}
