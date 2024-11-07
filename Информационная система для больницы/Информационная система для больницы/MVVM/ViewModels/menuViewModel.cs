using System;

namespace Информационная_система_для_больницы.MVVM.ViewModels
{
    class menuViewModel : ObservableObject
    {
        public registrarMenuButtonsViewModel registrarMenuButtonsVM {  get; set; }
        public doctorMenuButtonsViewModel doctorMenuButtonsVM { get; set; }
        public nurseMenuButtonsViewModel nurseMenuButtonsVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        
        public menuViewModel()
        {
            registrarMenuButtonsVM = new registrarMenuButtonsViewModel();
            CurrentView = registrarMenuButtonsVM;
        }
    }
}
