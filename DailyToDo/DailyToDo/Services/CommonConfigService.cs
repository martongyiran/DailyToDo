using DailyToDo.Services.Interfaces;
using DailyToDo.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DailyToDo.Services
{
    public class CommonConfigService : ICommonConfigService
    {
        public OSAppTheme AppTheme
        {
            get => (OSAppTheme)GetIntValue(nameof(AppTheme), (int)OSAppTheme.Light);
            set => SetIntValue(nameof(AppTheme), (int)value);
        }

        public CultureInfo Culture
        {
            get => CultureInfo.GetCultureInfo(GetIntValue(nameof(Culture), CultureInfo.CurrentCulture.LCID));
            set => SetIntValue(nameof(Culture), value.LCID);
        }

        public List<ToDoItemViewModel> ToDoList
        {
            get => GetSecuredValue<List<ToDoItemViewModel>>(nameof(ToDoList), null);
            set => SetSecuredValue(nameof(ToDoList), value);
        }

        public bool FirstStart
        {
            get => GetBoolValue(nameof(FirstStart), true);
            set => SetBoolValue(nameof(FirstStart), value);
        }

        #region Getters, setters

        private static T GetValue<T>(string name, T defaultValue)
        {
            var objectValue = Preferences.Get(name, string.Empty);
            return string.IsNullOrEmpty(objectValue)
                ? defaultValue
                : JsonConvert.DeserializeObject<T>(objectValue);
        }

        private static void SetValue<T>(string name, T value)
        {
            string stringValue;

            try
            {
                stringValue = JsonConvert.SerializeObject(value);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Cannot serialize value.", nameof(value), ex);
            }

            Preferences.Set(name, stringValue);
        }

        private string GetStringValue(string name, string defaultValue)
            => Preferences.Get(name, defaultValue);

        private int GetIntValue(string name, int defaultValue)
            => Preferences.Get(name, defaultValue);

        private bool GetBoolValue(string name, bool defaultValue)
            => Preferences.Get(name, defaultValue);

        private void SetStringValue(string name, string value)
            => Preferences.Set(name, value);

        private void SetIntValue(string name, int value)
            => Preferences.Set(name, value);

        private void SetBoolValue(string name, bool value)
            => Preferences.Set(name, value);

        private bool HasValue(string name)
            => Preferences.ContainsKey(name);

        private void RemoveValue(string name)
            => Preferences.Remove(name);

        private T GetSecuredValue<T>(string name, T defaultValue)
        {
            var getValueTask = SecureStorage.GetAsync(name);
            getValueTask.GetAwaiter().GetResult();

            var objectValue = getValueTask.Result;

            if (objectValue == null)
            {
                var unsecureValue = GetValue(name, defaultValue);
                if (unsecureValue != null)
                {
                    return unsecureValue;
                }
            }

            return objectValue == null
                ? defaultValue
                : JsonConvert.DeserializeObject<T>(objectValue);
        }

        private void SetSecuredValue<T>(string name, T value)
        {
            try
            {
                var stringValue = JsonConvert.SerializeObject(value);
                var setValueTask = SecureStorage.SetAsync(name, stringValue);
                setValueTask.GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                SetValue(name, value);
            }
        }

        #endregion
    }
}
