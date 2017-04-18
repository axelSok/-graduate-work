using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DNFTester.Entities.Interfaces;


namespace DNFTester.Entities
{
    public class NotifyObjectChanged : INotifyObjectChanged, INotifyPropertyChanged
    {
        /// <summary>
        /// Множество свойств, которые изменились
        /// </summary>
        private HashSet<string> _propertiesChanged;

        /// <summary>
        /// Счетчик вызовов BeginChange
        /// </summary>
        private int _changing;

        /// <summary>
        /// Вызвать событие изменения свойства
        /// </summary>
        /// <remarks>property - имя свойства</remarks>
        protected void RaisePropertyChanged(string property)
        {
            var oc = ObjectChanged;
            var pc = PropertyChanged;

            if (oc != null || pc != null)
            {
                if (_changing > 0)
                {
                    lock (this)
                    {
                        if (_propertiesChanged == null) _propertiesChanged = new HashSet<string>();
                        _propertiesChanged.Add(property);
                    }
                }
                else
                {
                    if (oc != null)
                    {
                        oc(this, new ObjectChangedEventArgs(property));
                    }
                    if (pc != null)
                    {
                        pc(this, new System.ComponentModel.PropertyChangedEventArgs(property));
                    }
                }
            }

        }

        /// <summary>
        /// Вызвать собитие изменения свойств
        /// </summary>
        /// <remarks>properties - имена свойств</remarks>
        protected void RaisePropertiesChanged(params string[] properties)
        {
            var oc = ObjectChanged;
            var pc = PropertyChanged;

            if (oc != null || pc != null)
            {
                if (_changing > 0)
                {
                    lock (this)
                    {
                        if (_propertiesChanged != null)
                        {
                            foreach (var p in properties)
                            {
                                _propertiesChanged.Add(p);
                            }
                        }
                        else
                        {
                            _propertiesChanged = new HashSet<string>(properties);
                        }
                    }
                }
                else
                {
                    if (oc != null)
                    {
                        oc(this, new ObjectChangedEventArgs(properties));
                    }
                    if (pc != null)
                    {
                        foreach (var property in properties)
                        {
                            pc(this, new System.ComponentModel.PropertyChangedEventArgs(property));
                        }
                    }
                }
            }

        }

        #region Члены INotifyObjectChanged

        /// <summary>
        /// Инициализировать начало изменений объекта
        /// </summary>
        /// <remarks>Все изменения будут накапливаться в внутрений список.</remarks>
        public void BeginChange()
        {
            lock (this)
            {
                if (_changing == 0) _propertiesChanged = null;
                _changing++;
            }
        }

        /// <summary>
        /// Закончить изменение объекта
        /// </summary>
        /// <remarks>
        /// raiseEvent == true - все вызываються события накопленых изменений.
        /// andAllChanges - если true обнуляет счетчик всех вызовов BeginChange, в противном случае уменшает счетчик на 1
        /// </remarks>
        public void EndChange(bool raiseEvent, bool endAllChanges = false)
        {
            if (_changing > 0)
            {
                EventHandler<ObjectChangedEventArgs> oc = null;
                PropertyChangedEventHandler pc = null;
                string[] properties = null;
                lock (this)
                {
                    if (endAllChanges) _changing = 0; else _changing--;
                    if (_changing == 0)
                    {
                        if (raiseEvent && _propertiesChanged != null)
                        {
                            oc = ObjectChanged;
                            pc = PropertyChanged;
                            properties = _propertiesChanged.ToArray();
                        }
                        _propertiesChanged = null;
                        _changing = 0;
                    }
                }
                if (properties != null && properties.Length > 0)
                {
                    if (oc != null) oc(this, new ObjectChangedEventArgs(properties));
                    if (pc != null) foreach (var p in properties) pc(this, new PropertyChangedEventArgs(p));
                }
            }
        }

        public virtual event EventHandler<ObjectChangedEventArgs> ObjectChanged;

        #endregion

        public NotifyObjectChanged()
        {
            _changing = 0;
            _propertiesChanged = null;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
